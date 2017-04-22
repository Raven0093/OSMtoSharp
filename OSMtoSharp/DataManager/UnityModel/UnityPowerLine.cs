//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace OSMtoSharp
//{
//    public class UnityPowerLine : IUnityModel
//    {
//        public PowerLineTypeEnum PowerLineType { get; set; }
//        public List<UnityPoint> PowerLinePoints { get; set; }

//        public UnityPowerLine(OsmWay osmWay, PowerLineTypeEnum type)
//        {
//            PowerLineType = type;

//            PowerLinePoints = new List<UnityPoint>();

//            foreach (var node in osmWay.Nodes)
//            {
//                if (node != null)
//                {
//                    PowerLinePoints.Add(new UnityPoint()
//                    {
//                        Lat = (float)node.Lat,
//                        Lon = (float)node.Lon
//                    });
//                }
//            }


//        }
//    }
//}
