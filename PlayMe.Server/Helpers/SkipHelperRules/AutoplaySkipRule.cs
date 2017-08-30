﻿using System;
using PlayMe.Common.Model;
using PlayMe.Server.Helpers.Interfaces;

namespace PlayMe.Server.Helpers.SkipHelperRules
{
    public class AutoplaySkipRule : ISkipRule
    {
        public int GetRequiredVetoCount(QueuedTrack track)
        {
            const int minVetoCount = 2;

            if (track.User == null)
            {
                return int.MaxValue;
            }

            return track.User.StartsWith(Constants.AutoplayUserNameBasePrefix, StringComparison.CurrentCultureIgnoreCase)
                        ? minVetoCount
                        : int.MaxValue;
        }

    }
}