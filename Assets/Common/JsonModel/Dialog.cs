using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Assets.Common.JsonModel
{
    //Our dialog object model for loading in from JSON
    public class DialogBag
    {
        [JsonProperty("id")]
        public string Id;

        [JsonProperty("content")]
        public List<Dialog> Content;
    }

    /*public class DialogGroup
    {
        [JsonProperty("name")]
        public string Id;

        [JsonProperty("content")]
        public List<Dialog> Content;
    }*/

    public class Dialog
    {
        [JsonProperty("id")]
        public string Id;

        [JsonProperty("text")]
        public string Text;

        //Flag used to set a dialog as active
        [JsonProperty("flag")]
        public string Flag;
    }


}
