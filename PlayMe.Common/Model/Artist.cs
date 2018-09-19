﻿using System.Collections.Generic;
using System.Diagnostics;

namespace PlayMe.Common.Model
{
    [DebuggerDisplay("Artist: {Name}")]
    public class Artist : PlayMeObject
    {
        public enum LoadedState
        {
            Basic,
            FullNoTracks,
            FullWithTracks
        }

        public ArtistProfile Profile { get; set; }
        public string PortraitId { get; set; }
        public string PortraitUrlLarge { get; set; }
        public string PortraitUrlMedium { get; set; }
        public string PortraitUrlSmall { get; set; }
        public IEnumerable<Album> Albums { get; set; }
        public IEnumerable<Track> Tracks { get; set; }
        public LoadedState LoadStatus { get; set; }
    }
}