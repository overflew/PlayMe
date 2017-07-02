using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayMe.Server.AutoPlay.UseMachineLearningApi
{
    public interface IMachineLearningApiService
    {
        TrackSeed DummyTest(TrackSeed seed);
        TrackSeed GetSuggestionFromSeed(TrackSeed seed);

    }

    public class MachineLearningApiService : IMachineLearningApiService
    {
        private const string EndpointBase = "https://someserver";

        // TEMP: Just for testing
        public TrackSeed DummyTest(TrackSeed seed)
        {
            return DummyResult;
        }

        public TrackSeed GetSuggestionFromSeed(TrackSeed seed)
        {
            var request = new RestRequest("recommendation");
            request.AddParameter("seed", seed);

            var client = GetClient();

            var response = client.Execute(request);

            // TODO: Handle & map result. See http://restsharp.org/
            // var response = response.Content
            
            return DummyResult;
        }
        
        private RestClient GetClient()
        {
            return new RestClient(EndpointBase);
        }

        private readonly TrackSeed DummyResult = 
            // For testing, just keep returning this classic
            new TrackSeed(
                "1GhtgRyXuAUdJkVLvrunx4", 
                "Daybreak", 
                "Michael Haggins");
    }
}
