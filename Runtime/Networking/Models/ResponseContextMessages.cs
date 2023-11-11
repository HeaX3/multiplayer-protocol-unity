using JetBrains.Annotations;

namespace MultiplayerProtocol
{
    public class ResponseContextMessages
    {
        [CanBeNull] public SerializedMessages preResponse { get; set; }
        [CanBeNull] public SerializedMessages postResponse { get; set; }

        public ResponseContextMessages()
        {
        }

        public ResponseContextMessages(SerializedMessages preResponse, SerializedMessages postResponse)
        {
            this.preResponse = preResponse;
            this.postResponse = postResponse;
        }

        public static ResponseContextMessages PreResponse(SerializedMessages messages)
        {
            return new ResponseContextMessages
            {
                preResponse = messages
            };
        }

        public static ResponseContextMessages PostResponse(SerializedMessages messages)
        {
            return new ResponseContextMessages
            {
                postResponse = messages
            };
        }
    }
}