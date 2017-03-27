using System;
using System.Collections.Generic;
using System.Text;

namespace OSMtoSharp
{
    public class FileData
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public double MinLong { get; set; }
        public double MinLat { get; set; }
        public double MaxLong { get; set; }
        public double MaxLat { get; set; }
    }
}
