using IRunes.App.ViewModels.Tracks;
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
    public class TracksController : Controller
    {
        private readonly IAlbumService albumService;
        private readonly ITrackService trackService;

        public TracksController()
        {
            this.albumService = new AlbumService();
            this.trackService = new TrackService();
        }

        [Authorize]
        public ActionResult Create()
        {
            Album album = this.albumService.GetAlbumById(this.Request.QueryData["albumId"].ToString());

            return this.View(ModelMapper.ProjectTo<TrackCreateViewModel>(album), "Create");
        }

        [HttpPost(ActionName = "Create")]
        [Authorize]
        public ActionResult CreateConfirm()
        {
            string albumId = this.Request.QueryData["albumId"].ToString();
            string name = ((ISet<string>)this.Request.FormData["name"]).FirstOrDefault();
            string link = ((ISet<string>)this.Request.FormData["link"]).FirstOrDefault();
            decimal price = decimal.Parse(((ISet<string>)this.Request.FormData["price"]).FirstOrDefault());

            Track track = new Track()
            {
                Name = name,
                Link = link,
                Price = price
            };

            if (!this.albumService.AddTrackToAlbum(albumId, track))
            {
                return this.Redirect("/Albums/All");
            }

            return this.Redirect($"/Albums/Details?id={albumId}");
        }

        [Authorize]
        public ActionResult Details()
        {
            string albumId = this.Request.QueryData["albumId"].ToString();
            string trackId = this.Request.QueryData["trackId"].ToString();

            Track trackFromDb = this.trackService.GetTrackById(trackId);

            if (trackFromDb == null)
            {
                return this.Redirect($"/Albums/Details?{trackId}");
            }

            TrackDetailsViewModel trackDetailsViewModel = ModelMapper.ProjectTo<TrackDetailsViewModel>(trackFromDb);
            trackDetailsViewModel.AlbumId = albumId;

            return this.View(trackDetailsViewModel);
        }
    }
}
