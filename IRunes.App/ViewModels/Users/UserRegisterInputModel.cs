using SIS.MvcFramework.Attributes.Validation;

namespace IRunes.App.ViewModels.Users
{
    public class UserRegisterInputModel
    {
        private const string InvalidEmailMessage = "Invalid email.";
        private const string InvalidPasswordMessage = "Invalid password.";
        private const string InvalidUsernameLengthMessage = "The username length should be between 4 and 20 characters.";

        [Required]
        [StringLength(4, 20, InvalidUsernameLengthMessage)]
        public string Username { get; set; }

        [Required]
        [Password(InvalidPasswordMessage)]
        public string Password { get; set; }

        [Required()]
        [Password(InvalidPasswordMessage)]
        public string ConfirmPassword { get; set; }

        [Required]
        [Email(InvalidEmailMessage)]
        public string Email { get; set; }
    }
}
