using Newtonsoft.Json.Linq;

namespace MultiplayerProtocol
{
    public class JsonArrayValue : ISerializableValue<JArray>
    {
        public JArray value { get; set; }
        public bool useCompression { get; }

        public JsonArrayValue(bool useCompression = false)
        {
            this.useCompression = useCompression;
        }

        public void SerializeInto(SerializedData message)
        {
            message.Write(value, useCompression);
        }

        public void DeserializeFrom(SerializedData message)
        {
            value = message.ReadJsonArray(decompress: useCompression);
        }
    }
}