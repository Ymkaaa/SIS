using IRunes.App.ViewModels.Albums;
using IRunes.Models;
using IRunes.Services;
using SIS.MvcFramework;
using SIS.MvcFramework.Attributes.Http;
using SIS.MvcFramework.Attributes.Security;
using SIS.MvcFramework.Mapping;
using SIS.MvcFramework.Result;
using System.Collections.Generic;
using System.Linq;

namespace IRunes.App.Controllers
{
    public class AlbumsController : Controller
    {
        private readonly IAlbumService albumService;

        public AlbumsController(IAlbumService albumService)
        {
            this.albumService = albumService;
        }

        [Authorize]
        public IActionResult All()
        {
            ICollection<Album> albums = albumService.GetAllAlbums();
            return this.View(albums.Select(ModelMapper.ProjectTo<AlbumAllViewModel>).ToList());
        }

        [Authorize]
        public IActionResult Create()
        {
            return this.View();
        }

        [Authorize]
        [HttpPost(ActionName = "Create")]
        public IActionResult CreateConfirm(AlbumCreateInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.Redirect("/Albums/Create");
            }

            Album album = new Album()
            {
                Name = model.Name,
                Cover = model.Cover,
                Price = 0M
            };

            this.albumService.CreateAlbum(album);

            return this.Redirect("/Albums/All");
        }

        [Authorize]
        public IActionResult Details(AlbumDetailsInputModel model)
        {
            Album albumFromDb = albumService.GetAlbumById(model.Id);

            if (albumFromDb == null)
            {
                return this.Redirect("/Albums/All");
            }

            return this.View(ModelMapper.ProjectTo<AlbumDetailsViewModel>(albumFromDb));
        }
    }
}
