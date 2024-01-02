using Newtonsoft.Json;

namespace Websocket.Common
{
    [Serializable]
    public class SceneChange
    {
        [JsonProperty("destinationScene")]
        public string destinationScene { get; set; }
    }
}