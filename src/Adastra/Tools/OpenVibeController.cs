using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;
using System.Threading;

namespace Adastra
{
    /// <summary>
    /// Starts and stops an OpenVibe instance.
    /// </summary>
    public class OpenVibeController
    {
        public static string OpenVibeDesignerWorkingFolder;
        public static string Scenario;
        public static bool NoGUI = false;
        public static bool FastPlay = false;

        private static bool started=false;

        #region unmanaged code hooks
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32")]
        private static extern int SetForegroundWindow(IntPtr hwnd);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern uint GetShortPathName([MarshalAs(UnmanagedType.LPTStr)] string lpszLongPath, [MarshalAs(UnmanagedType.LPTStr)]StringBuilder lpszShortPath, uint cchBuffer);
        #endregion

        /// <summary>
        /// Starts OpenVibes's executable
        /// </summary>
        /// <param name="run">'true' to execute scenario, 'false' to edit scenario with OpenVibe</param>
        public static void Start(bool run)
        {
            if (!OpenVibeDesignerWorkingFolder.EndsWith("\\")) OpenVibeDesignerWorkingFolder += "\\";

            string executable = OpenVibeDesignerWorkingFolder+"ov-designer.cmd";
            if (!System.IO.File.Exists(executable))
                executable = OpenVibeDesignerWorkingFolder+"openvibe-designer.cmd";

            if (!System.IO.File.Exists(executable)) { System.Windows.Forms.MessageBox.Show("Executable not found!"); return; }

			FixParametersBug(executable);

            string parameters = " --no-session-management "; //neither restore last used scenarios nor saves them at exit

            if (run)
            {
                if (FastPlay) parameters += " --play-fast " + GetDosPathName(Scenario);
                else parameters += " --play " + GetDosPathName(Scenario);
            }
            else //else edit
            {
                parameters += " --open " + GetDosPathName(Scenario);
            }

            if (NoGUI) parameters += " --no-gui ";
            System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo(GetDosPathName(executable), parameters);
            
            psi.WorkingDirectory = GetDosPathName(OpenVibeDesignerWorkingFolder);
            psi.RedirectStandardOutput = false;

            //if (NoGUI)
            //{
            //    psi.CreateNoWindow = true;
            //    psi.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            //    psi.RedirectStandardOutput = true;
            //    psi.RedirectStandardInput = true;
            //    psi.RedirectStandardError = true;

            //    psi.UseShellExecute = false;
            //}
            //else
            {
                psi.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
            }

            started = true;
            System.Diagnostics.Process.Start(psi);    
        }

        public static void Stop()
        {
            if (started)
            {
                //title can be set with the ms-dos command "title" which can be used below
				IntPtr ptr = FindWindow(null, Environment.GetFolderPath(Environment.SpecialFolder.System) + @"\cmd.exe");

                if (ptr != null)
                {
                    SetForegroundWindow(ptr);
                    System.Threading.Thread.Sleep(1000);
                    WindowsInput.InputSimulator.SimulateModifiedKeyStroke(WindowsInput.VirtualKeyCode.CONTROL, WindowsInput.VirtualKeyCode.CANCEL);

                    System.Threading.Thread.Sleep(1000);
                    WindowsInput.InputSimulator.SimulateTextEntry(GetConfirmationSymbol());
                    WindowsInput.InputSimulator.SimulateKeyPress(WindowsInput.VirtualKeyCode.RETURN);
                }
                else
                {
                    throw new Exception("Window not found!");
                }

				started = false;
            }
        }

        #region Get Yes in any culture
        private static string GetConfirmationSymbol()
        {
            string yes = "";
            try
            {
                yes = LoadResource("user32.dll", 805, "Yes");
            }
            catch
            {}
            if (!string.IsNullOrEmpty(yes))
            {
                return yes.Substring(1, 1);
            }
            else return "Y";      
        }

        private static string LoadResource(string libraryName, uint Ident, string DefaultText)
        {
            IntPtr libraryHandle = GetModuleHandle(libraryName);
            if (libraryHandle != IntPtr.Zero)
            {
                StringBuilder sb = new StringBuilder(1024);
                int size = LoadString(libraryHandle, Ident, sb, 1024);
                if (size > 0)
                    return sb.ToString();
                else
                    return DefaultText;
            }
            else
            {
                return DefaultText;
            }
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int LoadString(IntPtr hInstance, uint uID, StringBuilder lpBuffer, int nBufferMax);
        #endregion

        public static bool IsRunning
        {
            get
            {
                return started;
            }
        }

        #region helper methods
        private static void FixParametersBug(string file)
		{
			string text = File.ReadAllText(file);

			string[] lines = text.Split(new char[] {'\r','\n'});

			bool replace = false;
			for (int i = 0; i < lines.Length;i++)
			{
				if (lines[i].ToLower().EndsWith("OpenViBE-designer-dynamic.exe".ToLower()))
				{
					lines[i] += " %1 %2 %3 %4 %5 %6";
					replace = true;
				}
			}

			if (replace)
			{
				TextWriter tw = new StreamWriter(file,false);

				// write a line of text to the file
				foreach (string line in lines)
				{
					tw.WriteLine(line);
				}

				// close the stream
				tw.Close();
			}
		}

        /// <summary>
        /// If possible locates OpenVibe's installation folder
        /// </summary>
        /// <returns></returns>
        public static string DetectOpenVibeInstallFolder()
        {
            string openVibeLocation = "";

             #if (DEBUG)
             openVibeLocation = @"D:\Work\OpenVibe_src\dist\";
             if (Directory.Exists(openVibeLocation)) return openVibeLocation;
             #endif

            openVibeLocation = (string)Microsoft.Win32.Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\openvibe", "InstallDir", "");
            if (!string.IsNullOrWhiteSpace(openVibeLocation)) return openVibeLocation;

            openVibeLocation = (string)Microsoft.Win32.Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\openvibe", "InstallDir", "");
            if (!string.IsNullOrWhiteSpace(openVibeLocation)) return openVibeLocation;

            string programFiles = "";
 
             if (8 == IntPtr.Size
                || (!String.IsNullOrEmpty(Environment.GetEnvironmentVariable("PROCESSOR_ARCHITEW6432"))))
             {
                 programFiles = Environment.GetEnvironmentVariable("ProgramFiles(x86)");
             }

             if (programFiles == "") programFiles=Environment.GetEnvironmentVariable("ProgramFiles");

             openVibeLocation = programFiles + "\\openvibe";
             if (Directory.Exists(openVibeLocation))
             {
                 return openVibeLocation;
             }

			 openVibeLocation = "d" + openVibeLocation.Substring(1);
			 if (Directory.Exists(openVibeLocation))
			 {
				 return openVibeLocation;
			 }

             return "";
        }

        public static string GetDosPathName(string longName)
        {
            uint bufferSize = 256;

            // don´t allocate stringbuilder here but outside of the function for fast access
            StringBuilder shortNameBuffer = new StringBuilder((int)bufferSize);

            uint result = GetShortPathName(longName, shortNameBuffer, bufferSize);

			if (!string.IsNullOrEmpty(longName) && (shortNameBuffer != null && shortNameBuffer.ToString() == ""))
                throw new Exception("GetDosPathName: Check your file paths!\r\n\r\n");

            return shortNameBuffer.ToString();
        }
        #endregion
    }
}
