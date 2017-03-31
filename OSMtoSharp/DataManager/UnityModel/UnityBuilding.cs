using System.Collections.Generic;

namespace OSMtoSharp
{
    public class UnityBuilding : IUnityModel
    {
        public string Name { get; set; }
        public List<UnityPoint> BuildingPoints { get; set; }
        public UnityBuilding(OsmWay osmWay)
        {

            Name = "<building>";
            BuildingPoints = new List<UnityPoint>();

            foreach (var node in osmWay.Nodes)
            {
                if (node != null)
                {
                    BuildingPoints.Add(new UnityPoint()
                    {
                        Lat = (float)node.Lat,
                        Lon = (float)node.Lon
                    });
                }
            }


        }
    }
}