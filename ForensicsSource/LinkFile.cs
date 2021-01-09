using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ForensicsSource
{
    public class LinkFile : ForensicsSource
    {
        private readonly char _delimiter = ',';

        public string SourceFile { get; set; }

        public DateTime? SourceCreatedTimestamp { get; set; }

        public DateTime? SourceModifiedTimestamp { get; set; }
        public DateTime? SourceAccessedTimestamp { get; set; }
        public DateTime? TargetCreatedTimestamp { get; set; }
        public DateTime? TargetModifiedTimestamp { get; set; }
        public DateTime? TargetAccessedTimestamp { get; set; }
        public long? FileSize { get; set; }
        public string RelativePath { get; set; }
        public string WorkingDirectory { get; set; }
        public string FileAttributes { get; set; }
        public string HeaderFlags { get; set; }
        public string DriveType { get; set; }
        public int VolumeSerialNumber { get; set; }
        public string VolumeLabel { get; set; }
        public string LocalPath { get; set; }
        public string NetworkPath { get; set; }
        public string CommonPath { get; set; }
        public string Arguments { get; set; }
        public string TargetIdAbsolutePath { get; set; }
        public int? TargetMftEntryNumber { get; set; }
        public int? TargetMftSequenceNumber { get; set; }
        public string MachineId { get; set; }
        public string MachineMacAddress { get; set; }
        public string MacVendor { get; set; }
        public DateTime TrackerCreatedOn { get; set; }
        public string ExtraBlocksPresent { get; set; }

        public string Type => "LinkFileObject";

        public string Provider => ".LNK Files";

        public ForensicsSource FromString(string value)
        {
            throw new NotImplementedException();
        }

        public TransformedSource Transform()
        {
            throw new NotImplementedException();
        }
    }
}