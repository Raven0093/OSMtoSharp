//using OSMtoSharp.Model;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Text;
//using System.Xml;

//namespace OSMtoSharp
//{
//    public class OsmDataManager
//    {
//        private OsmData OsmData { get; set; }

//        private static bool preparingDataStarted;

//        private List<IUnityModel> resultList;
//        private object lockResultList;

//        private HighwayTypeEnum[] highwaysTypes;
//        private BuildingsTypeEnum[] buildingsTypes;
//        private RailwayTypeEnum[] railwaysTypes;

//        public OsmDataManager(OsmData osmData)
//        {
//            resultList = new List<IUnityModel>();
//            OsmData = osmData;
//            preparingDataStarted = false;
//            lockResultList = new object();
//        }




//        public IEnumerable<IUnityModel> GetHighways(bool deleteData, params HighwayTypeEnum[] types)
//        {
//            if (preparingDataStarted)
//            {
//                throw new Exception("preapring data is started in other thread");
//            }
//            else
//            {
//                preparingDataStarted = true;
//            }
//            resultList = new List<IUnityModel>();

//            if (OsmData == null)
//            {
//                return resultList;
//            }

//            this.highwaysTypes = types;
//            List<OsmWay> osmHighways = new List<OsmWay>();

//            foreach (var way in OsmData.Ways)
//            {
//                if (way.Value.Tags.ContainsKey(TagKeyEnum.Highway))
//                {
//                    osmHighways.Add(way.Value);
//                }
//            }

//            if (deleteData)
//            {
//                foreach (var item in osmHighways)
//                {
//                    OsmData.Ways.Remove(item.Id);
//                }
//            }

//            List<OsmWay> osmHighwaysFulfilledParams = new List<OsmWay>();

//            foreach (var osmHighway in osmHighways)
//            {
//                HighwayTypeEnum highwayType = EnumExtensions.GetTagKeyEnum<HighwayTypeEnum>(osmHighway.Tags[TagKeyEnum.Highway]);
//                if (highwayType != HighwayTypeEnum.None)
//                {
//                    foreach (var type in types)
//                    {
//                        //if (highwayType == HighwayTypeEnum.Proposed)
//                        //{ 
//                        //if (osmHighway.Tags.ContainsKey(TagKeyEnum.Proposed))
//                        //{
//                        //    HighwayTypeEnum proposedType = EnumExtensions.GetTagKeyEnum<HighwayTypeEnum>(osmHighway.Tags[TagKeyEnum.Proposed]);
//                        //    if (proposedType == type)
//                        //    {
//                        //        osmHighway.Tags[TagKeyEnum.Highway] = EnumExtensions.GetEnumAttributeValue(type);
//                        //        osmHighwaysFulfilledParams.Add(osmHighway);
//                        //        break;
//                        //    }

//                        //}
//                        //}
//                        //else if (highwayType == type)
//                        //{
//                        osmHighwaysFulfilledParams.Add(osmHighway);
//                        break;
//                        //}

//                    }

//                }
//            }



//            //ThreadPoolManager threadPoolManager = new ThreadPoolManager(FillWaysNodeThreadPoolCallback);
//            //threadPoolManager.Invoke(osmHighwaysFulfilledParams);
//            FillWaysNode(osmHighwaysFulfilledParams);


//            ThreadPoolManager threadPoolManager = new ThreadPoolManager(HighwayFillResultsThreadPoolCallback);
//            threadPoolManager.Invoke(osmHighwaysFulfilledParams);

//            preparingDataStarted = false;
//            return resultList;
//        }

//        public IEnumerable<IUnityModel> GetBuildings(bool deleteData, params BuildingsTypeEnum[] types)
//        {
//            if (preparingDataStarted)
//            {
//                throw new Exception("preapring data is started in other thread");
//            }
//            else
//            {
//                preparingDataStarted = true;
//            }

//            resultList = new List<IUnityModel>();

//            if (OsmData == null)
//            {
//                return resultList;
//            }
//            buildingsTypes = types;
//            List<OsmWay> osmBuilding = new List<OsmWay>();

//            foreach (var way in OsmData.Ways)
//            {
//                if (way.Value.Tags.ContainsKey(TagKeyEnum.Building))
//                {
//                    osmBuilding.Add(way.Value);
//                }
//            }

//            ThreadPoolManager threadPoolManager = new ThreadPoolManager(FillWaysNodeThreadPoolCallback);
//            threadPoolManager.Invoke(osmBuilding);


//            threadPoolManager = new ThreadPoolManager(BuildingsFillResultsThreadPoolCallback);
//            threadPoolManager.Invoke(osmBuilding);

//            preparingDataStarted = false;
//            return resultList;
//        }

//        public IEnumerable<IUnityModel> GetRailways(bool deleteData)
//        {
//            if (preparingDataStarted)
//            {
//                throw new Exception("preapring data is started in other thread");
//            }
//            else
//            {
//                preparingDataStarted = true;
//            }

//            resultList = new List<IUnityModel>();

//            if (OsmData == null)
//            {
//                return resultList;
//            }

//            List<OsmWay> osmRailways = new List<OsmWay>();

//            foreach (var way in OsmData.Ways)
//            {
//                if (way.Value.Tags.ContainsKey(TagKeyEnum.Railway))
//                {
//                    osmRailways.Add(way.Value);
//                }
//            }

//            if (deleteData)
//            {
//                foreach (var item in osmRailways)
//                {
//                    OsmData.Ways.Remove(item.Id);
//                }
//            }

//            ThreadPoolManager threadPoolManager = new ThreadPoolManager(FillWaysNodeThreadPoolCallback);
//            threadPoolManager.Invoke(osmRailways);


//            threadPoolManager = new ThreadPoolManager(RailwaysFillResultsThreadPoolCallback);
//            threadPoolManager.Invoke(osmRailways);

//            preparingDataStarted = false;
//            return resultList;
//        }

//        public IEnumerable<IUnityModel> GetPowerLines(bool deleteData)
//        {
//            if (preparingDataStarted)
//            {
//                throw new Exception("preapring data is started in other thread");
//            }
//            else
//            {
//                preparingDataStarted = true;
//            }

//            resultList = new List<IUnityModel>();

//            if (OsmData == null)
//            {
//                return resultList;
//            }

//            List<OsmWay> osmPowerLines = new List<OsmWay>();

//            foreach (var way in OsmData.Ways)
//            {
//                if (way.Value.Tags.ContainsKey(TagKeyEnum.Power))
//                {
//                    osmPowerLines.Add(way.Value);
//                }
//            }


//            if (deleteData)
//            {
//                foreach (var item in osmPowerLines)
//                {
//                    OsmData.Ways.Remove(item.Id);
//                }
//            }

//            ThreadPoolManager threadPoolManager = new ThreadPoolManager(FillWaysNodeThreadPoolCallback);
//            threadPoolManager.Invoke(osmPowerLines);


//            threadPoolManager = new ThreadPoolManager(PowerLinesFillResultsThreadPoolCallback);
//            threadPoolManager.Invoke(osmPowerLines);

//            preparingDataStarted = false;
//            return resultList;
//        }

//        public IEnumerable<IUnityModel> GetPowerTowers()
//        {
//            if (preparingDataStarted)
//            {
//                throw new Exception("preapring data is started in other thread");
//            }
//            else
//            {
//                preparingDataStarted = true;
//            }

//            resultList = new List<IUnityModel>();

//            if (OsmData == null)
//            {
//                return resultList;
//            }

//            List<IUnityModel> osmPowerTowers = new List<IUnityModel>();

//            foreach (var node in OsmData.Nodes)
//            {
//                if (node.Value.Tags.ContainsKey(TagKeyEnum.Power))
//                {
//                    PowerTowerTypeEnum powerType = EnumExtensions.GetTagKeyEnum<PowerTowerTypeEnum>(node.Value.Tags[TagKeyEnum.Power]);
//                    if (powerType == PowerTowerTypeEnum.Tower || powerType == PowerTowerTypeEnum.Pole)
//                    {
//                        osmPowerTowers.Add(new UnityPowerTower(node.Value));
//                    }

//                }
//            }
//            return osmPowerTowers;
//        }



//        private void HighwayFillResultsThreadPoolCallback(object threadContext)
//        {
//            IEnumerable<OsmWay> osmWays = threadContext as IEnumerable<OsmWay>;

//            foreach (var osmWay in osmWays)
//            {
//                if (osmWay.Nodes.Count > 0)
//                {
//                    HighwayTypeEnum highwayType = EnumExtensions.GetTagKeyEnum<HighwayTypeEnum>(osmWay.Tags[TagKeyEnum.Highway]);

//                    foreach (var type in highwaysTypes)
//                    {
//                        if (highwayType == type)
//                        {
//                            UnityHighway newUnityway = new UnityHighway(osmWay, type);
//                            if (newUnityway != null)
//                            {
//                                lock (lockResultList)
//                                {
//                                    resultList.Add(newUnityway);
//                                }
//                            }

//                            break;
//                        }

//                    }
//                }
//            }
//        }

//        private void BuildingsFillResultsThreadPoolCallback(object threadContext)
//        {
//            IEnumerable<OsmWay> osmWays = threadContext as IEnumerable<OsmWay>;

//            foreach (var osmWay in osmWays)
//            {
//                if (osmWay.Nodes.Count > 0)
//                {
//                    BuildingsTypeEnum buildingType = EnumExtensions.GetTagKeyEnum<BuildingsTypeEnum>(osmWay.Tags[TagKeyEnum.Building]);

//                    foreach (var type in buildingsTypes)
//                    {
//                        if (buildingType == type)
//                        {
//                            UnityBuilding newUnityway = new UnityBuilding(osmWay, type);
//                            if (newUnityway != null)
//                            {
//                                lock (lockResultList)
//                                {
//                                    resultList.Add(newUnityway);
//                                }
//                            }

//                            break;
//                        }

//                    }
//                }
//            }
//        }

//        private void RailwaysFillResultsThreadPoolCallback(object threadContext)
//        {
//            IEnumerable<OsmWay> osmWays = threadContext as IEnumerable<OsmWay>;

//            foreach (var osmWay in osmWays)
//            {
//                if (osmWay.Nodes.Count > 0)
//                {

//                    UnityRailway newUnityway = new UnityRailway(osmWay);
//                    if (newUnityway != null)
//                    {
//                        lock (lockResultList)
//                        {
//                            resultList.Add(newUnityway);
//                        }
//                    }



//                }
//            }
//        }

//        private void PowerLinesFillResultsThreadPoolCallback(object threadContext)
//        {
//            IEnumerable<OsmWay> osmWays = threadContext as IEnumerable<OsmWay>;

//            foreach (var osmWay in osmWays)
//            {
//                if (osmWay.Nodes.Count > 0)
//                {
//                    PowerLineTypeEnum powerLineType = EnumExtensions.GetTagKeyEnum<PowerLineTypeEnum>(osmWay.Tags[TagKeyEnum.Power]);
//                    UnityPowerLine newUnityway = new UnityPowerLine(osmWay, powerLineType);
//                    if (newUnityway != null)
//                    {
//                        lock (lockResultList)
//                        {
//                            resultList.Add(newUnityway);
//                        }
//                    }



//                }
//            }
//        }










//    }
//}
