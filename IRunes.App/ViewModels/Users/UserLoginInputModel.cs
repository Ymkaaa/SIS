using SIS.MvcFramework.Attributes.Validation;

namespace IRunes.App.ViewModels.Users
{
    public class UserLoginInputModel
    {
        private const string ErrorMessage = "Invalid username or password.";

        [Required(ErrorMessage)]
        public string Username { get; set; }
        
        [Required(ErrorMessage)]
        public string Password { get; set; }
    }
}
