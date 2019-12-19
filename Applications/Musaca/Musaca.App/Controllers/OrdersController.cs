using Musaca.App.ViewModels.Orders;
using Musaca.Models;
using Musaca.Services;
using SIS.MvcFramework;
using SIS.MvcFramework.Attributes.Http;
using SIS.MvcFramework.Attributes.Security;
using SIS.MvcFramework.Result;

namespace Musaca.App.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IProductsService productsService;
        private readonly IOrdersService ordersService;

        public OrdersController(IProductsService productsService, IOrdersService ordersService)
        {
            this.productsService = productsService;
            this.ordersService = ordersService;
        }

        [Authorize]
        [HttpPost]
        public IActionResult AddProduct(OrdersAddProductInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.Redirect("/");
            }

            Product product = this.productsService.GetByName(model.Name);

            if (product == null)
            {
                return this.Redirect("/");
            }

            this.ordersService.AddProduct(product, this.User.Id);

            return this.Redirect("/");
        }

        [Authorize]
        public IActionResult Cashout()
        {
            this.ordersService.CompleteOrder(this.ordersService.GetCurrentActiveOrder(this.User.Id).Id, this.User.Id);

            return this.Redirect("/Users/Profile");
        }
    }
}
