using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace edb_tool
{
    public class ProviderFactory
    { 
        static DataProvider provider;
        static string WebProvider;

        public static DataProvider GetDataProvider()
        {
            if (provider == null) provider = new edb_tool.WebService();//
            //if (provider == null) provider = new MySql();

            return provider;
        }

        public static string[] GetWebProvidersList()
        {
            return new string[] { "http://si-devel.gipsa-lab.grenoble-inp.fr/edm/", "http://localhost/edb-json/" };
        }

        public static void SetWebProvider(string url)
        {
            WebProvider = url;
        }

        public static string GetWebProvider()
        {
            return WebProvider;
        }
    }
}
