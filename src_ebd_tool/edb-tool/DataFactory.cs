using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace edb_tool
{
    public class DataFactory
    { 
        static DataProvider provider;

        public static DataProvider GetDataProvider()
        {
            if (provider == null) provider = new edb_tool.WebService();//new MySql();

            return provider;
        }
    }
}
