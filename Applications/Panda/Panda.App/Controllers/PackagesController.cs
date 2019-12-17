using Panda.App.ViewModels.Packages;
using Panda.Models;
using Panda.Services;
using SIS.MvcFramework;
using SIS.MvcFramework.Attributes.Http;
using SIS.MvcFramework.Attributes.Security;
using SIS.MvcFramework.Result;
using System.Collections.Generic;
using System.Linq;

namespace Panda.App.Controllers
{
    public class PackagesController : Controller
    {
        private readonly IPackagesService packagesService;
        private readonly IUsersService usersService;

        public PackagesController(IPackagesService packageService, IUsersService usersService)
        {
            this.packagesService = packageService;
            this.usersService = usersService;
        }

        [Authorize]
        public IActionResult Create()
        {
            return this.View(this.usersService.GetUsernames());
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create(PackageCreateInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.Redirect("/Packages/Create");
            }

            this.packagesService.Create(model.Description, model.Weight, model.ShippingAddress, model.RecipientName);

            return this.Redirect("/Packages/Pending");
        }

        [Authorize]
        public IActionResult Delivered()
        {
            //Refactor: User Model Mapper
            List<PackageViewModel> packages = this.packagesService
                .GetAllByStatus(PackageStatus.Delivered)
                .Select(x => new PackageViewModel()
                {
                    Description = x.Description,
                    Id = x.Id,
                    Weight = x.Weight,
                    ShippingAddress = x.ShippingAddress,
                    RecipientName = x.Recipient.Username
                }).ToList();

            return this.View(new PackagesListViewModel() { Packages = packages });
        }

        [Authorize]
        public IActionResult Pending()
        {
            //Refactor: User Model Mapper
            List<PackageViewModel> packages = this.packagesService
                .GetAllByStatus(PackageStatus.Pending)
                .Select(x => new PackageViewModel()
                {
                    Description = x.Description,
                    Id = x.Id,
                    Weight = x.Weight,
                    ShippingAddress = x.ShippingAddress,
                    RecipientName = x.Recipient.Username
                }).ToList();

            return this.View(new PackagesListViewModel() { Packages = packages } );
        }

        [Authorize]
        public IActionResult Deliver(string id)
        {
            this.packagesService.Deliver(id);

            return this.Redirect("/Packages/Delivered");
        }
    }
}
