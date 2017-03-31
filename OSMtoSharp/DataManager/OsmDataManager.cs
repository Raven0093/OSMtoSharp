using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace OSMtoSharp
{
    public class OsmDataManager
    {
        private OsmData OsmData { get; set; }


        private List<UnityWay> resultList;
        private object lockResultList;
        private HighwayTypeEnum[] types;

        public OsmDataManager(OsmData osmData)
        {
            resultList = new List<UnityWay>();
            OsmData = osmData;
            lockResultList = new object();
        }

        public IEnumerable<UnityWay> GetHighways(params HighwayTypeEnum[] types)
        {
            if (OsmData == null)
            {
                return resultList;
            }

            this.types = types;
            List<OsmWay> osmHighways = new List<OsmWay>();

            foreach (var way in OsmData.Ways)
            {
                if (way.Value.Tags.ContainsKey(TagKeyEnum.Highway))
                {
                    osmHighways.Add(way.Value);
                }
            }

            List<OsmWay> osmHighwaysFulfilledParams = new List<OsmWay>();

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
                                    osmHighway.Tags[TagKeyEnum.Highway] = EnumExtensions.GetEnumAttributeValue(type);
                                    osmHighwaysFulfilledParams.Add(osmHighway);
                                    break;
                                }

                            }
                        }
                        else if (highwayType == type)
                        {
                            osmHighwaysFulfilledParams.Add(osmHighway);
                            break;
                        }

                    }

                }
            }

            ThreadPoolManager threadPoolManager = new ThreadPoolManager(HighwayFillWaysNodeThreadPoolCallback);
            threadPoolManager.Invoke(osmHighwaysFulfilledParams);


            threadPoolManager = new ThreadPoolManager(HighwayFillResultsThreadPoolCallback);
            threadPoolManager.Invoke(osmHighwaysFulfilledParams);

            return resultList;
        }

        private void HighwayFillWaysNodeThreadPoolCallback(object threadContext)
        {
            IEnumerable<OsmWay> osmWays = threadContext as IEnumerable<OsmWay>;
            foreach (OsmWay osmWay in osmWays)
            {
                osmWay.FillNodes();
            }
        }

        private void HighwayFillResultsThreadPoolCallback(object threadContext)
        {
            IEnumerable<OsmWay> osmWays = threadContext as IEnumerable<OsmWay>;

            foreach (var osmWay in osmWays)
            {
                if (osmWay.Nodes.Count > 0)
                {
                    HighwayTypeEnum highwayType = EnumExtensions.GetTagKeyEnum<HighwayTypeEnum>(osmWay.Tags[TagKeyEnum.Highway]);

                    foreach (var type in types)
                    {
                        if (highwayType == type)
                        {
                            UnityWay newUnityway = new UnityWay(osmWay, type);
                            if (newUnityway != null)
                            {
                                lock (lockResultList)
                                {
                                    resultList.Add(newUnityway);
                                }
                            }

                            break;
                        }

                    }
                }
            }
        }

    }
}
