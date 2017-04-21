using System.Collections.Generic;

namespace OSMtoSharp
{
    public class UnityBuilding : IUnityModel
    {
        public string Name { get; set; }
        public List<UnityPoint> BuildingPoints { get; set; }
        public BuildingsTypeEnum BuildingsType { get; set; }
        public int BuildingLevels { get; set; }

        public UnityBuilding(OsmWay osmWay, BuildingsTypeEnum type)
        {
            BuildingsType = type;
            Name = "<building>";
            BuildingPoints = new List<UnityPoint>();

            int level = 1;
            if (osmWay.Tags.ContainsKey(TagKeyEnum.BuildingLevels))
            {
                int.TryParse(osmWay.Tags[TagKeyEnum.BuildingLevels], out level);
            }
            BuildingLevels = level;

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