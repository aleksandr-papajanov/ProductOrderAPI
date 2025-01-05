using System.Net;

namespace ProductOrderApi.Helpers.Exceptions
{
    internal class UnauthorizedApiException : ApiException
    {
        public UnauthorizedApiException(string message = "Unauthorized access.", HttpStatusCode status = HttpStatusCode.Unauthorized)
            : base(message, status) { }
    }

    internal class AccessDeniedApiException : UnauthorizedApiException
    {
        public AccessDeniedApiException() : base()
        {
            StatusCode = HttpStatusCode.Forbidden;
        }
    }

    internal class GoogleAuthentificationFailedApiException : UnauthorizedApiException
    {
        public GoogleAuthentificationFailedApiException(string message) : base(message)
        {
        }
    }
}
