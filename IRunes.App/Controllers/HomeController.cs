using SIS.HTTP.Requests.Contracts;
using SIS.HTTP.Responses.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace IRunes.App.Controllers
{
    public class HomeController : BaseController
    {
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
