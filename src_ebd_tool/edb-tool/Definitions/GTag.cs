using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Newtonsoft.Json;

namespace edb_tool
{
    /// <summary>
    /// Tags are applied to files
    /// </summary>
    public class GTag
    {
        public GTag()
        {
        }

        public GTag(int idtag, string name)
        {
            this.idtag = idtag;
            this.name = name;
        }

        [JsonProperty("idtag")]
        public int idtag { get; set; }

        [JsonProperty("name")]
        public string name { get; set; }
    }
}
