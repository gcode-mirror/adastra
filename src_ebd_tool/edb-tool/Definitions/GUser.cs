using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Newtonsoft.Json;

namespace edb_tool
{
    public class GUser
    {
        [JsonProperty("iduser")]
        public string iduser { get; set; }

        [JsonProperty("FirstName")]
        public string Name { get; set; }

        [JsonProperty("Email")]
        public string Mail { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }
        //public int Price { get; set; }
        //public int Area { get; set; }
        //public string City { get; set; }
        //public string Link { get; set; }
    }
}
