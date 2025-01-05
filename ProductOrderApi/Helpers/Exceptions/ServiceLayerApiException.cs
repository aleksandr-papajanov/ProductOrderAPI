using System.Net;

namespace ProductOrderApi.Helpers.Exceptions
{
    internal class ServiceLayerApiException : ApiException
    {
        public ServiceLayerApiException(string message, HttpStatusCode status) : base(message, status) { }
    }

    internal class EntityAlreadyExistsApiException : ServiceLayerApiException
    {
        public EntityAlreadyExistsApiException(string message) : base(message, HttpStatusCode.Conflict) { }
    }

    internal class EntityNotFoundApiException : ServiceLayerApiException
    {
        public EntityNotFoundApiException(string message) : base(message, HttpStatusCode.NotFound) { }
    }
}
