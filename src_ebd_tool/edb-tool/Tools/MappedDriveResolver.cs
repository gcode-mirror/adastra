using System;
using System.IO;
using System.Management;    // System.Management.dll
using System.Collections.Generic;

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

    public static List<string> GetSharedFolders()
    {

        List<string> sharedFolders = new List<string>();

        // Object to query the WMI Win32_Share API for shared files...

        ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from win32_share");

        ManagementBaseObject outParams;

        ManagementClass mc = new ManagementClass("Win32_Share"); //for local shares

        foreach (ManagementObject share in searcher.Get())
        {

            string type = share["Type"].ToString();

            if (type == "0") // 0 = DiskDrive (1 = Print Queue, 2 = Device, 3 = IPH)
            {
                string name = share["Name"].ToString(); //getting share name

                string path = share["Path"].ToString(); //getting share path

                string caption = share["Caption"].ToString(); //getting share description

                sharedFolders.Add(path);
            }

        }

        return sharedFolders;

    }

}