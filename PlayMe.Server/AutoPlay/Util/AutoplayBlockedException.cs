using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayMe.Server.AutoPlay.Util
{
    public class AutoplayBlockedException : Exception
    {
        public AutoplayBlockedException(string message) : base(message)
        {
            
        }
    }
}
