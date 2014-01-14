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

        [JsonProperty("prenom")]
        public string FirstName { get; set; }

        [JsonProperty("nom")]
        public string LastName { get; set; }

        [JsonProperty("email_agalan")]
        public string EMail { get; set; }

        [JsonProperty("login_agalan")]
        public string Username { get; set; }

        public string FullName 
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }
    }
}
