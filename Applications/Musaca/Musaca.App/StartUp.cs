using Musaca.Data;
using Musaca.Services;
using SIS.MvcFramework;
using SIS.MvcFramework.DependencyContainer;
using SIS.MvcFramework.Routing.Contracts;

namespace Musaca.App
{
    public class StartUp : IMvcApplication
    {
        public void Configure(IServerRoutingTable serverRoutingTable)
        {
            using (MusacaDbContext context = new MusacaDbContext())
            {
                context.Database.EnsureCreated();
            }
        }

        public void ConfigureServices(IServiceProvider serviceProvider)
        {
            serviceProvider.Add<IUsersService, UsersService>();
            serviceProvider.Add<IProductsService, ProductsService>();
            serviceProvider.Add<IOrdersService, OrdersService>();
        }
    }
}
