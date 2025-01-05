using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Net;

namespace ProductOrderApi.Helpers.Exceptions
{
    internal class ValidationApiException : ApiException
    {
        public Dictionary<string, List<string>> Errors { get; private set; }

        public ValidationApiException(ModelStateDictionary modelState)
            : base("Error occured while validating request",
                  HttpStatusCode.BadRequest)
        {
            Errors = GetErrors(modelState);
        }

        private Dictionary<string, List<string>> GetErrors(ModelStateDictionary modelState)
        {
            var collection = new Dictionary<string, List<string>>();

            foreach (var state in modelState)
            {
                if (!state.Value.Errors.Any())
                {
                    continue;
                }

                collection.Add(
                    state.Key,
                    state.Value.Errors
                        .Select(x => x.ErrorMessage)
                        .ToList());

            }

            return collection;
        }
    }
}
