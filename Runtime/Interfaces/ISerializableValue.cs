namespace MultiplayerProtocol
{
    public interface ISerializableValue
    {
        /// <summary>
        /// Write data to the serialized message
        /// </summary>
        /// <param name="message"></param>
        void SerializeInto(SerializedMessage message);
        
        /// <summary>
        /// Read data from the serialized message
        /// </summary>
        /// <param name="message"></param>
        void DeserializeFrom(SerializedMessage message);
    }

    public interface ISerializableValue<T> : ISerializableValue
    {
        T value { get; set; }
    }
}