using SIS.MvcFramework.Attributes.Validation;

namespace IRunes.App.ViewModels.Tracks
{
    public class CreateInputModel
    {
        public string AlbumId { get; set; }

        [StringLength(3, 20, "Length should be between 3 and 20 characters.")]
        public string Name { get; set; }

        public string Link { get; set; }

        public decimal Price { get; set; }
    }
}
