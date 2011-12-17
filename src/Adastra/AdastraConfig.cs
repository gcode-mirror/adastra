using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Adastra
{
	public static class AdastraConfig
	{
		/// <summary>
		/// Returns the path where m scripts are contained.
		/// </summary>
		/// <returns></returns>
		public static string GetBaseOctaveScriptPath()
		{
			string folder = "";
			#if (DEBUG)
				 folder = Environment.CurrentDirectory + @"\..\..\..\..\scripts\octave\";
			#else
			     folder = Environment.CurrentDirectory + @"\scripts\octave\";
			#endif
			return folder;
		}

		/// <summary>
		/// Returns the path where sample EEG records are contained and .
		/// </summary>
		/// <returns></returns>
		public static string GetDataFolder()
		{
			string folder = "";
			#if (DEBUG)
				 folder = Environment.CurrentDirectory + @"\..\..\..\..\data\";
			#else
				 folder = Environment.CurrentDirectory + @"\data\";
			#endif
				 return folder;
		}

		/// <summary>
		/// If possible locates the folder that contains OpenVibe scenarios specific for Adastra 
		/// </summary>
		/// <returns></returns>
		public static string GetOpenVibeScenarioFolder()
		{
			string folder = "";
            #if (DEBUG)
			folder = Environment.CurrentDirectory + @"\..\..\..\..\scenarios\";
			#else
				 folder = Environment.CurrentDirectory + @"\scenarios\";
			#endif
			return folder;
		}

		public static string GetRecordsFolder()
		{
            string folder = GetDataFolder() + @"records\";
			return folder;
		}
	}
}
