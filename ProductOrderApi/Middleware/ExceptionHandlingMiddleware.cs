using Microsoft.AspNetCore.Http.HttpResults;
using ProductOrderApi.Helpers.Exceptions;
using System;

namespace ProductOrderApi.Middleware
{
    internal class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            httpContext.Response.ContentType = "application/json";

            var response = new object();

            try
            {
                await _next(httpContext);
            }
            catch (ValidationApiException ex)
            {
                httpContext.Response.StatusCode = (int)ex.StatusCode;

                await httpContext.Response.WriteAsJsonAsync(new { message = ex.Message, Type = ex.GetType().Name, errors = ex.Errors });
            }
            catch (ApiException ex)
            {
                httpContext.Response.StatusCode = (int)ex.StatusCode;

                await httpContext.Response.WriteAsJsonAsync(new { message = ex.Message, Type = ex.GetType().Name });
            }
        }
    }
}
