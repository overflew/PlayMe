using Nerdle.AutoConfig;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayMe.Server.AutoPlay.Songkick.SongkickApi
{
    // Songkick API docs: https://www.songkick.com/developer

    public class SongkickApiService
    {
        private const string SongkickApiBaseUrl = "http://api.songkick.com/api/3.0";

        private readonly ISongkickConfig _songkickConfig;

        public SongkickApiService()
        {
            _songkickConfig = AutoConfig.Map<ISongkickConfig>();
        }

        // Docs: https://www.songkick.com/developer/upcoming-events-for-metro-area
        public UpcomingEventsResponse GetUpcomingEvents(int regionId)
        {
            var client = new RestClient(SongkickApiBaseUrl);
            var request = new RestRequest($"metro_areas/{regionId}/calendar.json", Method.GET);
            request.AddParameter("apikey", _songkickConfig.apiKey);

            // request.AddParameter("location", regionId);

            var response = client.Execute(request);       
            var data = JsonConvert.DeserializeObject<UpcomingEventsResponse>(response.Content);

            return data;
        }
    }
}
