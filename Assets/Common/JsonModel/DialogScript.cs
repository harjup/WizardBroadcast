using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Assets.Common.JsonModel
{
    public class DialogScriptBag
    {
        [JsonProperty("scriptId")]
        public string ScriptId;

        [JsonProperty("content")]
        public List<DialogScript> Content;
    }

    public class DialogScript
    {
        [JsonProperty("id")]
        public string Id;

        [JsonProperty("text")]
        public string Text;
    }


}
