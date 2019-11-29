using IRunes.App.Controllers;
using IRunes.Data;
using SIS.HTTP.Enums;
using SIS.WebServer;
using SIS.WebServer.Result;
using SIS.WebServer.Routing.Contracts;

namespace IRunes.App
{
    public class Startup : IMvcApplication
    {
        public void Configure(IServerRoutingTable serverRoutingTable)
        {
            using (RunesDbContext context = new RunesDbContext())
            {
                context.Database.EnsureCreated();
            }
        }

        public void ConfigureServices()
        {

        }
    }
}
