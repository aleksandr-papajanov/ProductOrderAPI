using MediatR;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using ProductOrderApi.Commands.Users;
using ProductOrderApi.DTOs.Users;
using System.Security.Claims;
using System.Web;
using MySqlX.XDevAPI;
using Newtonsoft.Json;
using ProductOrderApi.DTOs;
using ProductOrderApi.Infrastructure.Interfaces;

namespace ProductOrderApi.Controllers
{
    /// <summary>
    /// Provides endpoints for user authentication and registration.
    /// </summary>
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// API controller for managing authorization.
        /// </summary>
        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Registers a new user in the system.
        /// </summary>
        /// <param name="request">The user registration details.</param>
        /// <returns>A success message upon registration.</returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var command = new RegisterCommand(request);

            var user = await _mediator.Send(command);

            return Ok(new { message = "User registered successfully." });
        }

        /// <summary>
        /// Authenticates a user and generates a JWT token.
        /// </summary>
        /// <param name="request">The user login credentials.</param>
        /// <returns>A bearer token for authenticated requests.</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var command = new LoginCommand(request);

            var token = await _mediator.Send(command);

            return Ok($"Bearer {token}");
        }

        /// <summary>
        /// Initiates the Google registration process by generating a URL for the user to follow.
        /// </summary>
        /// <returns>
        /// Redirect URL for Google registration.
        /// </returns>
        [HttpPost("register-via-google")]
        public async Task<IActionResult> GoogleRegister()
        {
            var command = new GoogleRegisterCommand();

            var url = await _mediator.Send(command);

            return Ok(new { Message = "Please follow this link", RedirectURL = url });
            //return Redirect(url);
        }

        /// <summary>
        /// Initiates the Google login process by generating a URL for the user to follow.
        /// </summary>
        /// <returns>
        /// Redirect URL for Google login.
        /// </returns>
        [HttpGet("login-via-google")]
        public async Task<IActionResult> GoogleLogin()
        {
            var command = new GoogleLoginCommand();

            var url = await _mediator.Send(command);

            return Ok(new { Message = "Please follow this link", RedirectURL = url });
            //return Redirect(url);
        }

        /// <summary></summary>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet("confirm-email/{token:guid}")]
        public async Task<IActionResult> ConfirmEmail([FromRoute] Guid token)
        {
            var command = new ConfirmEmailCommand(token);

            await _mediator.Send(command);

            return Ok();
        }

        /// <summary></summary>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet("callback")]
        public async Task<IActionResult> GoogleCallback([FromQuery] string code, [FromQuery] string state)
        {
            var command = new GoogleCallbackCommand(code, state);

            var token = await _mediator.Send(command);

            return Ok($"Bearer {token}");
        }
    }
}