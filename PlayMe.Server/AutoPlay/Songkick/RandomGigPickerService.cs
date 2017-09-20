using Nerdle.AutoConfig;
using PlayMe.Common.Util;
using PlayMe.Server.AutoPlay.Songkick.SongkickApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayMe.Server.AutoPlay.Songkick
{
    public class RandomGigPickerService
    {
        private const int WEIGHTING_HEADLINER = 3;
        private const int WEIGHTING_SUPPORT = 1;

        private Random _random = new Random();

        private Event PickRandomGig(List<Event> upcomingEvents)
        {
            var randomIndex = _random.Next(upcomingEvents.Count());
            return upcomingEvents.ToArray()[randomIndex];
        }

        public ArtistGigResult PickRandomGig_ByArtistWeight(List<Event> upcomingEvents)
        {
            var artistKeys = upcomingEvents.SelectMany(e => e.Performance.Select(p => new
            {
                ArtistName = p.DisplayName
            }));

            var distinctArtistKeys = artistKeys.Distinct();

            var artistEventsLookup = artistKeys.Select(a => new
            {
                key = a,
                events = upcomingEvents.Where(e =>
                    e.Performance.Any(p => p.DisplayName == a.ArtistName))
                                 .ToList(),
                billings = upcomingEvents.SelectMany(e =>
                    e.Performance.Where(p => p.DisplayName == a.ArtistName))
                                 .ToList()
            });

            var artistsWithMultipleEvents = artistEventsLookup.Where(ael => ael.events.Count() > 1).ToList();

            var weightedArtistThing = artistEventsLookup.Select(a => new WeightedThing
            {
                ArtistName = a.key.ArtistName,
                Event = a.events.First(),
                Weight = GetWeighting(a.billings)
            }).ToList();

            var x = WeightingUtil.ChooseWeightedRandom(weightedArtistThing);

            return new ArtistGigResult() {
                ArtistName = x.ArtistName,
                Event = x.Event
            };
        }

        private class WeightedThing : IWeighted
        {
            public string ArtistName { get; set; }

            public Event Event { get; set; }
            public int Weight { get; set; }
        }

        private int GetWeighting(IEnumerable<Artist> artistBillings)
        {
            return artistBillings.Any(b => b.Billing.Equals("headline", StringComparison.InvariantCultureIgnoreCase))
                ? WEIGHTING_HEADLINER
                : WEIGHTING_SUPPORT
        }
    }

    public class ArtistGigResult
    {
        public string ArtistName { get; set; }
        public Event Event { get; set; }
    }
}
