﻿using System;
using OSMtoSharp;
using System.Xml;
using System.IO;
using System.Linq;
using OSMtoSharp.Model;
using OSMtoSharp.FileManagers;
using OSMtoSharp.FileManagers.Constants;
using System.Collections.Generic;

namespace TestApp
{

    class Program
    {
        public static string FirstCharToUpper(string input)
        {
            if (String.IsNullOrEmpty(input))
                throw new ArgumentException("ARGH!");
            return input.First().ToString().ToUpper() + String.Join("", input.Skip(1));
        }

        public static string StrongToValueName(string input)
        {
            string result = FirstCharToUpper(input);
            for (int i = 0; i < 5; i++)
            {
                int index = result.IndexOf('-');
                if (index >= 0)
                {
                    result = result.Remove(index) + result.ElementAt(index + 1).ToString().ToUpper() + result.Remove(0, index + 2);
                }
                index = result.IndexOf(':');
                if (index >= 0)
                {
                    result = result.Remove(index) + result.ElementAt(index + 1).ToString().ToUpper() + result.Remove(0, index + 2);
                }
                index = result.IndexOf('_');
                if (index >= 0)
                {
                    result = result.Remove(index) + result.ElementAt(index + 1).ToString().ToUpper() + result.Remove(0, index + 2);
                }
            }



            return result;
        }


        static void Main(string[] args)
        {
            ////string a = "apartments,farm,hotel,house,detached,residential,dormitory,terrace,houseboat,bungalow,static_caravan,commercial,office,industrial,retail,warehouse,bakehouse,cathedral,chapel,church,mosque,temple,synagogue,shrine,civic,hospital,school,stadium,train_station,transportation,university,public,barn,bridge,bunker,cabin,construction,cowshed,digester,farm_auxiliary,garage,garages,greenhouse,hangar,hut,roof,shed,stable,sty,transformer_tower,service,kiosk,carport,ruins,yes,user defined";
            ////string a = "entrance,height,building:levels,building:fireproof,min_level,max_level,non_existent_levels";
            ////string a = "abandoned,construction,disused,funicular,light_rail,miniature,monorail,narrow_gauge,preserved,rail,subway,tram";
            ////using (StreamWriter outputFile = new StreamWriter("WriteLines.txt"))
            ////{
            ////    var b = a.Split(',');
            ////    foreach (string c in b)
            ////    {
            ////        string d = $"[Enum(\"{c}\")]\r\n{FirstCharToUpper(c)},";

            ////        outputFile.WriteLine(d);
            ////    }
            ////}


            ////string fileName = "Download//slaskie-latest.osm";
            ////string fileName = "18.27758_50.28279_18.69443_50.30892.xml";
            ////double minLon = 18.2775800;
            ////double minLat = 50.2827900;
            ////double maxLon = 18.6944300;
            ////double maxLat = 50.3089200;

            //string fileName = "C:\\Users\\Public\\Documents\\Unity Projects\\Tests\\Files\\Gliwice.osm";
            ////double minLon = -100;
            ////double minLat = -100;
            ////double maxLon = 100;
            ////double maxLat = 100;

            ////double minLon = 18.6762000;
            ////double minLat = 50.2862500;
            ////double maxLon = 18.6797600;
            ////double maxLat = 50.2877300;

            //double minLon = -100;
            //double minLat = -100;
            //double maxLon = 100;
            //double maxLat = 100;


            ////DateTime data = DateTime.Now;

            ////OsmData osmData = OsmParser.GetDataFromOSM(fileName, minLon, minLat, maxLon, maxLat);
            ////OsmDataManager osmDataManager = new OsmDataManager(osmData);
            ////var result = osmDataManager.GetHighways(Enum.GetValues(typeof(HighwayTypeEnum)) as HighwayTypeEnum[]);
            ////var result1 = osmDataManager.GetBuildings();
            ////var result2 = osmDataManager.GetRailways();
            ////Console.WriteLine(osmData.Ways.Count);
            ////Console.WriteLine(DateTime.Now - data);


            //OsmData osmData = OsmParser.Parse(fileName, minLon, minLat, maxLon, maxLat);
            ////OsmDataManager osmDataManager = new OsmDataManager(osmData);

            //// var result = osmDataManager.GetPowerLines(false);

            ////OsmXmlWriter.WriteOsmDataToXml(osmData, "map1.osm", 18.6762000, 50.2862500, 18.6797600, 50.2877300);
            ////OsmData osmData1 = OsmParser.GetDataFromOSM("map1.osm", minLon, minLat, maxLon, maxLat);
            ////Console.ReadLine();

            Dictionary<string, string> dic = new Dictionary<string, string>();
            using (StreamWriter sr = new StreamWriter("TestFile.txt"))
            {
                using (XmlReader reader = XmlReader.Create("C:\\Users\\dariu\\OneDrive\\Dokumenty\\Visual Studio 2015\\Projects\\OSMtoSharp\\TestApp\\bin\\Debug\\Files\\map2.osm"))
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            if (reader.NodeType == XmlNodeType.Element)
                            {


                                if (reader.Name == Constants.osmTag)
                                {
                                    try
                                    {
                                        //                                [Helpers.Enum("")]
                                        //None,

                                        string key = reader.GetAttribute(Constants.KeyString);
                                        dic.Add(key, "");
                                        sr.WriteLine($"[Helpers.Enum(\"{key}\")]");
                                        sr.WriteLine($"{StrongToValueName(key)},");

                                    }
                                    catch (Exception ex)
                                    {

#if VERBOSE
                                        
                                    // Console.WriteLine(ex.Message);
#endif
                                    }
                                }


                            }
                        }
                    }
                }
            }
        }
    }
}
