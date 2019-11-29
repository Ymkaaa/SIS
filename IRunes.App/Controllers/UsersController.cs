using IRunes.Data;
using SIS.HTTP.Requests.Contracts;
using SIS.HTTP.Responses.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using IRunes.Models;
using System.Security.Cryptography;
using SIS.WebServer;
using SIS.WebServer.Attributes;

namespace IRunes.App.Controllers
{
    public class UsersController : Controller
    {   
        private string HashPassword(string password)
        {
            using(SHA256 sha256Hash = SHA256.Create())
            {
                return Encoding.UTF8.GetString(sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password)));
            }
        }
        public IHttpResponse Login(IHttpRequest request)
        {
            return this.View();
        }

        [HttpPost(ActionName = "Login")]
        public IHttpResponse LoginConfirm(IHttpRequest request)
        {
            using(RunesDbContext context = new RunesDbContext())
            {
                string username = ((ISet<string>)request.FormData["username"]).FirstOrDefault();
                string password = ((ISet<string>)request.FormData["password"]).FirstOrDefault();

                User userFromDb = context.Users.FirstOrDefault(user => (user.Username == username || user.Email == username) && user.Password == this.HashPassword(password));

                if (userFromDb == null)
                {
                    return this.Redirect("/Users/Login");
                }

                this.SignIn(request, userFromDb.Id, userFromDb.Username, userFromDb.Email);
            }

            return this.Redirect("/");
        }

        public IHttpResponse Register(IHttpRequest request)
        {
            return this.View();
        }

        [HttpPost(ActionName = "Register")]
        public IHttpResponse RegisterConfirm(IHttpRequest request)
        {
            using (RunesDbContext context = new RunesDbContext())
            {
                string username = ((ISet<string>)request.FormData["username"]).FirstOrDefault();
                string password = ((ISet<string>)request.FormData["password"]).FirstOrDefault();
                string confirmPassword = ((ISet<string>)request.FormData["confirmPassword"]).FirstOrDefault();
                string email = ((ISet<string>)request.FormData["email"]).FirstOrDefault();

                if (password != confirmPassword)
                {
                    return this.Redirect("/Users/Register");
                }

                User user = new User()
                {
                    Username = username,
                    Password = this.HashPassword(password), 
                    Email = email
                };

                context.Users.Add(user);
                context.SaveChanges();  
            }

            return this.Redirect("/Users/Login");
        }

        public IHttpResponse Logout(IHttpRequest request)
        {
            this.SignOut(request);

            return this.Redirect("/");
        }
    }
}
