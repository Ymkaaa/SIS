using SIS.HTTP.Requests.Contracts;
using SIS.HTTP.Responses.Contracts;
using SIS.WebServer;
using SIS.WebServer.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace IRunes.App.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet(Url = "/")]
        public IHttpResponse IndexSlash(IHttpRequest request)
        {
            return Index(request);
        }

        public IHttpResponse Index(IHttpRequest request)
        {
            if (this.IsLoggedIn(request))
            {
                this.ViewData["Username"] = request.Session.GetParameter("username");

                return this.View("Home");
            }

            return this.View();
        }
    }
}
