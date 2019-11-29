using SIS.HTTP.Enums;
using SIS.HTTP.Requests.Contracts;
using SIS.HTTP.Responses.Contracts;
using SIS.WebServer.Result;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;

namespace SIS.WebServer
{
    public abstract class Controller
    {
        protected Controller()
        {
            this.ViewData = new Dictionary<string, object>();
        }

        protected Dictionary<string, object> ViewData;

        private string ParseTemplate(string viewContent)
        {
            foreach (KeyValuePair<string, object> param in this.ViewData)
            {
                viewContent = viewContent.Replace($"@Model.{param.Key}", param.Value.ToString());
            }

            return viewContent;
        }
        
        protected bool IsLoggedIn(IHttpRequest request)
        {
            return request.Session.ContainsParameter("username");
        }

        protected void SignIn(IHttpRequest request, string id, string username, string email)
        {
            request.Session.AddParameter("id", id);
            request.Session.AddParameter("username", username);
            request.Session.AddParameter("email", email);
        }

        protected void SignOut(IHttpRequest request)
        {
            request.Session.ClearParameters();
        }

        protected IHttpResponse View([CallerMemberName] string view = null)
        {
            string controllerName = this.GetType().Name.Replace("Controller", string.Empty);
            string viewName = view;

            string viewContent = File.ReadAllText($"Views/{controllerName}/{viewName}.html");

            viewContent = this.ParseTemplate(viewContent);

            HtmlResult htmlResult = new HtmlResult(viewContent, HttpResponseStatusCode.Ok);

            return htmlResult;
        }

        protected IHttpResponse Redirect(string url)
        {
            return new RedirectResult(url);
        }
    }
}
