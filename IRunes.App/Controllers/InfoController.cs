using SIS.MvcFramework;
using SIS.MvcFramework.Result;

namespace IRunes.App.Controllers
{
    public class InfoController : Controller
    {
        public ActionResult About()
        {
            return this.View();
        }
    }
}
