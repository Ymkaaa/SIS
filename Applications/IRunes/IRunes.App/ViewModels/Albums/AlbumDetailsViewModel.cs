﻿using System.Collections.Generic;

namespace IRunes.App.ViewModels.Albums
{
    public class AlbumDetailsViewModel
    {
        public AlbumDetailsViewModel()
        {
            this.Tracks = new List<TrackAlbumDetailsViewModel>();
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public string Cover { get; set; }

        public decimal Price { get; set; }

        public List<TrackAlbumDetailsViewModel> Tracks { get; set; }
    }
}
