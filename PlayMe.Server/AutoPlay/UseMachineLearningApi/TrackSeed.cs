using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayMe.Server.AutoPlay.UseMachineLearningApi
{
    public class TrackSeed
    {
        public TrackSeed() { }

        public TrackSeed(
            string trackId,
            string trackName,
            string artistName)
        {
            TrackId = trackId;
            TrackName = trackName;
            ArtistName = artistName;
        }

        public string TrackId { get; set; }

        public string TrackName { get; set; }

        public string ArtistName { get; set; }
    }
}
