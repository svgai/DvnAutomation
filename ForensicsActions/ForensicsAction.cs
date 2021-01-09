using System;
using System.Collections.Generic;
using System.Text;
using ForensicsObjects;

namespace ForensicsActions
{
    public class ForensicsAction
    {
        public string Id { get; set; }

        public ActionTypes Type { get; set; }

        public long Timestamp { get; set; }

        public ForensicsObject Object { get; set; }

        public ForensicsObject Actor { get; set; }
    }
}
