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

        public List<OsmWay> WaysList
        {
            get
            {
                List<OsmWay> result = new List<OsmWay>();
                foreach (var way in Ways)
                {
                    result.Add(way.Value);
                }
                return result;

            }
        }

        public void RemoveWaysWithoutNodes()
        {
            List<long> idsToRemove = new List<long>();
            foreach (var way in Ways)
            {
                if (way.Value.Nodes.Count == 0)
                {
                    idsToRemove.Add(way.Key);
                }
            }
            foreach (var idToRemove in idsToRemove)
            {
                Ways.Remove(idToRemove);
            }
        }

        public void RemoveRelationsWithoutMembers()
        {
            List<long> idsToRemove = new List<long>();
            foreach (var relation in Relations)
            {
                int fillMembers = 0;
                foreach (var member in relation.Value.Members)
                {
                    if (member.Value != null)
                    {
                        fillMembers++;
                    }
                }
                if (fillMembers == 0)
                {
                    idsToRemove.Add(relation.Key);
                }
            }
            foreach (var idToRemove in idsToRemove)
            {
                Relations.Remove(idToRemove);
            }
        }
    }

}
