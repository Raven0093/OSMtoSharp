using System;
using System.Collections.Generic;
using System.Text;

namespace OSMtoSharp
{
    public class OsmData
    {
        public OsmData()
        {
            Nodes = new Dictionary<long, OsmNode>();
            Ways = new Dictionary<long, OsmWay>();
            Relations = new Dictionary<long, OsmRelation>();
        }

        public OsmBounds bounds;
        public Dictionary<long, OsmNode> Nodes;
        public Dictionary<long, OsmWay> Ways;
        public Dictionary<long, OsmRelation> Relations;
    }
}
