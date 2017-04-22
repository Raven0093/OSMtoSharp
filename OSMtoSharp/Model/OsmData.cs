using System;
using System.Collections.Generic;
using System.Text;

namespace OSMtoSharp.Model
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

        public void RemoveNodesWithoutTags()
        {
            List<long> idsToRemove = new List<long>();

            foreach (var nodeDic in Nodes)
            {
                if (nodeDic.Value.Tags.Count == 0)
                {
                    idsToRemove.Add(nodeDic.Key);
                }
            }
            foreach (var idToRemove in idsToRemove)
            {
                Nodes.Remove(idToRemove);
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

        public void FillWaysNode()
        {
            FillWaysNode(WaysList);
        }

        private void FillWaysNodeThreadPoolCallback(object threadContext)
        {
            IEnumerable<OsmWay> osmWays = threadContext as IEnumerable<OsmWay>;
            foreach (OsmWay osmWay in osmWays)
            {
                osmWay.FillNodes();
            }
        }

        private void FillWaysNode(List<OsmWay> osmHighwaysFulfilled)
        {
            ThreadPoolManager.ThreadPoolManager threadPoolManager = new ThreadPoolManager.ThreadPoolManager(FillWaysNodeThreadPoolCallback);
            threadPoolManager.Invoke(osmHighwaysFulfilled);
        }
    }

}
