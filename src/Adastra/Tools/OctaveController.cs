using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;

namespace Adastra
{
    /// <summary>
    /// Starts and stops Octave (Matlab compatible product).
    /// </summary>
    public class OctaveController
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern uint GetShortPathName([MarshalAs(UnmanagedType.LPTStr)] string lpszLongPath, [MarshalAs(UnmanagedType.LPTStr)]StringBuilder lpszShortPath, uint cchBuffer);

		static string folder = LocateOctaveInstallDir();
        static string executable = folder + "octave-3.2.4.exe";
		//\..\..\..\..\scripts\octave\LinearRegression
		public static string FunctionSearchPath = @"D:\Work_anton\anton_work\Adastra\scripts\octave\LinearRegression";

        /// <summary>
        /// Example script="A=[1 2]; B=[3; 4]; C=A*B";
        /// </summary>
        /// <param name="script"></param>
        /// <returns>result from Octave</returns>
        public static string Execute(string script)
        {
            //--eval is limitted to Windows Command Line Buffer, so this why temporay files are used
            string output = "";
			script = "addpath('" + FunctionSearchPath + "');\r\n" + script;//addpath (genpath ("~/octave")); //for recursive search
            string tempFile = SaveTempFile(script);

            try
            {
                //string param = "--eval \"" + script + "\" -p " + FunctionSearchPath;
				string param = tempFile; //+ " -p " + FunctionSearchPath;

                System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo(GetDosPathName(executable), param);
                psi.WorkingDirectory = GetDosPathName(folder);

                psi.RedirectStandardOutput = false;

                bool NoGUI = true;
                if (NoGUI)
                {
                    psi.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                    psi.RedirectStandardOutput = true;
                    psi.RedirectStandardInput = true;
                    psi.RedirectStandardError = true;
                }
                else
                    psi.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;

                psi.UseShellExecute = false;

                //started = true;
                System.Diagnostics.Process p = System.Diagnostics.Process.Start(psi);
                output = p.StandardOutput.ReadToEnd();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (File.Exists(tempFile))
                  File.Delete(tempFile);
            }

            return ParseResult(output);
        }

        //public static string Interactive(string script)
        //{
        //    System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo(GetDosPathName(executable), "");
        //    psi.WorkingDirectory = GetDosPathName(folder);

        //    psi.RedirectStandardOutput = false;

        //    bool NoGUI = true;
        //    if (NoGUI)
        //    {
        //        psi.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
        //        psi.RedirectStandardOutput = true;
        //        psi.RedirectStandardInput = true;
        //        psi.RedirectStandardError = true;
        //    }
        //    else
        //        psi.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;

        //    psi.UseShellExecute = false;

        //    //started = true;
        //    System.Diagnostics.Process p = System.Diagnostics.Process.Start(psi);

        //    p.StandardInput.WriteLine("a=1");
        //    p.StandardInput.WriteLine("b=2");
        //    string output = p.StandardOutput.ReadToEnd();

        //    return output;
        //}

        private static string ParseResult(string output)
        {
            int pos = output.IndexOf("`news'.");

            return (pos != -1) ? output.Substring(pos + 7) : output;
        }

        private static string GetDosPathName(string longName)
        {
            uint bufferSize = 256;

            // don´t allocate stringbuilder here but outside of the function for fast access
            StringBuilder shortNameBuffer = new StringBuilder((int)bufferSize);

            uint result = GetShortPathName(longName, shortNameBuffer, bufferSize);

            if (!string.IsNullOrEmpty(longName) && (shortNameBuffer != null && shortNameBuffer.ToString() == ""))
                throw new Exception("GetDosPathName: Check your file paths!\r\n\r\n");

            return shortNameBuffer.ToString();
        }

        public static string SaveTempFile(string conent)
        {
            string tempFile = Path.GetTempFileName();

            using (StreamWriter outfile =
            new StreamWriter(tempFile))
            {
                outfile.Write(conent);
            }

            return tempFile;
        }

		private static string LocateOctaveInstallDir()
		{
			string folder=@"D:\Program Files\octave_3.2.4_gcc-4.4.0\bin\";
			if (Directory.Exists(folder)) return folder;
            folder = @"D:\Octave\3.2.4_gcc-4.4.0\bin\";
			if (Directory.Exists(folder)) return folder;
		    folder = @"D:\Work_anton\octave_3.2.4_gcc-4.4.0\bin\";
			if (Directory.Exists(folder)) return folder;
			folder = @"c:\Program Files\octave_3.2.4_gcc-4.4.0\bin\";
			if (Directory.Exists(folder)) return folder;

			return "octave not found";
		}

    }
}
