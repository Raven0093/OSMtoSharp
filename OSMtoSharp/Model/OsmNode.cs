using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Xml;

namespace OSMtoSharp
{
    public class OsmNode : AbstractOsmNode
    {
        public double Lat { get; set; }
        public double Lon { get; set; }

        public OsmNode() { }
    }
}
