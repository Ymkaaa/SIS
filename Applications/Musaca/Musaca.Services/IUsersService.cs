using Musaca.Models;

namespace Musaca.Services
{
    public interface IUsersService
    {
        User GetUserOrNull(string username, string password);

        string CreateUser(string username, string email, string password);
    }
}
