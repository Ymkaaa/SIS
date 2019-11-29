using SIS.HTTP.Requests.Contracts;
using SIS.HTTP.Responses.Contracts;
using SIS.WebServer;

namespace IRunes.App.Controllers
{
    public class InfoController : Controller
    {
        public IHttpResponse About(IHttpRequest request)
        {
            return this.View();
        }
    }
}
