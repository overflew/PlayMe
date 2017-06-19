using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayMe.Server.Queue
{
    public class TrackBlockedReason
    {
        public TrackBlockedReason()
        {

        }

        public TrackBlockedReason(string reason)
        {
            Reason = reason;
        }

        public string Reason { get; set; }
    }
}
