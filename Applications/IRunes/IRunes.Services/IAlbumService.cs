using IRunes.Models;
using System.Collections.Generic;

namespace IRunes.Services
{
    public interface IAlbumService
    {
        Album CreateAlbum(Album album);

        bool AddTrackToAlbum(string albumId, Track track);

        ICollection<Album> GetAllAlbums();

        Album GetAlbumById(string id);
    }
}
