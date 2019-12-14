using IRunes.App.ViewModels.Tracks;
using IRunes.Models;
using IRunes.Services;
using SIS.MvcFramework;
using SIS.MvcFramework.Attributes.Http;
using SIS.MvcFramework.Attributes.Security;
using SIS.MvcFramework.Mapping;
using SIS.MvcFramework.Result;

namespace IRunes.App.Controllers
{
    public class TracksController : Controller
    {
        private readonly IAlbumService albumService;
        private readonly ITrackService trackService;

        public TracksController(IAlbumService albumService, ITrackService trackService)
        {
            this.albumService = albumService;
            this.trackService = trackService;
        }

        [Authorize]
        public ActionResult Create(string albumId)
        {
            Album album = this.albumService.GetAlbumById(albumId);

            return this.View(ModelMapper.ProjectTo<TrackCreateViewModel>(album), "Create");
        }

        [HttpPost(ActionName = "Create")]
        [Authorize]
        public ActionResult CreateConfirm(CreateInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.Redirect("/");
            }

            Track track = new Track()
            {
                Name = model.Name,
                Link = model.Link,
                Price = model.Price
            };

            if (!this.albumService.AddTrackToAlbum(model.AlbumId, track))
            {
                return this.Redirect("/Albums/All");
            }

            return this.Redirect($"/Albums/Details?id={model.AlbumId}");
        }

        [Authorize]
        public ActionResult Details(DetailsInputModel model)
        {
            Track trackFromDb = this.trackService.GetTrackById(model.TrackId);

            if (trackFromDb == null)
            {
                return this.Redirect($"/Albums/Details?{model.TrackId}");
            }

            TrackDetailsViewModel trackDetailsViewModel = ModelMapper.ProjectTo<TrackDetailsViewModel>(trackFromDb);
            trackDetailsViewModel.AlbumId = model.AlbumId;

            return this.View(trackDetailsViewModel);
        }
    }
}
