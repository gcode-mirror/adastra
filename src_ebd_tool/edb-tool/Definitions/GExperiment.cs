using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Newtonsoft.Json;

namespace edb_tool
{
    public class GExperiment
    {
        public GExperiment()
        {
        }

        public GExperiment(int idexperiment, string name, string comment, string description, int iduser,DateTime exp_date)
        {
            this.idexperiment = idexperiment;
            this.name = name;
            this.comment = comment;
            this.description = description;
            this.iduser = iduser;
            this.exp_date = exp_date;
        }

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

        [JsonProperty("exp_date")]
        public DateTime exp_date { get; set; }

        /// <summary>
        /// You do not have full rigts on experiments that are shared to you
        /// </summary>
        public bool IsSharedToCurrentUser { get; set; }
    }
}
