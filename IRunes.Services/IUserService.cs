using IRunes.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace IRunes.Services
{
    public interface IUserService
    {
        User CreateUser(User user);

        User GetUserByUsernameAndPassword(string username, string password);

    }
}
