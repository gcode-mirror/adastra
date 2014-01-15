using System;
using System.IO;
using System.Management;    // System.Management.dll
using System.Collections.Generic;
using System.Linq;

using System.Windows.Forms;
using System.Security.Principal;
using System.Security.AccessControl;

public static class MappedDriveResolver
{

    /// <summary>Resolves the given path to a full UNC path, or full local drive path.</summary>
    /// <param name="pPath"></param>
    /// <returns></returns>
    public static string ResolveToUNC(string pPath)
    {
        if (pPath.StartsWith(@"\\")) { return pPath; }

        string root = ResolveToRootUNC(pPath);

        if (pPath.StartsWith(root))
        {
            return pPath; // Local drive, no resolving occurred
        }
        else
        {
            return pPath.Replace(GetDriveLetter(pPath), root);
        }
    }

    /// <summary>Resolves the given path to a root UNC path, or root local drive path.</summary>
    /// <param name="pPath"></param>
    /// <returns>\\server\share OR C:\</returns>
    public static string ResolveToRootUNC(string pPath)
    {
        ManagementObject mo = new ManagementObject();

        if (pPath.StartsWith(@"\\")) { return Directory.GetDirectoryRoot(pPath); }

        // Get just the drive letter for WMI call
        string driveletter = GetDriveLetter(pPath);

        mo.Path = new ManagementPath(string.Format("Win32_LogicalDisk='{0}'", driveletter));

        // Get the data we need
        uint DriveType = Convert.ToUInt32(mo["DriveType"]);
        string NetworkRoot = Convert.ToString(mo["ProviderName"]);
        mo = null;

        // Return the root UNC path if network drive, otherwise return the root path to the local drive
        if (DriveType == 4)
        {
            return NetworkRoot;
        }
        else
        {
            return driveletter + Path.DirectorySeparatorChar;
        }
    }

    public static List<edb_tool.GNetworkShare> GetRemoteMappedDrives()
    {
        List<edb_tool.GNetworkShare> mappedDrives;
        string[] drives = Environment.GetLogicalDrives();

        var q = from d in drives
                where isNetworkDrive(d)
                select new edb_tool.GNetworkShare
                {
                    name = d,
                    path = ResolveToRootUNC(d)
                };

        mappedDrives = q.ToList();

        return mappedDrives;
    }

    /// <summary>Checks if the given path is on a network drive.</summary>
    /// <param name="pPath"></param>
    /// <returns></returns>
    public static bool isNetworkDrive(string pPath)
    {
        ManagementObject mo = new ManagementObject();

        if (pPath.StartsWith(@"\\")) { return true; }

        // Get just the drive letter for WMI call
        string driveletter = GetDriveLetter(pPath);

        mo.Path = new ManagementPath(string.Format("Win32_LogicalDisk='{0}'", driveletter));

        // Get the data we need
        uint DriveType = Convert.ToUInt32(mo["DriveType"]);
        mo = null;

        return DriveType == 4;
    }

    /// <summary>Given a path will extract just the drive letter with volume separator.</summary>
    /// <param name="pPath"></param>
    /// <returns>C:</returns>
    public static string GetDriveLetter(string pPath)
    {
        if (pPath.StartsWith(@"\\")) { throw new ArgumentException("A UNC path was passed to GetDriveLetter"); }
        return Directory.GetDirectoryRoot(pPath).Replace(Path.DirectorySeparatorChar.ToString(), "");
    }

    public static List<edb_tool.GNetworkShare> GetSharedFolders()
    {
        List<edb_tool.GNetworkShare> sharedFolders;

        // Object to query the WMI Win32_Share API for shared files...

        ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from win32_share");

        ManagementBaseObject outParams;

        ManagementClass mc = new ManagementClass("Win32_Share"); //for local shares

        var q = from ManagementObject share in searcher.Get()
                let type = share["Type"].ToString()
                where type == "0"
                select new edb_tool.GNetworkShare()
                {
                      name = share["Name"].ToString(), //getting share name

                      path = share["Path"].ToString(), //getting share path

                      //caption = share["Caption"].ToString(), //getting share description
                };


        sharedFolders = q.ToList();
        return sharedFolders;
    }

     public static List<edb_tool.GFile> ReplaceLocalPathWithShared(List<edb_tool.GFile> files, List<edb_tool.GNetworkShare> localShares)
     {
         foreach (edb_tool.GFile file in files)
         {
             foreach (edb_tool.GNetworkShare share in localShares)
             {
                 if (file.pathname.StartsWith(share.path))
                 {
                     int pos = share.path.Length;
                     file.pathname = @"\\" + System.Environment.MachineName + @"\" + share.name + file.pathname.Substring(pos);
                 }
             }
         }



         return files;
     }

     public static void QshareFolder(string FolderPath, string ShareName, string Description)
     {
         try
         {
             // Create a ManagementClass object
             ManagementClass managementClass = new ManagementClass("Win32_Share");

             // Create ManagementBaseObjects for in and out parameters
             ManagementBaseObject inParams = managementClass.GetMethodParameters("Create");
             ManagementBaseObject outParams;

             // Set the input parameters
             inParams["Description"] = Description;
             inParams["Name"] = ShareName;
             inParams["Path"] = FolderPath;
             inParams["Type"] = 0x0; // Disk Drive
             //Another Type:
             //        DISK_DRIVE = 0x0
             //        PRINT_QUEUE = 0x1
             //        DEVICE = 0x2
             //        IPC = 0x3
             //        DISK_DRIVE_ADMIN = 0x80000000
             //        PRINT_QUEUE_ADMIN = 0x80000001
             //        DEVICE_ADMIN = 0x80000002
             //        IPC_ADMIN = 0x8000003
             inParams["MaximumAllowed"] = null;
             inParams["Password"] = null;
             inParams["Access"] = null; // Make Everyone has full control access.                
             //inParams["MaximumAllowed"] = int maxConnectionsNum;

             // Invoke the method on the ManagementClass object
             outParams = managementClass.InvokeMethod("Create", inParams, null);
             // Check to see if the method invocation was successful
             if ((uint)(outParams.Properties["ReturnValue"].Value) != 0)
             {
                 //MessageBox.Show ("Unable to share the folder: " + outParams.Properties["ReturnValue"].Value);
                 MessageBox.Show("Error sharing MMS folders. Please make sure you install as Administrator.", "Error!");
                 return;
             }

             //user selection
             NTAccount ntAccount = new NTAccount("Everyone");

             //SID
             SecurityIdentifier userSID = (SecurityIdentifier)ntAccount.Translate(typeof(SecurityIdentifier));
             byte[] utenteSIDArray = new byte[userSID.BinaryLength];
             userSID.GetBinaryForm(utenteSIDArray, 0);

             //Trustee
             ManagementObject userTrustee = new ManagementClass(new ManagementPath("Win32_Trustee"), null);
             userTrustee["Name"] = "Everyone";
             userTrustee["SID"] = utenteSIDArray;

             //ACE
             ManagementObject userACE = new ManagementClass(new ManagementPath("Win32_Ace"), null);
             userACE["AccessMask"] = 2032127;                                 //Full access
             userACE["AceFlags"] = AceFlags.ObjectInherit | AceFlags.ContainerInherit;
             userACE["AceType"] = AceType.AccessAllowed;
             userACE["Trustee"] = userTrustee;

             ManagementObject userSecurityDescriptor = new ManagementClass(new ManagementPath("Win32_SecurityDescriptor"), null);
             userSecurityDescriptor["ControlFlags"] = 4; //SE_DACL_PRESENT 
             userSecurityDescriptor["DACL"] = new object[] { userACE };
             //can declare share either way, where "ShareName" is the name used to share the folder
             //ManagementPath path = new ManagementPath("Win32_Share.Name='" + ShareName + "'");
             //ManagementObject share = new ManagementObject(path);
             ManagementObject share = new ManagementObject(managementClass.Path + ".Name='" + ShareName + "'");

             share.InvokeMethod("SetShareInfo", new object[] { Int32.MaxValue, Description, userSecurityDescriptor });

         }
         catch (Exception ex)
         {
             MessageBox.Show("Error sharing folders. Please make sure you install as Administrator. ERROR: " + ex.Message, "Error!");
         }
     }

}