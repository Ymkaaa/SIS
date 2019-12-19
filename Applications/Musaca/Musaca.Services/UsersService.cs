using Musaca.Data;
using Musaca.Models;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Musaca.Services
{
    public class UsersService : IUsersService
    {
        private readonly MusacaDbContext context;

        public UsersService(MusacaDbContext context)
        {
            this.context = context;
        }

        public User GetUserOrNull(string username, string password)
        {
            return this.context.Users.FirstOrDefault(u => u.Username == username && u.Password == this.HashPassword(password));
        }

        public string CreateUser(string username, string email, string password)
        {
            User user = new User()
            {
                Username = username,
                Email = email,
                Password = this.HashPassword(password)
            };

            this.context.Add(user);
            this.context.SaveChanges();

            return user.Id;
        }

        private string HashPassword(string password)
        {
            using (SHA512 sha256Hash = SHA512.Create())
            {
                return Encoding.UTF8.GetString(sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password)));
            }
        }
    }
}
