using SIS.MvcFramework;
using SIS.MvcFramework.Attributes.Http;
using SIS.MvcFramework.Result;

namespace IRunes.App.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet(Url = "/")]
        public ActionResult IndexSlash()
        {
            return Index();
        }

        public ActionResult Index()
        {
            return this.View();
        }
    }
}
