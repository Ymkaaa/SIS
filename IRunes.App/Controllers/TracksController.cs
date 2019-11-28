using IRunes.App.Extensions;
using IRunes.Data;
using IRunes.Models;
using SIS.HTTP.Requests.Contracts;
using SIS.HTTP.Responses.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace IRunes.App.Controllers
{
    public class TracksController : BaseController
    {
        public IHttpResponse Create(IHttpRequest request)
        {
            if (!this.IsLoggedIn(request))
            {
                return this.Redirect("/Users/Login");
            }

            string albumId = request.QueryData["albumId"].ToString();

            this.ViewData["AlbumId"] = albumId;

            return this.View(); 
        }

        public IHttpResponse CreateConfirm(IHttpRequest request)
        {
            if (!this.IsLoggedIn(request))
            {
                return this.Redirect("/Users/Login");
            }

            string albumId = request.QueryData["albumId"].ToString();

            using (RunesDbContext context = new RunesDbContext())
            {
                Album albumFromDb = context.Albums.SingleOrDefault(album => album.Id == albumId);

                if (albumFromDb == null)
                {
                    return this.Redirect("/Albums/All");
                }

                string name = ((ISet<string>)request.FormData["name"]).FirstOrDefault();
                string link = ((ISet<string>)request.FormData["link"]).FirstOrDefault();
                decimal price = decimal.Parse(((ISet<string>)request.FormData["price"]).FirstOrDefault());

                Track track = new Track()
                {
                    Name = name,
                    Link = link,
                    Price = price
                };

                albumFromDb.Tracks.Add(track);
                albumFromDb.Price = (albumFromDb.Tracks.Select(track => track.Price).Sum() * 0.87M);
                context.Update(albumFromDb);
                context.SaveChanges();
            }

            return this.Redirect($"/Albums/Details?id={albumId}");
        }

        public IHttpResponse Details(IHttpRequest request)
        {
            if (!this.IsLoggedIn(request))
            {
                return this.Redirect("/Users/Login");
            }

            string albumId = request.QueryData["albumId"].ToString();
            string trackId = request.QueryData["trackId"].ToString();

            using (RunesDbContext context = new RunesDbContext())
            {
                Track trackFromDb = context.Tracks.SingleOrDefault(track => track.Id == trackId);

                if (trackFromDb == null)
                {
                    return this.Redirect($"/Albums/Details?{trackId}");
                }
                
                this.ViewData["AlbumId"] = albumId;
                this.ViewData["Track"] = trackFromDb.ToHtmlDetails(albumId);
            }

            return this.View();
        }
    }
}
