using Newtonsoft.Json.Linq;
using UnityEngine;

namespace MultiplayerProtocol
{
    public class JsonValue : ISerializableValue<JObject>
    {
        public JObject value { get; set; }

        public void SerializeInto(SerializedMessage message)
        {
            message.Write(value?.ToString());
        }

        public void DeserializeFrom(SerializedMessage message)
        {
            var jsonString = message.ReadString();
            if (jsonString == null)
            {
                value = null;
                return;
            }

            try
            {
                value = JObject.Parse(jsonString);
            }
            catch
            {
                Debug.LogError("Failed parsing " + jsonString);
            }
        }
    }
}