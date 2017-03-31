using System;
using OSMtoSharp;
using System.Xml;
using System.IO;

namespace TestApp
{

    class Program
    {
        static void Main(string[] args)
        {
            //string fileName = "Download//slaskie-latest.osm";
            //string fileName = "18.27758_50.28279_18.69443_50.30892.xml";
            //double minLon = 18.2775800;
            //double minLat = 50.2827900;
            //double maxLon = 18.6944300;
            //double maxLat = 50.3089200;

            string fileName = "map.osm";
            double minLon = -100;
            double minLat = -100;
            double maxLon = 100;
            double maxLat = 100;

            DateTime data = DateTime.Now;

            OsmData osmData = OsmParser.GetDataFromOSM(fileName, minLon, minLat, maxLon, maxLat);
            OsmDataManager osmDataManager = new OsmDataManager(osmData);
            var result = osmDataManager.GetHighways(Enum.GetValues(typeof(HighwayTypeEnum)) as HighwayTypeEnum[]);
            var result1 = osmDataManager.GetBuilding();
            Console.WriteLine(osmData.Ways.Count);
            Console.WriteLine(DateTime.Now - data);


            Console.ReadLine();
        }
    }
}
