using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Xml;

namespace OSMtoSharp
{
    public class OsmWay : AbstractOsmNode
    {
        public List<long> Nds { get; set; }
        public List<OsmNode> Nodes { get; set; }

        public OsmWay()
        {
            Nds = new List<long>();
            Nodes = new List<OsmNode>();
        }

        public void FillNodes()
        {
            foreach (var nd in Nds)
            {
                if (parent.Nodes.ContainsKey(nd))
                {
                    Nodes.Add(parent.Nodes[nd]);
                }
            }
        }
    }
}
