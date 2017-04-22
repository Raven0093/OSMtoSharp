//using OSMtoSharp.Model;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace OSMtoSharp
//{
//    public class UnityHighway : IUnityModel
//    {
//        public string Name { get; set; }

//        public HighwayTypeEnum HighwayType { get; set; }
//        public HighwayTypeEnum ProposedHighwayType { get; set; }
//        public HighwayTypeEnum UnderConstructionType { get; set; }

//        public List<UnityPoint> HighwayPoints { get; set; }

//        public UnityHighway(OsmWay osmWay, HighwayTypeEnum type)
//        {
//            HighwayType = type;

//            ProposedHighwayType = HighwayTypeEnum.None;
//            UnderConstructionType = HighwayTypeEnum.None;
//            if (type == HighwayTypeEnum.Proposed)
//            {
//                if (osmWay.Tags.ContainsKey(TagKeyEnum.Proposed))
//                {
//                    ProposedHighwayType = EnumExtensions.GetTagKeyEnum<HighwayTypeEnum>(osmWay.Tags[TagKeyEnum.Proposed]);
//                }
//            }
//            if (type == HighwayTypeEnum.Construction)
//            {
//                if (osmWay.Tags.ContainsKey(TagKeyEnum.Construction))
//                {
//                    UnderConstructionType = EnumExtensions.GetTagKeyEnum<HighwayTypeEnum>(osmWay.Tags[TagKeyEnum.Construction]);
//                }
//            }

//            Name = "<no name>";
//            HighwayPoints = new List<UnityPoint>();

//            if (osmWay.Tags.ContainsKey(TagKeyEnum.Name))
//            {
//                Name = osmWay.Tags[TagKeyEnum.Name];
//            }

//            foreach (var node in osmWay.Nodes)
//            {
//                if (node != null)
//                {
//                    HighwayPoints.Add(new UnityPoint()
//                    {
//                        Lat = (float)node.Lat,
//                        Lon = (float)node.Lon
//                    });
//                }
//            }


//        }
//    }
//}
