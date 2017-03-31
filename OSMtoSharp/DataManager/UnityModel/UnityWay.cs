using System;
using System.Collections.Generic;
using System.Text;

namespace OSMtoSharp
{
    public class UnityWay : IUnityModel
    {
        public string Name { get; set; }
        public HighwayTypeEnum HighwayType { get; set; }
        public List<UnityPoint> HighwayPoints { get; set; }
        public UnityWay(OsmWay osmWay, HighwayTypeEnum type)
        {
            HighwayType = type;
            Name = "<no name>";
            HighwayPoints = new List<UnityPoint>();

            if (osmWay.Tags.ContainsKey(TagKeyEnum.Name))
            {

                Name = osmWay.Tags[TagKeyEnum.Name];
            }

            foreach (var node in osmWay.Nodes)
            {
                if (node != null)
                {
                    HighwayPoints.Add(new UnityPoint()
                    {
                        Lat = (float)node.Lat,
                        Lon = (float)node.Lon
                    });
                }
            }


        }
    }
}
