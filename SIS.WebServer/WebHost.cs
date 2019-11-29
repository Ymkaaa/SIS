using SIS.WebServer.Routing.Contracts;
using SIS.WebServer.Routing;
using SIS.WebServer.Attributes;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System;
using SIS.HTTP.Common;
using SIS.HTTP.Enums;
using SIS.HTTP.Responses.Contracts;

namespace SIS.WebServer
{
    public static class WebHost
    {
        public static void Start(IMvcApplication application)
        {
            IServerRoutingTable serverRoutingTable = new ServerRoutingTable();
            AutoRegisterRoutes(application, serverRoutingTable);

            application.ConfigureServices();
            application.Configure(serverRoutingTable);

            Server server = new Server(8000, serverRoutingTable);
            server.Run();
        }

        private static void AutoRegisterRoutes(IMvcApplication application, IServerRoutingTable serverRoutingTable)
        {
            IEnumerable<Type> controllers = application
                .GetType()
                .Assembly
                .GetTypes()
                .Where(type => type.IsClass && !type.IsAbstract && type.IsSubclassOf(typeof(Controller)));

            foreach (Type controller in controllers)
            {
                IEnumerable<MethodInfo> actions = controller
                    .GetMethods(BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Instance)
                    .Where(method => !method.IsSpecialName);

                foreach (MethodInfo action in actions)
                {
                    CustomAttributeData attr = action
                        .CustomAttributes
                        .Where(attribute => attribute.AttributeType.IsSubclassOf(typeof(BaseHttpAttribute)))
                        .LastOrDefault();

                    if (attr == null)
                    {
                        string url = $"/{controller.Name.Replace("Controller", string.Empty)}/{action.Name}";

                        serverRoutingTable.Add(HttpRequestMethod.Get, url, request =>
                        {
                            Object controllerInstance = Activator.CreateInstance(controller);
                            IHttpResponse response = action.Invoke(controllerInstance, new[] { request }) as IHttpResponse;

                            return response;
                        });

                        Console.WriteLine($"{HttpRequestMethod.Get} - {url}");
                    }
                    else
                    {
                        CustomAttributeNamedArgument actionNameAttrs = attr.NamedArguments.Where(a => a.MemberName == "ActionName").FirstOrDefault();
                        CustomAttributeNamedArgument urlAttrs = attr.NamedArguments.Where(a => a.MemberName == "Url").FirstOrDefault();

                        HttpRequestMethod method = (HttpRequestMethod)Enum.Parse(typeof(HttpRequestMethod), Regex.Match(attr.AttributeType.Name, GlobalConstants.AttributeMethodMatchPattern).Groups["method"].Value);
                        string url;

                        if (attr.NamedArguments.Where(a => a.MemberName == "Url").ToList().Count == 0)
                        {
                            url = $"/{controller.Name.Replace("Controller", string.Empty)}/{actionNameAttrs.TypedValue.Value.ToString()}";
                        }
                        else
                        {
                            url = urlAttrs.TypedValue.Value.ToString();
                        }
                        
                        serverRoutingTable.Add(method, url, request =>
                        {
                            Object controllerInstance = Activator.CreateInstance(controller);
                            IHttpResponse response = action.Invoke(controllerInstance, new[] { request }) as IHttpResponse;

                            return response;
                        });

                        Console.WriteLine($"{method} - {url}");
                    }


                }

            }
        }
    }
}
