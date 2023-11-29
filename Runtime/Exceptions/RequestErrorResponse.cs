using System;
using System.Text;

namespace MultiplayerProtocol
{
    public abstract class RequestErrorResponse : Exception, IRequestResponse
    {
        public abstract StatusCode status { get; }
        public bool isError => true;
        public Exception error() => this;

        public T value<T>() where T : ISerializableValue, new()
        {
            throw new InvalidOperationException();
        }

        public byte[] ToBytes()
        {
            return Encoding.UTF8.GetBytes(Message ?? GetType().Name);
        }

        protected RequestErrorResponse(string message) : base(message)
        {
        }

        public static Exception Of(StatusCode status, string message)
        {
            return status switch
            {
                StatusCode.BadRequest => new BadRequestException(message),
                StatusCode.Forbidden => new ForbiddenException(message),
                StatusCode.Gone => new GoneException(message),
                StatusCode.InternalServerError => new InternalServerErrorException(message),
                StatusCode.NotFound => new NotFoundException(message),
                StatusCode.NotImplemented => new RequestNotImplementedException(message),
                StatusCode.ServiceUnavailable => new ServiceUnavailableException(message),
                StatusCode.RequestTimeout => new TimeoutException(message),
                StatusCode.Unauthorized => new UnauthorizedException(message),
                StatusCode.UnprocessableEntity => new UnprocessableEntityException(message),
                _ => new Exception(message)
            };
        }
    }
}