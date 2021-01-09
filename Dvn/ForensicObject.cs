using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ForensicsObjects
{
    public class ForensicsObject
    {
        public Guid Id { get; set; }

        public ObjectTypes Type { get; set; } = ObjectTypes.Unknown;

        public Dictionary<string, string> Properties { get; set; } = new Dictionary<string, string>();

        public virtual MergeResult Merge(ForensicsObject obj)
        {
            return MergeResult.NoIntersection;
        }

        [JsonIgnore]
        public Dictionary<int, string> MaskHashes { get; set; }

    }
}
