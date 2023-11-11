namespace MultiplayerProtocol
{
    public interface ISerializableValue
    {
        /// <summary>
        /// Write data to the serialized message
        /// </summary>
        /// <param name="message"></param>
        void SerializeInto(SerializedData message);

        /// <summary>
        /// Read data from the serialized message
        /// </summary>
        /// <param name="message"></param>
        void DeserializeFrom(SerializedData message);

        /// <summary>
        /// Serialize this value into a generic message block. This message will NOT contain a message id.
        /// </summary>
        SerializedData Serialize()
        {
            var message = new SerializedData();
            SerializeInto(message);
            return message;
        }
    }

    public interface ISerializableValue<T> : ISerializableValue
    {
        T value { get; set; }
    }
}