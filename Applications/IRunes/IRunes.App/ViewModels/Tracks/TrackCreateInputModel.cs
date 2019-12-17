using SIS.MvcFramework.Attributes.Validation;

namespace IRunes.App.ViewModels.Tracks
{
    public class TrackCreateInputModel
    {
        private const string NameLengthErrorMessage = "Length should be between 3 and 20 characters.";
        private const string LinkLengthErrorMessage = "Length should be between 4 and 1000 characters.";
        private const string PriceRangeErrorMessage = "Invalid Price. The price should be between 1 and 1000";

        [Required]
        public string AlbumId { get; set; }

        [Required]
        [StringLength(3, 20, NameLengthErrorMessage)]
        public string Name { get; set; }

        [Required]
        [StringLength(4, 1000, LinkLengthErrorMessage)]
        public string Link { get; set; }

        [Required]
        [Range(typeof(decimal), "1", "1000", PriceRangeErrorMessage)]
        public decimal Price { get; set; }
    }
}
