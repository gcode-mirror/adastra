using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Adastra
{
    /// <summary>
    /// Starts and stops Octave (Matlab compatible product).
    /// </summary>
    public class OctaveController
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern uint GetShortPathName([MarshalAs(UnmanagedType.LPTStr)] string lpszLongPath, [MarshalAs(UnmanagedType.LPTStr)]StringBuilder lpszShortPath, uint cchBuffer);

        static string folder = @"D:\Program Files\octave_3.2.4_gcc-4.4.0\bin\";
        static string executable = folder + "octave-3.2.4.exe";

        /// <summary>
        /// Example script="A=[1 2]; B=[3; 4]; C=A*B";
        /// </summary>
        /// <param name="script"></param>
        /// <returns>result from Octave</returns>
        public static string Execute(string script)
        {
           
            System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo(GetDosPathName(executable), "--eval \"" + script + "\"");
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

            string output = p.StandardOutput.ReadToEnd();

            return ParseResult(output);
        }

        private static string ParseResult(string output)
        {
            int pos = output.IndexOf("`news'.");
            return output.Substring(pos + 7);
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
    }
}
