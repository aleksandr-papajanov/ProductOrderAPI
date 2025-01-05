using MediatR;
using ProductOrderApi.Infrastructure.Interfaces;

namespace ProductOrderApi.Commands.Users
{
    internal class GoogleRegisterCommand : IRequest<string>
    {
    }

    internal class GoogleRegisterCommandHandler : IRequestHandler<GoogleRegisterCommand, string>
    {
        private readonly IUserService _userService;
        private readonly IGoogleAuthService _googleService;

        public GoogleRegisterCommandHandler(IUserService userService, IGoogleAuthService googleService)
        {
            _userService = userService;
            _googleService = googleService;
        }

        public async Task<string> Handle(GoogleRegisterCommand command, CancellationToken cancellationToken)
        {
            var authUrl = await _googleService.CreateRedirectUrlAsync("Register");

            return authUrl;
        }
    }
}
