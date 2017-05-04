using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlayMe.Common.Model;
using PlayMe.Plumbing.Diagnostics;
using PlayMe.Server.Providers.NewSpotifyProvider;
using PlayMe.Server.Providers.NewSpotifyProvider.Mappers;
using SpotifyAPI.Web.Models;

namespace PlayMe.Server.Providers.NewSpotifyProvider
{
    public interface ISpotifySearchService
    {
        SearchResults SearchAll(string searchTerm, string user);
        Artist BrowseArtist(string link, bool mapTracks);
        Album BrowseAlbum(string link, string user);
    }

    public class NewSpotifySearchService : ISpotifySearchService
    {

        private readonly NewSpotifyProvider _spotify;
        private readonly ILogger _logger;

        private readonly ITrackMapper _trackMapper;
        private readonly IArtistMapper _artistMapper;
        private readonly IAlbumMapper _albumMapper;

        public NewSpotifySearchService(
            // TODO: Inject these via interfaces
            NewSpotifyProvider spotify,
            ILogger logger,
            ITrackMapper trackMapper,
            IAlbumMapper albumMapper,
            IArtistMapper artistMapper
            )
        {
            _spotify = spotify;
            _logger = logger;

            _trackMapper = trackMapper;
            _artistMapper = artistMapper;
            _albumMapper = albumMapper;

        }

        public SearchResults SearchAll(string searchTerm, string user)
        {
            var client = _spotify.GetClient();

            var searchResults = client.SearchItems(
                searchTerm, 
                SpotifyAPI.Web.Enums.SearchType.All, 
                market: "NZ");

            var artists = new ArtistPagedList
            {
                Total = searchResults.Artists.Items.Count(),
                Artists = searchResults.Artists.Items.Select(a =>
                   _artistMapper.Map(a)).ToArray()
            };

            var albums = new AlbumPagedList
            {
                Total = searchResults.Albums.Total,
                Albums = searchResults.Albums.Items.Select(
                    a => _albumMapper.Map(a)).ToArray()
            };

            var tracks = new TrackPagedList
            {
                Total = searchResults.Tracks.Total,
                Tracks = searchResults.Tracks.Items.Select(
                    t => _trackMapper.Map(t, user))
            };

            var results = new SearchResults();

            results.PagedArtists = artists;
            results.PagedAlbums = albums;
            results.PagedTracks = tracks;

            return results;
        }

        public Artist BrowseArtist(string link, bool mapTracks)
        {
            var client = _spotify.GetClient();

            var artist = client.GetArtist(link);
            var artistAlbums = client.GetArtistsAlbums(link, market: "NZ");
            var relatedArtists = client.GetRelatedArtists(link);

            var artistResult = new ArtistMapper().Map(artist);
            artistResult.Albums = artistAlbums.Items.Select(
                a => _albumMapper.Map(a)).ToArray();

            return artistResult;
        }

        public Album BrowseAlbum(string link, string user)
        {
            var client = _spotify.GetClient();

            var album = client.GetAlbum(link);

            var albumResult = _albumMapper.Map(album);

            albumResult.Tracks = album.Tracks.Items.Select(
                t => _trackMapper.Map(t, user)).ToArray();

           
            // TODO: Map tracks in mapper? How to avoid cyclic dependencies??????

            return albumResult;
        }
    }
}
