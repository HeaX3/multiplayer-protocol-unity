using System;
using JetBrains.Annotations;

namespace MultiplayerProtocol
{
    public class RequestResponse : IRequestResponse
    {
        public StatusCode status { get; }

        /// <summary>
        /// Bundle of additional messages which should be received before the response is handled
        /// </summary>
        public SerializedMessages preResponse { get; set; }

        /// Bundle of additional messages which should be received after the response is handled, but before the requesting application logic continues
        public SerializedMessages postResponse { get; set; }

        private readonly SerializedData message;
        private readonly Exception _error;

        public bool isError { get; }

        /// <summary>
        /// Create a new request response and set the response status code to <see cref="StatusCode"/>.OK
        /// </summary>
        /// <param name="message">Serialized response value</param>
        public RequestResponse([CanBeNull] SerializedData message)
            : this(StatusCode.Ok, message)
        {
        }

        /// <summary>
        /// Create a new request response using a custom <see cref="StatusCode"/>
        /// </summary>
        /// <param name="status">Response status</param>
        /// <param name="message">Serialized response value</param>
        public RequestResponse(StatusCode status, [CanBeNull] SerializedData message)
        {
            this.status = status;
            this.message = message;
            _error = null;
            isError = false;
        }

        /// <summary>
        /// Create a new request response and set the response status code to <see cref="StatusCode"/>.OK
        /// </summary>
        /// <param name="value">Serializable response value</param>
        public RequestResponse([CanBeNull] ISerializableValue value = null)
        {
            status = StatusCode.Ok;
            message = value?.Serialize();
            _error = null;
            isError = false;
        }

        /// <summary>
        /// Create a new error response using a custom <see cref="StatusCode"/>
        /// </summary>
        /// <param name="status">Response status code</param>
        /// <param name="error">The exception</param>
        public RequestResponse(StatusCode status, [NotNull] Exception error)
        {
            this.status = status;
            message = null;
            _error = error;
            isError = true;
        }

        public Exception error()
        {
            return _error;
        }

        [CanBeNull]
        public T value<T>() where T : ISerializableValue, new()
        {
            if (message == null) return default;

            var result = new T();
            result.DeserializeFrom(message);
            return result;
        }

        RequestResponse PreResponse(Protocol protocol, params INetworkMessage[] messages)
        {
            preResponse = protocol.Serialize(messages);
            return this;
        }

        RequestResponse PreResponse(SerializedMessages value)
        {
            preResponse = value;
            return this;
        }

        RequestResponse PostResponse(Protocol protocol, params INetworkMessage[] messages)
        {
            postResponse = protocol.Serialize(messages);
            return this;
        }

        RequestResponse PostResponse(SerializedMessages value)
        {
            postResponse = value;
            return this;
        }

        public byte[] ToBytes() => message?.ToArray();

        public override string ToString()
        {
            return $"{nameof(status)}: {status}\n{nameof(message)}: {message.UnreadLength()} bytes";
        }

        public static RequestResponse Ok(ISerializableValue value = null)
        {
            return new RequestResponse(value);
        }

        public static RequestResponse BadRequest(string body = null)
        {
            return new RequestResponse(StatusCode.BadRequest, new BadRequestException(body ?? "Bad Request"));
        }

        public static RequestResponse Unauthorized(string body = null)
        {
            return new RequestResponse(StatusCode.Unauthorized, new UnauthorizedException(body ?? "Unauthorized"));
        }

        public static RequestResponse Forbidden(string body = null)
        {
            return new RequestResponse(StatusCode.Forbidden, new ForbiddenException(body ?? "Forbidden"));
        }

        public static RequestResponse NotFound(string body = null)
        {
            return new RequestResponse(StatusCode.NotFound, new NotFoundException(body ?? "Not Found"));
        }

        public static RequestResponse RequestTimeout(string body = null)
        {
            return new RequestResponse(StatusCode.RequestTimeout, new TimeoutException(body ?? "Request Timeout"));
        }

        public static RequestResponse Gone(string body = null)
        {
            return new RequestResponse(StatusCode.Gone, new GoneException(body ?? "Gone"));
        }

        public static RequestResponse UnprocessableEntity(string body = null)
        {
            return new RequestResponse(StatusCode.UnprocessableEntity,
                new UnprocessableEntityException(body ?? "Unprocessable Entity"));
        }

        public static RequestResponse InternalServerError(string body = null)
        {
            return new RequestResponse(StatusCode.InternalServerError,
                new InternalServerErrorException(body ?? "Internal Server Error"));
        }

        public static RequestResponse NotImplemented(string body = null)
        {
            return new RequestResponse(StatusCode.NotImplemented,
                new RequestNotImplementedException(body ?? "Not Implemented"));
        }

        public static RequestResponse ServiceUnavailable(string body = null)
        {
            return new RequestResponse(StatusCode.ServiceUnavailable,
                new ServiceUnavailableException(body ?? "Service Unavailable"));
        }

        public static RequestResponse TooManyRequests(string body = null)
        {
            return new RequestResponse(StatusCode.TooManyRequests,
                new TooManyRequestsException(body ?? "Too Many Requests"));
        }
    }
}