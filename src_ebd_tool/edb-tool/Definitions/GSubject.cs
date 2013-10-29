using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Newtonsoft.Json;

namespace edb_tool
{
    public class GSubject
    {
        public GSubject()
        {
        }

        public GSubject(int id, string name, int? age, int sex, int idexperiment,int? iduser)
        {
            this.idsubject = id;
            this.name = name;
            this.age = age;
            this.sex = sex;
            this.idexperiment = idexperiment;
            this.iduser = iduser;
        }

        [JsonProperty("idsubject")]
        public int idsubject { get; set; }

        [JsonProperty("name")]
        public string name { get; set; } 

        [JsonProperty("age")]
        public int? age { get; set; }

        [JsonProperty("sex")]
        public int sex { get; set; }

        [JsonProperty("idexperiment")]
        public int idexperiment { get; set; }

        [JsonProperty("iduser")]
        public int? iduser { get; set; }
    }
}
