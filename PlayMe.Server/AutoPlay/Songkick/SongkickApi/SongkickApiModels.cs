using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayMe.Server.AutoPlay.Songkick.SongkickApi
{
    public class UpcomingEventsResponse
    {
        public ResultsPage ResultsPage { get; set; }
    }

    public class ResultsPage
    {
        public Results Results;

        public int TotalEntries { get; set; }
        public int PerPage { get; set; }

        public int Page { get; set; }

        public string Status { get; set; }
    }

    public class Results
    {
        public List<Event> Event { get; set; }
    }

    [DebuggerDisplay("{DisplayName}")]
    public class Event
    {
        public int Id { get; set; }

        public string Type { get; set; }

        public string Uri { get; set; }

        public string DisplayName { get; set; }

        public List<Artist> Performance { get; set; }

        public decimal Popularity { get; set; }

    }

    
    [DebuggerDisplay("{DisplayName}")]
    public class Artist
    {
        public int Id { get; set; }
        
        public string DisplayName { get; set; }

        public string Billing { get; set; }

        public int BillingIndex { get; set; }

    }
}
