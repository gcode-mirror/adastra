using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace CursorControl
{
    public class OpenVibeController
    {
        public static string OpenVibeDesignerWorkingFolder;
        public static string Scenario;
        public static bool NoGUI = false;
        public static bool FastPlay = false;

        private static bool started=false;

        public static void Start()
        {
            if (!OpenVibeDesignerWorkingFolder.EndsWith("\\")) OpenVibeDesignerWorkingFolder += "\\";

            string executable = OpenVibeDesignerWorkingFolder+"ov-designer.cmd";
            if (!System.IO.File.Exists(executable))
                executable = OpenVibeDesignerWorkingFolder+"openvibe-designer.cmd";
            if (!System.IO.File.Exists(executable)) { System.Windows.Forms.MessageBox.Show("Executable not found!"); return; }

            string parameters="";
            if (FastPlay) parameters+= " --play-fast " + Scenario;
            else parameters += " --play " + Scenario;

            if (NoGUI) parameters += " --no-gui ";
            System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo(executable, parameters);
            
            psi.WorkingDirectory = OpenVibeDesignerWorkingFolder;
            psi.RedirectStandardOutput = false;

            //if (NoGUI)
            //{
            //    psi.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            //    psi.RedirectStandardOutput = true;
            //    psi.RedirectStandardInput = true;
            //    psi.RedirectStandardError = true;
            //}
            //else
            //    psi.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;

            psi.UseShellExecute = false;

            started = true;
            System.Diagnostics.Process.Start(psi);    
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32")]
        private static extern int SetForegroundWindow(IntPtr hwnd);


        public static void Stop()
        {
            if (started)
            {
                //title can be set with the ms-dos command "title" which can be used below
                IntPtr ptr = FindWindow(null, @"C:\Windows\System32\cmd.exe");

                if (ptr != null)
                {
                    SetForegroundWindow(ptr);
                    System.Threading.Thread.Sleep(1000);
                    WindowsInput.InputSimulator.SimulateModifiedKeyStroke(WindowsInput.VirtualKeyCode.CONTROL, WindowsInput.VirtualKeyCode.CANCEL);

                    System.Threading.Thread.Sleep(1000);
                    WindowsInput.InputSimulator.SimulateTextEntry("Y");
                    WindowsInput.InputSimulator.SimulateKeyPress(WindowsInput.VirtualKeyCode.RETURN);

                    started = false;
                }
                else
                {
                    throw new Exception("Window not found!");
                }
            }
        }
    }
}
