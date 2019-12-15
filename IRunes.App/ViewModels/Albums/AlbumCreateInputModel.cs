using SIS.MvcFramework.Attributes.Validation;

namespace IRunes.App.ViewModels.Albums
{
    public class AlbumCreateInputModel
    {
        [Required]
        [StringLength(4, 20, "The Name length should be between 4 and 20 characters.")]
        public string Name { get; set; }

        [Required]
        [StringLength(15, 255, "The Cover length should be between 15 and 255 characters.")]
        public string Cover { get; set; }
    }
}
