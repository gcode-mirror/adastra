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
        public int iduser { get; set; }

        [JsonProperty("FirstName")]
        public string FirstName { get; set; }

        [JsonProperty("LastName")]
        public string LastName { get; set; }

        [JsonProperty("Email")]
        public string EMail { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }
    }
}
