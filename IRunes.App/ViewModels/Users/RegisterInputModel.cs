using SIS.MvcFramework.Attributes.Validation;

namespace IRunes.App.ViewModels.Users
{
    public class RegisterInputModel
    {
        [StringLength(4, 20, "The length should be between 4 and 20 characters.")]
        public string Username { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        [Email("Invalid email")]
        public string Email { get; set; }
    }
}
