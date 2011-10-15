using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Eloquera.Client;

namespace Adastra
{
    public class DbSettings
    {
        const string dbName = "AdastraDB";

        public static string fullpath = Environment.CurrentDirectory + "\\" + dbName;

        public static string ConnectionString = "server=(local);options=none;";

        //public static DB db = new DB("server=(local);password=;options=inmemory,persist;");//in-memory save on exit
    }
}
