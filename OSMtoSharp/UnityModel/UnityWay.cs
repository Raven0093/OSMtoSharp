using System;
using System.Collections.Generic;
using System.Text;

namespace OSMtoSharp
{
    public class UnityWay
    {
        public string Name { get; set; }
        public HighwayTypeEnum HighwayType { get; set; }
        public List<OsmPoint> HighwayPoints { get; set; }
        public UnityWay(OsmWay osmWay, HighwayTypeEnum type)
        {
            HighwayType = type;
            Name = "<no name>";
            HighwayPoints = new List<OsmPoint>();

            if (osmWay.Tags.ContainsKey(TagKeyEnum.Name))
            {

                Name = osmWay.Tags[TagKeyEnum.Name];
            }

            foreach (var node in osmWay.Nodes)
            {
                if (node.Point != null)
                {
                    HighwayPoints.Add(node.Point);
                }
            }


        }
    }
}
