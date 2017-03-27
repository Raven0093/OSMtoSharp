using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace OSMtoSharp
{
    public class OsmDataManager
    {
        private OsmData osmData { get; set; }

        public OsmDataManager(double minLong, double minLat, double maxLong, double maxLat)
        {
            osmData = OsmParser.GetDataFromOSM(OsmIOManager.LoadOsmDocument(minLong, minLat, maxLong, maxLat));
        }

        public IEnumerable<UnityWay> GetHighways(params HighwayTypeEnum[] types)
        {
            List<OsmWay> osmHighways = new List<OsmWay>();

            foreach (var way in osmData.Ways)
            {
                if (way.Tags.ContainsKey(TagKeyEnum.Highway))
                {
                    osmHighways.Add(way);
                }
            }

            List<UnityWay> resultList = new List<UnityWay>();

            foreach (var osmHighway in osmHighways)
            {
                HighwayTypeEnum highwayType = EnumExtensions.GetTagKeyEnum<HighwayTypeEnum>(osmHighway.Tags[TagKeyEnum.Highway]);
                if (highwayType != HighwayTypeEnum.None)
                {
                    foreach (var type in types)
                    {
                        if (highwayType == HighwayTypeEnum.Proposed)
                        {
                            if (osmHighway.Tags.ContainsKey(TagKeyEnum.Proposed))
                            {
                                HighwayTypeEnum proposedType = EnumExtensions.GetTagKeyEnum<HighwayTypeEnum>(osmHighway.Tags[TagKeyEnum.Proposed]);
                                if (proposedType == type)
                                {
                                    resultList.Add(new UnityWay(osmHighway, type));
                                    break;
                                }

                            }
                        }
                        else if (highwayType == type)
                        {
                            resultList.Add(new UnityWay(osmHighway, type));
                            break;
                        }

                    }

                }
            }

            return resultList;
        }

    }
}
