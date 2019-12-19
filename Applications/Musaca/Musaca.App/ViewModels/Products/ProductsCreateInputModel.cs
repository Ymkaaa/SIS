using SIS.MvcFramework.Attributes.Validation;
using System;

namespace Musaca.App.ViewModels.Products
{
    public class ProductsCreateInputModel
    {
        private const string NameLengthErrorMessage = "Name should be between 3 and 10 characters";
        private const string PriceRangeErrorMessage = "Product price must be greater than or equal to 0.01";

        [Required]
        [StringLength(3, 10, NameLengthErrorMessage)]
        public string Name { get; set; }

        [Required]
        [Range(typeof(decimal), "0.01", "1000000", PriceRangeErrorMessage)]
        public decimal Price { get; set; }
    }
}
