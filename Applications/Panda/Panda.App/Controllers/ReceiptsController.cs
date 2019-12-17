using Panda.App.ViewModels.Receipts;
using Panda.Services;
using SIS.MvcFramework;
using SIS.MvcFramework.Attributes.Security;
using SIS.MvcFramework.Result;
using System.Linq;

namespace Panda.App.Controllers
{
    public class ReceiptsController : Controller
    {
        private readonly IReceiptsService receiptsService;

        public ReceiptsController(IReceiptsService receiptsService)
        {
            this.receiptsService = receiptsService;
        }

        [Authorize]
        public IActionResult Index()
        {
            IQueryable<ReceiptViewModel> model = this.receiptsService.GetAll().Select(x => new ReceiptViewModel()
            {
                Id = x.Id,
                Fee = x.Fee,
                IssuedOn = x.IssuedOn,
                RecipientName = x.Recipient.Username
            });

            return this.View(model);
        }
    }
}
