using System;
using System.Collections.Generic;
using System.Text;

namespace ForensicsObjects
{
    public class File : ForensicsObject
    {
        public int MftNumber { get; set; }

        public int MftSequenceNumber { get; set; }

        public string FullPath { get; set; }

        public string Name { get; set; }

        public string Extension { get; set; }

        public long Size { get; set; }

        public string Attributes { get; set; }

        public bool IsFolder { get; set; }

        public string ExtraBlocks { get; set; }
    }
}
