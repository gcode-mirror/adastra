using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Newtonsoft.Json;

namespace edb_tool
{
    public class GExperiment
    {
        [JsonProperty("idexperiment")]
        public int idexperiment { get; set; }

        [JsonProperty("name")]
        public string name { get; set; }

        [JsonProperty("comment")]
        public string comment { get; set; }

        [JsonProperty("description")]
        public string description { get; set; }

        [JsonProperty("iduser")]
        public int iduser { get; set; }
    }
}
