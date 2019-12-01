using System.Linq;
using IRunes.Data;
using IRunes.Models;

namespace IRunes.Services
{
    public class TrackService : ITrackService
    {
        private RunesDbContext context;

        public TrackService()
        {
            this.context = new RunesDbContext();
        }

        public Track CreateTrack(Track track)
        {
            track = this.context.Tracks.Add(track).Entity;
            this.context.SaveChanges();

            return track;
        }

        public Track GetTrackById(string trackId)
        {
            return this.context.Tracks.SingleOrDefault(t => t.Id == trackId);
        }
    }
}
