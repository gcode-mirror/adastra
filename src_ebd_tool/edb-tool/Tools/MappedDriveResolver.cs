using System;
using System.IO;
using System.Management;    // System.Management.dll
using System.Collections.Generic;
using System.Linq;

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

}