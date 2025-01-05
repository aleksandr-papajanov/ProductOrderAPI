using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Net;

namespace ProductOrderApi.Helpers.Exceptions
{
    internal abstract class ApiException : Exception
    {
        public HttpStatusCode StatusCode { get; protected set; }

        public ApiException(string message, HttpStatusCode statusCode) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
