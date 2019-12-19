using Musaca.App.ViewModels.Products;
using Musaca.Services;
using SIS.MvcFramework;
using SIS.MvcFramework.Attributes.Http;
using SIS.MvcFramework.Result;
using SIS.MvcFramework.Attributes.Security;
using SIS.MvcFramework.Mapping;
using Musaca.Models;
using Musaca.App.ViewModels.Orders;

namespace Musaca.App.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductsService productsService;
        private readonly IOrdersService ordersService;

        public ProductsController(IProductsService productsService, IOrdersService ordersService)
        {
            this.productsService = productsService;
            this.ordersService = ordersService;
        }

        [Authorize]
        public IActionResult Create()
        {
            return this.View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult Create(ProductsCreateInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.Redirect("/Products/Create");
            }

            this.productsService.Create(model.To<Product>());

            return this.Redirect("/Products/All");
        }

        [Authorize]
        public IActionResult All()
        {
            return this.View(this.productsService.GetAll().To<ProductsAllViewModel>());
        }
    }
}
