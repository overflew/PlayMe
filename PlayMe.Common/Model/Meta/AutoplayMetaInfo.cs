using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayMe.Server.AutoPlay.Meta
{
    public class AutoplayMetaInfo
    {
        public string AutoplayNameId { get; set; }

        [BsonIgnoreIfNull]
        public MetaInfo MetaInfo { get; set; }
    }
}
