﻿using SIS.MvcFramework.DependencyContainer;
using SIS.MvcFramework.Routing.Contracts;

namespace SIS.MvcFramework
{
    public interface IMvcApplication
    {
        void Configure(IServerRoutingTable serverRoutingTable);

        void ConfigureServices(IServiceProvider serviceProvider);
    }
}
