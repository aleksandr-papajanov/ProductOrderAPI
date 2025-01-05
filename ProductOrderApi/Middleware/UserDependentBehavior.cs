using MediatR;
using ProductOrderApi.Infrastructure.Interfaces;

namespace ProductOrderApi.Middleware
{
    internal class UserDependentBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IUserService _userService;

        public UserDependentBehavior(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (request is IUserDependent command)
            {
                command.CurrentUser = await _userService.GetUserFromClaimAsync();
            }

            return await next();
        }
    }
}
