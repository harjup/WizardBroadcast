using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Assets.Common.JsonModel
{
    //Our dialog object model for loading in from JSON
    public class DialogBag
    {
        [JsonProperty("id")]
        public string Id;

        [JsonProperty("name")]
        public string Name;

        [JsonProperty("mumble")]
        [JsonConverter(typeof(StringEnumConverter))]
        public MumbleType Mumble;

        [JsonProperty("content")]
        public List<Dialog> Content;
    }

    public class Dialog
    {
        [JsonProperty("id")]
        public string Id;

        [JsonProperty("text")]
        public string Text;

        //Flag used to set a dialog as active
        [JsonProperty("flag")]
        public string Flag;

        //For some reason you can only access dialog by the inividial pieces, not the bag
        //So we have to shove shared properies from the bag onto the individual pieces when we access them
        //Not great!!! 
        public string Name;
        public MumbleType Mumble;
    }


}
