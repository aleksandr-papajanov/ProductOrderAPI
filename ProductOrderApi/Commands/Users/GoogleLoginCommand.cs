using MediatR;
using ProductOrderApi.Infrastructure.Interfaces;

namespace ProductOrderApi.Commands.Users
{
    internal class GoogleLoginCommand : IRequest<string>
    {
    }

    internal class GoogleLoginCommandHandler : IRequestHandler<GoogleLoginCommand, string>
    {
        private readonly IUserService _userService;
        private readonly IGoogleAuthService _googleService;

        public GoogleLoginCommandHandler(IUserService userService, IGoogleAuthService googleService)
        {
            _userService = userService;
            _googleService = googleService;
        }

        public async Task<string> Handle(GoogleLoginCommand command, CancellationToken cancellationToken)
        {
            var authUrl = await _googleService.CreateRedirectUrlAsync("Login");

            return authUrl;
        }
    }
}
