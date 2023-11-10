using System;
using Newtonsoft.Json.Linq;

namespace MultiplayerProtocol
{
    public class RequestResponse : IRequestResponse
    {
        public StatusCode status { get; }
        private readonly SerializedMessage message;
        private readonly Exception _error;

        public bool isError { get; }

        public RequestResponse(SerializedMessage message) : this(StatusCode.Ok, message)
        {
        }

        public RequestResponse(StatusCode status, SerializedMessage message)
        {
            this.status = status;
            this.message = message;
            _error = null;
            isError = false;
        }

        public RequestResponse(ISerializableValue value = null)
        {
            status = StatusCode.Ok;
            message = value?.Serialize();
            _error = null;
            isError = false;
        }

        public RequestResponse(StatusCode status, Exception error)
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

        public T value<T>() where T : ISerializableValue, new()
        {
            if (message == null) return new T();

            var result = new T();
            result.DeserializeFrom(message);
            return result;
        }

        public byte[] ToBytes() => message?.ToArray();

        public override string ToString()
        {
            return $"{nameof(status)}: {status}\n{nameof(message)}: {message.UnreadLength()} bytes";
        }

        public static RequestResponse Ok(JArray json)
        {
            return new RequestResponse();
        }

        public static RequestResponse Ok(JObject json)
        {
            return new RequestResponse();
        }

        public static RequestResponse Ok(string body = null)
        {
            return new RequestResponse();
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