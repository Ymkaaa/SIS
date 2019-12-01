using System.Collections.Generic;
using System.Linq;
using IRunes.Data;
using IRunes.Models;
using Microsoft.EntityFrameworkCore;

namespace IRunes.Services
{
    public class AlbumService : IAlbumService
    {
        private RunesDbContext context;

        public AlbumService()
        {
            this.context = new RunesDbContext();
        }

        public Album CreateAlbum(Album album)
        {
            using (RunesDbContext context = new RunesDbContext())
            {
                album = context.Albums.Add(album).Entity;
                context.SaveChanges();

                return album;
            }
        }
        
        public bool AddTrackToAlbum(string albumId, Track track)
        {
            Album albumFromDb = this.GetAlbumById(albumId);

            if (albumFromDb == null)
            {
                return false;
            }

            albumFromDb.Tracks.Add(track);
            albumFromDb.Price = (albumFromDb.Tracks.Select(track => track.Price).Sum() * 0.87M);

            this.context.Update(albumFromDb);
            this.context.SaveChanges();

            return true;
        }

        public Album GetAlbumById(string id)
        {
            return this.context.Albums
                .Include(a => a.Tracks)
                .SingleOrDefault(a => a.Id == id);
        }

        public ICollection<Album> GetAllAlbums()
        {
            return this.context.Albums.ToList();
        }
    }
}
