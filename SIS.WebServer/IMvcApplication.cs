using SIS.WebServer.Routing.Contracts;

namespace SIS.WebServer
{
    public interface IMvcApplication
    {
        void Configure(IServerRoutingTable serverRoutingTable);

        void ConfigureServices();
    }
}
