using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Newtonsoft.Json;

namespace edb_tool
{
    /// <summary>
    /// Modality is: fMRI, EEG, Audio, Video, etc
    /// </summary>
    public class GModality
    {
        public GModality()
        {
        }

        public GModality(int idmodality, string name, string comment, string description)
        {
            this.idmodality = idmodality;
            this.name = name;
            this.comment = comment;
            this.description = description;
        }

        [JsonProperty("idmodality")]
        public int idmodality;

        [JsonProperty("name")]
        public string name;

        [JsonProperty("comment")]
        public string comment;

        [JsonProperty("description")]
        public string description;

    }
}
