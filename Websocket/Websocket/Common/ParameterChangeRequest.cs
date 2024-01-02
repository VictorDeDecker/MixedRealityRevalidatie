using Newtonsoft.Json;

namespace Websocket.Common
{
    [Serializable]
    public class ParameterChangeRequest
    {
        [JsonProperty("script")]
        public string script { get; set; }

        [JsonProperty("parameter")]
        public string parameter { get; set; }

        [JsonProperty("value")]
        public float value { get; set; }
    }
}