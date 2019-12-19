using Musaca.App.ViewModels.Home;
using Musaca.Models;
using Musaca.Services;
using SIS.MvcFramework;
using SIS.MvcFramework.Attributes.Http;
using SIS.MvcFramework.Result;
using System.Collections.Generic;

namespace Musaca.App.Controllers
{
    public class HomeController : Controller
    {
        private readonly IOrdersService ordersService;

        public HomeController(IOrdersService ordersService)
        {
            this.ordersService = ordersService;
        }

        [HttpGet(Url = "/")]
        public IActionResult Slash()
        {
            return this.Index();
        }

        public IActionResult Index()
        {
            if (this.IsLoggedIn())
            {
                this.ordersService.CreateOrder(this.User.Id);

                Order order = this.ordersService.GetCurrentActiveOrder(this.User.Id);
                List<Product> orderProducts = this.ordersService.GetOrderProducts(order.Id);

                List<HomeProductViewModel> products = new List<HomeProductViewModel>();

                foreach (Product product in orderProducts)
                {
                    products.Add(new HomeProductViewModel()
                    {
                        Name = product.Name,
                        Price = product.Price
                    });
                }


                return this.View(products, "IndexLoggedIn");
            }

            return this.View();
        }
    }
}