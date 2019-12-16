using SIS.MvcFramework.Attributes.Validation;

namespace Panda.App.ViewModels.Users
{
    public class UserRegisterInputModel
    {
        [Required]
        [StringLength(5, 20, "Username should be between 5 and 20 characters")]
        public string Username { get; set; }

        [Required]
        [StringLength(5, 20, "Username should be between 5 and 20 characters")]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string ConfirmPassword { get; set; }
    }
}
