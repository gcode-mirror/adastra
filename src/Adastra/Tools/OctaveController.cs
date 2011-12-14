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
		public static bool NoGUI = true;

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern uint GetShortPathName([MarshalAs(UnmanagedType.LPTStr)] string lpszLongPath, [MarshalAs(UnmanagedType.LPTStr)]StringBuilder lpszShortPath, uint cchBuffer);

		static string folder = LocateOctaveInstallDir();
		//static string executable = Directory.GetFiles(folder + "bin").Where(p => Path.GetFileName(p).StartsWith("octave-") && Path.GetFileName(p).Length<=16).FirstOrDefault();
        static string executable = folder + "bin\\octave.exe";

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

				psi.RedirectStandardOutput = true;
				psi.RedirectStandardInput = true;
				psi.RedirectStandardError = true;

                if (NoGUI)
                {
					psi.CreateNoWindow = true;
                    psi.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                   
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

		public static string SaveTempFile(List<double[]> values)
		{
			string tempFile = Path.GetTempFileName();

			StreamWriter outfile =new StreamWriter(tempFile);
			try
			{
				foreach (var row in values)
				{
					for (int i = 0; i < row.Length; i++)
					{
						if (i == row.Length - 1) outfile.WriteLine(row[i]);
						else outfile.Write(row[i] + " ");
					}
				}

			}
			catch (Exception ex)
			{
				int a;
			}
			finally
			{
				if (outfile != null)
				{
					outfile.Close();
					outfile = null;
				}
			}

			return tempFile;
		}

		private static string LocateOctaveInstallDir()
		{
            string candidate = System.Configuration.ConfigurationManager.AppSettings["OctaveInstallDir"];
            if (!string.IsNullOrEmpty(candidate)) return candidate; 

			string[] baseFolders = { @"c:\Program Files\",  @"C:\Program Files (x86)\", @"D:\Program Files\", @"D:\", @"D:\Work_anton\"};

			foreach (string folder in baseFolders.Where(p => Directory.Exists(p)))
			{
				candidate = Directory.GetDirectories(folder).Where(p => p.Contains("octave_")).FirstOrDefault();
				if (!string.IsNullOrEmpty(candidate)) break;
			}

			if (string.IsNullOrEmpty(candidate)) throw new Exception("Octave installation could not be detected!");
			else
				return candidate+"\\";
		}

        public static string GetBaseScriptPath()
        {
            string folder = "";
            #if (DEBUG)
            folder = Environment.CurrentDirectory + @"\..\..\..\..\scripts\octave\";
            #else
				folder = Environment.CurrentDirectory + @"\scripts\octave\";
            #endif

            return folder;
        }

    }
}
