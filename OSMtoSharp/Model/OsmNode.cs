using OSMtoSharp.Model.Abstract;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Xml;

namespace OSMtoSharp.Model
{
    public class OsmNode : AbstractOsmNode
    {
        public Point Point { get; set; }

        public OsmNode() { }
    }
}
