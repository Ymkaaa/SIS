using Panda.Data;
using SIS.MvcFramework;
using SIS.MvcFramework.Routing.Contracts;

namespace Panda.App
{
    public class StartUp : IMvcApplication
    {
        public void Configure(IServerRoutingTable serverRoutingTable)
        {
            using (PandaDbContext db = new PandaDbContext())
            {
                db.Database.EnsureCreated();
            }
        }

        public void ConfigureServices(SIS.MvcFramework.DependencyContainer.IServiceProvider serviceProvider)
        {
            // serviceProvider.Add()
        }
    }
}
