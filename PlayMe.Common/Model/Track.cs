﻿using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace PlayMe.Common.Model
{
    [DebuggerDisplay("Song: {Name}")]
    public class Track : PlayMeObject
    {        
        public IEnumerable<Artist> Artists { get; set; }
        public Album Album { get; set; }
        public TimeSpan Duration { get; set; }
        public long DurationMilliseconds { get; set; }
        public bool IsAlreadyQueued { get; set; }
        public bool IsAvailable { get; set; }
        public int Popularity { get; set; }
        public string TrackArtworkUrl { get; set; }
        public string Reason { get; set; }
        public string SongPreviewUrl { get; set; }
    }
}