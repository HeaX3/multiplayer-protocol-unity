namespace MultiplayerProtocol
{
    public enum StatusCode
    {
        None = 0,
        Ok = 200,
        BadRequest = 400,
        Unauthorized = 401,
        Forbidden = 403,
        NotFound = 404,
        RequestTimeout = 408,
        Rejected = 409,
        Gone = 410,
        UnprocessableEntity = 422,
        InternalServerError = 500,
        NotImplemented = 501,
        ServiceUnavailable = 503,
        Delayed = 999,
        TooManyRequests = 429,
    }
}