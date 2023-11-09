using Newtonsoft.Json.Linq;
using UnityEngine;

namespace MultiplayerProtocol
{
    public class JsonArrayValue : ISerializableValue<JArray>
    {
        public JArray value { get; set; }

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
                value = JArray.Parse(jsonString);
            }
            catch
            {
                Debug.LogError("Failed parsing " + jsonString);
            }
        }
    }
}