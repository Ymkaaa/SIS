using Musaca.App.ViewModels.Users;
using Musaca.Models;
using Musaca.Services;
using SIS.MvcFramework;
using SIS.MvcFramework.Attributes.Http;
using SIS.MvcFramework.Attributes.Security;
using SIS.MvcFramework.Result;
using System.Collections.Generic;
using System.Linq;

namespace Musaca.App.Controllers
{
    public class UsersController : Controller
    {
        private readonly UsersService usersService;
        private readonly OrdersService ordersService;

        public UsersController(UsersService usersService, OrdersService ordersService)
        {
            this.usersService = usersService;
            this.ordersService = ordersService;
        }

        public IActionResult Login()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Login(UsersLoginInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.Redirect("/Users/Login");
            }

            User user = this.usersService.GetUserOrNull(model.Username, model.Password);

            if (user == null)
            {
                return this.Redirect("/Users/Login");
            }

            this.SignIn(user.Id, user.Username, user.Email);

            return this.Redirect("/");
        }

        public IActionResult Register()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Register(UsersRegisterInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.Redirect("/users/register");
            }

            string userId = this.usersService.CreateUser(model.Username, model.Email, model.Password);

            this.SignIn(userId, model.Username, model.Email);

            return this.Redirect("/");
        }

        [Authorize]
        public IActionResult Profile()
        {
            List<UsersProfileViewModel> orders = this.ordersService
                .GetAllCompletedOrders(this.User.Id)
                .Select(x => new UsersProfileViewModel
                {
                    Id = x.Id,
                    Total = this.ordersService.GetOrderProducts(x.Id).Select(x => x.Price).Sum(),
                    IssuedOn = x.IssuedOn.ToString("dd/MM/yy")
                }).ToList();

            return this.View(orders);
        }

        [Authorize]
        public IActionResult Logout()
        {
            this.SignOut();

            return this.Redirect("/");
        }
    }
}
