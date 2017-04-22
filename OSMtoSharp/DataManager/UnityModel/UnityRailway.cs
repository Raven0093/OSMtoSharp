//using OSMtoSharp.Model;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace OSMtoSharp
//{
//    public class UnityRailway : IUnityModel
//    {
//        public RailwayTypeEnum RailwayType { get; set; }
//        public List<UnityPoint> RailwayPoints { get; set; }
//        public UnityRailway(OsmWay osmWay)
//        {
//            //HighwayType = type;

//            RailwayPoints = new List<UnityPoint>();

//            foreach (var node in osmWay.Nodes)
//            {
//                if (node != null)
//                {
//                    RailwayPoints.Add(new UnityPoint()
//                    {
//                        Lat = (float)node.Lat,
//                        Lon = (float)node.Lon
//                    });
//                }
//            }


//        }
//    }
//}
