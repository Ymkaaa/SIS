using SIS.MvcFramework.Attributes.Validation;

namespace Musaca.App.ViewModels.Orders
{
    public class OrdersAddProductInputModel
    {
        private const string ProductNameErrorMessage = "Name length should be between 3 and 10 characters";

        [Required]
        [StringLength(3, 10, ProductNameErrorMessage)]
        public string Name { get; set; }
    }
}
