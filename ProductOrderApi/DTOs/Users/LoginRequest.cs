using System.ComponentModel.DataAnnotations;

namespace ProductOrderApi.DTOs.Users
{
    /// <summary>
    /// Represents a request for login
    /// </summary>
    public class LoginRequest
    {
        /// <summary>
        /// The email address
        /// </summary>
        /// <remarks>
        /// This field must contain a valid email address. 
        /// It is used as the primary identifier for the user account.
        /// </remarks>
        [EmailAddress]
        public required string Email { get; set; }

        /// <summary>
        /// The password for the user account.
        /// </summary>
        /// <remarks>
        /// This field is used to set the password for the new account.
        /// It must contain a non-empty string.
        /// </remarks>
        public required string Password { get; set; }
    }
}
