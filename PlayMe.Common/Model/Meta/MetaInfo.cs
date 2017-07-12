using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayMe.Server.AutoPlay.Meta
{
    // Note: This is a one-size-fits-all class, to allow the Mongo serialisation to work easily.
    public class MetaInfo
    {
        // -- Account/playlist:
        [BsonIgnoreIfNull]
        public string AccountName { get; set; }

        [BsonIgnoreIfNull]
        public string AccountId { get; set; }

        [BsonIgnoreIfNull]
        public string PlaylistName { get; set; }

        [BsonIgnoreIfNull]
        public string PlaylistId { get; set; }

        [BsonIgnoreIfNull]
        public string SpotifyUri { get; set; }

        // -- For recommendations stuff:
        [BsonIgnoreIfNull]
        public IEnumerable<BasicTrack> SeedTracks { get; set; }
    }

    public class BasicTrack
    {
        public string TrackId { get; set; }

        public string TrackName { get; set; }

        public string ArtistId { get; set; }

        public string ArtistName { get; set; }

        // public string SpotifyUri { get; set; }
    }
}
