namespace SIS.SandBox
{
    public class User
    { 
        [StringLength(4, 20, "The username length should be between 4 and 20 characters.")]
        public string Username { get; set; }
    }
}
