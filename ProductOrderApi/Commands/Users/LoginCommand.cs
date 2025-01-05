using MediatR;
using ProductOrderApi.DTOs.Users;
using ProductOrderApi.Infrastructure.Interfaces;

namespace ProductOrderApi.Commands.Users
{
    internal class LoginCommand : IRequest<string>
    {
        public LoginRequest Request { get; private set; }

        public LoginCommand(LoginRequest request)
        {
            Request = request;
        }
    }

    internal class LoginCommandHandler : IRequestHandler<LoginCommand, string>
    {
        private readonly IUserService _service;

        public LoginCommandHandler(IUserService service)
        {
            _service = service;
        }

        public async Task<string> Handle(LoginCommand command, CancellationToken cancellationToken)
        {
            var user = await _service.GetUserAsync(command.Request.Email, command.Request.Password);

            var expiresAt = DateTime.Now.AddHours(2);
            var token = _service.CreateToken(user, expiresAt);

            return token;

        }
    }
}
