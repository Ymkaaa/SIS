using SIS.MvcFramework.Attributes.Validation;

namespace Musaca.App.ViewModels.Users
{
    public class UsersRegisterInputModel
    {
        private const string UsernameLengthErrorMessage = "Username should be between 5 and 20 characters";
        private const string EmailLengthErrorMessage = "Email should be between 5 and 20 characters";

        [Required]
        [StringLength(5, 20, UsernameLengthErrorMessage)]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string ConfirmPassword { get; set; }

        [Required]
        [StringLength(5, 20, EmailLengthErrorMessage)]
        public string Email { get; set; }
    }
}
