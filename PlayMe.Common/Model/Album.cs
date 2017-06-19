using System.Collections.Generic;
using System.Diagnostics;

namespace PlayMe.Common.Model
{
    [DebuggerDisplay("Album: {Name}")]
    public class Album: PlayMeObject
    {        
        public Artist Artist { get; set; }
        public int Year { get; set; }
        public string ArtworkId { get; set; }
        public IEnumerable<Track> Tracks { get; set; }
        public bool IsAvailable { get; set; }
        public string ArtworkUrlLarge { get; set; }
        public string ArtworkUrlMedium { get; set; }
        public string ArtworkUrlSmall { get; set; }
    }
}