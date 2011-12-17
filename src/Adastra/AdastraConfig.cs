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

		public static string GetDataFolder()
		{
			string folder = "";
			#if (DEBUG)
				 folder = Environment.CurrentDirectory + @"\..\..\..\..\data\";
			#else
				 folder = Environment.CurrentDirectory + @"\data\mitko-small.csv";
			#endif
				 return folder;
		}

		/// <summary>
		/// If possible locates the folder that contains OpenVibe scenarios specific for Adastra 
		/// </summary>
		/// <returns></returns>
		public static string GetOpenVibeScenarioFolder()
		{
			string scenarioFolder = Environment.CurrentDirectory + @"\..\..\..\..\scenarios\";

			if (Directory.Exists(scenarioFolder))
			{
				return scenarioFolder;
			}

			scenarioFolder = Environment.CurrentDirectory + @"\scenarios\";

			if (Directory.Exists(scenarioFolder))
			{
				return scenarioFolder;
			}
			return "";
		}
	}
}
