using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Newtonsoft.Json;

namespace edb_tool
{
    public class GFile
    {
        public GFile()
        {
        }

        public GFile(int idfile, string filename, string pathname)
        {
            this.idfile = idfile;
            this.filename = filename;
            this.pathname = pathname;
        }

        [JsonProperty("idfile")]
        public int idfile { get; set; }

        [JsonProperty("filename")]
        public string filename { get; set; }

        [JsonProperty("pathname")]
        public string pathname { get; set; }

        [JsonProperty("comment")]
        public int comment { get; set; }

        [JsonProperty("sizebytes")]
        public int sizebytes { get; set; }

        [JsonProperty("date")]
        public DateTime date { get; set; }

        [JsonProperty("tags")]
        public string tags { get; set; }
    }
}
