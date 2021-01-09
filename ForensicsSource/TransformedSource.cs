using System;
using System.Collections.Generic;
using System.Text;
using ForensicsActions;
using ForensicsObjects;

namespace ForensicsSource
{
    public class TransformedSource
    {
        public List<ForensicsAction> Actions { get; set; }

        public List<ForensicsObject> ExtraObjects { get; set; }

        public TransformedSource()
        {
            Actions = new List<ForensicsAction>();
            ExtraObjects = new List<ForensicsObject>();
        }
    }
}
