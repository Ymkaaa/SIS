using Panda.Data;
using Panda.Data.Models;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Panda.Services
{
    public class UsersService : IUsersService
    {
        private readonly PandaDbContext context;

        public UsersService(PandaDbContext context)
        {
            this.context = context;
        }

        public string CreateUser(string username, string email, string password)
        {
            User user = new User()
            {
                Username = username,
                Email = email,
                Password = HashPassword(password)
            };

            this.context.Add(user);
            this.context.SaveChanges();

            return user.Id;
        }

        public User GetUserOrNull(string username, string password)
        {
            return this.context.Users.FirstOrDefault(u => u.Username == username && u.Password == this.HashPassword(password));
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
