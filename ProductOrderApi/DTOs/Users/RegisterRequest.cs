using System.ComponentModel.DataAnnotations;

namespace ProductOrderApi.DTOs.Users
{
    /// <summary>
    /// Represents a request for user registration, including email and password for creating a new account.
    /// </summary>
    public class RegisterRequest
    {
        /// <summary>
        /// The email address of the user registering for an account.
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
        /// It must have at least 8 characters and contain at least one special character and one digit.
        /// </remarks>
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        [RegularExpression(@"^(?=.*[a-zA-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]+$", ErrorMessage = "Password must contain at least one letter, one number, and one special character.")]
        public required string Password { get; set; }
    }
}
