using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProductOrderApi.DTOs;
using ProductOrderApi.Entities;
using ProductOrderApi.Helpers;
using ProductOrderApi.Helpers.Exceptions;
using ProductOrderApi.Infrastructure.Interfaces;

namespace ProductOrderApi.Commands.Users
{
    internal class GoogleCallbackCommand : IRequest<string>
    {
        public string Code { get; private set; }
        public Guid State { get; private set; }

        public GoogleCallbackCommand(string code, string state)
        {
            Code = code;
            State = Guid.Parse(state);
        }
    }

    internal class GoogleCallbackCommandHandler : IRequestHandler<GoogleCallbackCommand, string>
    {
        private readonly IUserService _userService;
        private readonly IGoogleAuthService _googleService;

        public GoogleCallbackCommandHandler(IUserService userService, IGoogleAuthService googleService)
        {
            _userService = userService;
            _googleService = googleService;
        }

        public async Task<string> Handle(GoogleCallbackCommand command, CancellationToken cancellationToken)
        {
            var token = await _googleService.GetTokenAsync(command.Code, command.State);
            await _googleService.VerifyTokenAsync(token);
            var userInfo = await _googleService.GetUserInfoAsync(token);

            if (!userInfo.EmailVerified)
            {
                throw new GoogleAuthentificationFailedApiException("Email not verified");
            }

            if (token.RequestType == "Login")
            {
                var user = await _userService.GetGoogleUserAsync(userInfo.Email);

                return _userService.CreateToken(user, token.ExpiresAt);
            }

            if (token.RequestType == "Register")
            {
                var user = await _userService.AddGoogleUserAsync(userInfo);

                return _userService.CreateToken(user, token.ExpiresAt);
            }

            throw new GoogleAuthentificationFailedApiException("Invalid request type");
        }
    }
}
