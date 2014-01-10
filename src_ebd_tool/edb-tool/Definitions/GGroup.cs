using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Newtonsoft.Json;

namespace edb_tool
{
    public class GGroup
    {
        [JsonProperty("id_groupe")]
        public int idgroup { get; set; }

        [JsonProperty("si_groupe")]
        public string Name { get; set; }

        [JsonProperty("mail")]
        public string ShortName { get; set; }

        public string EMail 
        {
            get
            {
                return ShortName + "@gipsa-lab.grenoble-inp.fr";
            }
        }
    }
}
