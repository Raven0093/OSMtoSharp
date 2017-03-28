using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace OSMtoSharp
{
    public class OsmDataManager
    {
        private OsmData osmData { get; set; }


        private List<UnityWay> resultList;
        private object lockResultList;
        private HighwayTypeEnum[] types;

        public OsmDataManager(double minLong, double minLat, double maxLong, double maxLat)
        {
            resultList = new List<UnityWay>();
            osmData = OsmParser.GetDataFromOSM(OsmIOManager.LoadOsmDocument(minLong, minLat, maxLong, maxLat));
            lockResultList = new object();
        }

        public IEnumerable<UnityWay> GetHighways(params HighwayTypeEnum[] types)
        {
            this.types = types;
            List<OsmWay> osmHighways = new List<OsmWay>();

            foreach (var way in osmData.Ways)
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
            foreach (OsmWay unityWay in osmWays)
            {
                unityWay.FillNodes();
            }
        }

        private void HighwayFillResultsThreadPoolCallback(object threadContext)
        {
            IEnumerable<OsmWay> osmWays = threadContext as IEnumerable<OsmWay>;

            foreach (var osmHighwaysFulfilledParam in osmWays)
            {
                HighwayTypeEnum highwayType = EnumExtensions.GetTagKeyEnum<HighwayTypeEnum>(osmHighwaysFulfilledParam.Tags[TagKeyEnum.Highway]);

                foreach (var type in types)
                {
                    if (highwayType == type)
                    {
                        UnityWay newUnityway = new UnityWay(osmHighwaysFulfilledParam, type);
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
