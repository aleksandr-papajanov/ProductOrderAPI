using MediatR;
using ProductOrderApi.DTOs.Mappers;
using ProductOrderApi.DTOs.Users;
using ProductOrderApi.Entities;
using ProductOrderApi.Infrastructure.Interfaces;
using System.Data;

namespace ProductOrderApi.Commands.Users
{
    internal class RegisterCommand : IRequest<User>, ITransactionDependent
    {
        public RegisterRequest Request { get; private set; }

        public IsolationLevel IsolationLevel => IsolationLevel.Serializable;

        public RegisterCommand(RegisterRequest request)
        {
            Request = request;
        }
    }

    internal class RegisterCommandHandler : IRequestHandler<RegisterCommand, User>
    {
        private readonly IUserService _service;
        private readonly IMailService _mailService;

        public RegisterCommandHandler(IUserService service, IMailService mailService)
        {
            _service = service;
            _mailService = mailService;
        }

        public async Task<User> Handle(RegisterCommand command, CancellationToken cancellationToken)
        {
            var user = command.Request.ToEntity();

            await _service.AddUserAsync(user);
            await SendConfirmationRequestMail(user);

            return user;
        }

        private async Task SendConfirmationRequestMail(User user)
        {
            var confirmationLink = $"http://localhost:8080/api/auth/confirm-email/{user.ConfirmationToken.Token}";

            await _mailService.SendEmailAsync(
                user.Email,
                "Email confirmation request",
                "<h1>Welcome to OrderProductAPI</h1>" +
                "<hr />" +
                "We are happy that you have decided to join us. " +
                "To finish registration please confirm your email by " +
                $"<a href=\"{confirmationLink}\">following this link</a>");
        }
    }
}
