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


        private List<IUnityModel> resultList;
        private object lockResultList;
        private HighwayTypeEnum[] types;

        public OsmDataManager(OsmData osmData)
        {
            resultList = new List<IUnityModel>();
            OsmData = osmData;
            lockResultList = new object();
        }

        public IEnumerable<IUnityModel> GetHighways(params HighwayTypeEnum[] types)
        {
            resultList = new List<IUnityModel>();

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

            ThreadPoolManager threadPoolManager = new ThreadPoolManager(FillWaysNodeThreadPoolCallback);
            threadPoolManager.Invoke(osmHighwaysFulfilledParams);


            threadPoolManager = new ThreadPoolManager(HighwayFillResultsThreadPoolCallback);
            threadPoolManager.Invoke(osmHighwaysFulfilledParams);

            return resultList;
        }

        public IEnumerable<IUnityModel> GetBuilding()
        {
            resultList = new List<IUnityModel>();

            if (OsmData == null)
            {
                return resultList;
            }

            List<OsmWay> osmBuilding = new List<OsmWay>();

            foreach (var way in OsmData.Ways)
            {
                if (way.Value.Tags.ContainsKey(TagKeyEnum.Building))
                {
                    osmBuilding.Add(way.Value);
                }
            }

            ThreadPoolManager threadPoolManager = new ThreadPoolManager(FillWaysNodeThreadPoolCallback);
            threadPoolManager.Invoke(osmBuilding);


            threadPoolManager = new ThreadPoolManager(BuildingsFillResultsThreadPoolCallback);
            threadPoolManager.Invoke(osmBuilding);

            return resultList;
        }

        private void FillWaysNodeThreadPoolCallback(object threadContext)
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


        private void BuildingsFillResultsThreadPoolCallback(object threadContext)
        {
            IEnumerable<OsmWay> osmWays = threadContext as IEnumerable<OsmWay>;

            foreach (var osmWay in osmWays)
            {
                if (osmWay.Nodes.Count > 0)
                {

                    UnityBuilding newUnityway = new UnityBuilding(osmWay);
                    if (newUnityway != null)
                    {
                        lock (lockResultList)
                        {
                            resultList.Add(newUnityway);
                        }
                    }



                }
            }
        }

    }
}
