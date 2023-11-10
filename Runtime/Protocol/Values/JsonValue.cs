using GZipCompress;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace MultiplayerProtocol
{
    public class JsonValue : ISerializableValue<JObject>
    {
        public JObject value { get; set; }
        public bool useCompression { get; }

        public JsonValue(bool useCompression = false)
        {
            this.useCompression = useCompression;
        }

        public void SerializeInto(SerializedMessage message)
        {
            var stringValue = value?.ToString();
            if (useCompression && stringValue != null)
            {
                stringValue = GZipCompressor.CompressString(stringValue);
            }

            message.Write(stringValue);
        }

        public void DeserializeFrom(SerializedMessage message)
        {
            var jsonString = message.ReadString();
            if (jsonString == null)
            {
                value = null;
                return;
            }

            if (useCompression) jsonString = GZipCompressor.DecompressString(jsonString);

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