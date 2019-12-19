using SIS.MvcFramework.Attributes.Validation;

namespace Musaca.App.ViewModels.Users
{
    public class UsersLoginInputModel
    {
        private const string UsernameLengthErrorMessage = "Username should be between 5 and 20 characters";

        [Required]
        [StringLength(5, 20, UsernameLengthErrorMessage)]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
