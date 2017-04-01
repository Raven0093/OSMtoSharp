using System;
using System.Collections.Generic;
using System.Text;

namespace OSMtoSharp
{
    public class UnityRailway : IUnityModel
    {
        public HighwayTypeEnum HighwayType { get; set; }
        public List<UnityPoint> HighwayPoints { get; set; }
        public UnityRailway(OsmWay osmWay)
        {
            //HighwayType = type;

            HighwayPoints = new List<UnityPoint>();

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
