using System;
using System.Collections.Generic;
using System.Text;

namespace OSMtoSharp
{
    public class OsmData
    {
        public OsmData()
        {
            Nodes = new List<OsmNode>();
            Ways = new List<OsmWay>();
            Relations = new List<OsmRelation>();
            jobsLock = new object();
        }

        public OsmBounds bounds;
        public List<OsmNode> Nodes;
        public List<OsmWay> Ways;
        public List<OsmRelation> Relations;
        public long jobs;
        public object jobsLock;
    }
}
