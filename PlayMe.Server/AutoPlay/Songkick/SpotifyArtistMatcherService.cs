using PlayMe.Plumbing.Diagnostics;
using PlayMe.Server.Providers.NewSpotifyProvider;
using SpotifyAPI.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayMe.Server.AutoPlay.Songkick
{
    public class SpotifyArtistMatcherService
    {
        private readonly NewSpotifyProvider _spotify;
        private readonly ILogger _logger;

        private readonly Random _random;

        public SpotifyArtistMatcherService(
            NewSpotifyProvider spotify,
            ILogger logger
            )
        {
            _spotify = spotify;
            _logger = logger;

            _random = new Random();
        }

        public FullArtist FindSpotifyArtist(string artistName)
        {
            // -- Find matching artist candidates from Spotify

            var spotifyClient = _spotify.GetClient();

            var searchableArtistName = GetSearchableArtistName(artistName);

            var artistSearchResults = spotifyClient.SearchItems(
                searchableArtistName,
                SpotifyAPI.Web.Enums.SearchType.Artist,
                market: "NZ");

            if (artistSearchResults.Artists == null
                || artistSearchResults.Artists.Total == 0)
            {
                return null;
            }

            // -- Pick a reasonably-exact match

            var compareOptions = System.Globalization.CompareOptions.IgnoreNonSpace // Make sure to allow mācrön-matches
                | System.Globalization.CompareOptions.IgnoreSymbols
                | System.Globalization.CompareOptions.IgnoreCase;

            var comparer = System.Globalization.CultureInfo.CurrentCulture.CompareInfo;

            var searchableArtistKey = MakeSearchKey(artistName);
            var firstArtistResult = artistSearchResults.Artists.Items
                .Where(a => comparer.Compare(MakeSearchKey(a.Name), searchableArtistKey, compareOptions) == 0)
                .FirstOrDefault();

            if (firstArtistResult == null)
            {
                _logger.Debug($"No artist match found for: '{artistName}'");
            }

            return firstArtistResult;
        }

        private string MakeSearchKey(string s)
        {
            if (s.Contains("&"))
            {
                s = s.Replace("&", "and");
            }

            return s;
        }

        private string GetSearchableArtistName(string artistName)
        {
            if (artistName.EndsWith(" NZ", StringComparison.InvariantCultureIgnoreCase))
            {
                artistName = artistName.Replace(" NZ", "");
            }

            if (artistName.EndsWith(" (NZ)", StringComparison.InvariantCultureIgnoreCase))
            {
                artistName = artistName.Replace(" (NZ)", "");
            }

            return artistName;
        }

        public FullTrack PickTrackByArtist(FullArtist spotifyArtist)
        {
            var client = _spotify.GetClient();
            var artistTopTracks = client.GetArtistsTopTracks(spotifyArtist.Id, "NZ");

            if (artistTopTracks.Tracks == null
                || !artistTopTracks.Tracks.Any())
            {
                return null;
            }

            var randomIndex = _random.Next(artistTopTracks.Tracks.Count());
            return artistTopTracks.Tracks.ToArray()[randomIndex];
        }
    }
}
