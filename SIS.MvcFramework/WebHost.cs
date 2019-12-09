using SIS.MvcFramework.Routing.Contracts;
using SIS.MvcFramework.Routing;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using SIS.HTTP.Common;
using SIS.HTTP.Enums;
using SIS.MvcFramework.Result;
using SIS.MvcFramework.Identity;
using SIS.MvcFramework.Attributes.Security;
using SIS.HTTP.Responses;
using SIS.MvcFramework.Attributes.Http;
using SIS.MvcFramework.Attributes.Action;
using SIS.MvcFramework.Sessions;
using SIS.MvcFramework.DependencyContainer;
using SIS.MvcFramework.Logging;
using SIS.HTTP.Responses.Contracts;
using SIS.HTTP.Requests.Contracts;

namespace SIS.MvcFramework
{
    public static class WebHost
    {
        public static void Start(IMvcApplication application)
        {
            IServerRoutingTable serverRoutingTable = new ServerRoutingTable();
            IHttpSessionStorage sessionStorage = new HttpSessionStorage();
            IServiceProvider serviceProvider = new ServiceProvider();

            serviceProvider.Add<ILogger, ConsoleLogger>();

            AutoRegisterRoutes(application, serverRoutingTable, serviceProvider);

            application.ConfigureServices(serviceProvider);
            application.Configure(serverRoutingTable);

            Server server = new Server(8000, serverRoutingTable, sessionStorage);
            server.Run();
        }

        private static void AutoRegisterRoutes(IMvcApplication application, IServerRoutingTable serverRoutingTable, IServiceProvider serviceProvider)
        {
            IEnumerable<System.Type> controllers = application
                .GetType()
                .Assembly
                .GetTypes()
                .Where(type => type.IsClass && !type.IsAbstract && type.IsSubclassOf(typeof(Controller)));

            foreach (System.Type controller in controllers)
            {
                IEnumerable<MethodInfo> actions = controller
                    .GetMethods(BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Instance)
                    .Where(method => !method.IsSpecialName && method.DeclaringType == controller)
                    .Where(method => method.GetCustomAttributes().All(a => a.GetType() != typeof(NonActionAttribute)));

                foreach (MethodInfo action in actions)
                {
                    CustomAttributeData attr = action
                        .CustomAttributes
                        .Where(attribute => attribute.AttributeType.IsSubclassOf(typeof(BaseHttpAttribute)))
                        .LastOrDefault();

                    string url = $"/{controller.Name.Replace("Controller", string.Empty)}/{action.Name}";
                    HttpRequestMethod method = HttpRequestMethod.Get;

                    if (attr != null)
                    {
                        List<CustomAttributeNamedArgument> actionNameArgs = attr.NamedArguments.Where(a => a.MemberName == "ActionName").ToList();
                        List<CustomAttributeNamedArgument> urlArgs = attr.NamedArguments.Where(a => a.MemberName == "Url").ToList();

                        if (urlArgs.Count != 0)
                        {
                            url = urlArgs[0].TypedValue.Value.ToString();
                        }

                        if (actionNameArgs.Count != 0)
                        {
                            url = $"/{controller.Name.Replace("Controller", string.Empty)}/{actionNameArgs[0].TypedValue.Value.ToString()}";
                        }

                        method = (HttpRequestMethod)System.Enum.Parse(typeof(HttpRequestMethod), Regex.Match(attr.AttributeType.Name, GlobalConstants.AttributeMethodMatchPattern).Groups["method"].Value);
                    }

                    serverRoutingTable.Add(method, url, (request) => ProcessRequest(serviceProvider, controller, action, request));

                    System.Console.WriteLine($"{method} - {url}");
                }
            }
        }

        private static IHttpResponse ProcessRequest(IServiceProvider serviceProvider, System.Type controller, MethodInfo action, IHttpRequest request)
        {
            Controller controllerInstance = serviceProvider.CreateInstance(controller) as Controller;
            controllerInstance.Request = request;

            //Security Authorization
            Principal controllerPrincipal = controllerInstance.User;
            AuthorizeAttribute authorizeAttibute = action.GetCustomAttributes().LastOrDefault(a => a.GetType() == typeof(AuthorizeAttribute)) as AuthorizeAttribute;

            if (authorizeAttibute != null && !authorizeAttibute.IsInAuthority(controllerPrincipal))
            {
                return new HttpResponse(HttpResponseStatusCode.Forbidden);
            }

            ParameterInfo[] parameters = action.GetParameters();
            List<object> parameterValues = new List<object>();

            foreach (ParameterInfo parameter in parameters)
            {
                string parameterName = parameter.Name.ToLower();
                ISet<string> httpDataValue =  null;

                if (request.QueryData.Any(x => x.Key.ToLower() == parameterName))
                {
                    httpDataValue = request.QueryData.FirstOrDefault(x => x.Key.ToLower() == parameterName).Value;
                }
                else if (request.FormData.Any(x => x.Key.ToLower() == parameterName))
                {
                    httpDataValue = request.FormData.FirstOrDefault(x => x.Key.ToLower() == parameterName).Value;
                }

                string httpStringValue = httpDataValue.FirstOrDefault();

                object parameterValue = System.Convert.ChangeType(httpStringValue, parameter.ParameterType);

                parameterValues.Add(parameterValue);
            }

            ActionResult response = action.Invoke(controllerInstance, parameterValues.ToArray()) as ActionResult;

            return response;
        }
    }
}
