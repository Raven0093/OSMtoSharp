using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSMtoSharp;
using System.Xml;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //for (int i = 0; i < 1; i++)
            //{
            //    DateTime data = DateTime.Now;

            //    //Gliwice
            //    //OsmData osmData = OsmParser.GetDataFromOSM(OsmIOManager.LoadOsmDocument(18.27758, 50.28279, 18.69443, 50.30892));

            //    //London
            //    //OsmData osmData = OsmParser.GetDataFromOSM(OsmIOManager.LoadOsmDocument(-0.1353, 51.5019, -0.1016, 51.5132));

            //    //float minLong = 18.5484f;
            //    //float minLat = 50.2426f;
            //    //float maxLong = 18.8179f;
            //    //float maxLat = 50.3354f;

            //    //OsmData osmData = OsmParser.GetDataFromOSM(OsmIOManager.LoadOsmDocument(18.5484, 50.2426f, 18.8179f, 50.3354f));

            DateTime data1 = DateTime.Now;

            float minLong = 18.67758f;
            float minLat = 50.28279f;
            float maxLong = 18.69443f;
            float maxLat = 50.28892f;

            OsmDataManager osmDataManager = new OsmDataManager(minLong, minLat, maxLong, maxLat);
            //OsmDataManager osmDataManager = new OsmDataManager(9.3542, 47.02903, 9.7511, 47.3323);
            var result = osmDataManager.GetHighways(Enum.GetValues(typeof(HighwayTypeEnum)) as HighwayTypeEnum[]);

            //OsmData osmData = OsmParser.GetDataFromOSM(OsmIOManager.LoadOsmDocument(18.5484, 50.2426f, 18.8179f, 50.3354f));

            // Console.WriteLine(result.Count());
            Console.WriteLine(DateTime.Now - data1);

            //    int a = osmData.Ways.Where(x => x.Nodes.Count != x.Nds.Count).Count();
            //    int b = osmData.Relations.Where(x => x.Members.Where(y => y.Value != null).Count() > 0).Count();
            //    Console.WriteLine($"Ways count != Nds count {a}");
            //    Console.WriteLine($"Member != null count {b}");
            //    Console.WriteLine(DateTime.Now - data);
            //    Console.WriteLine($"Nodes count {osmData.Nodes.Count}");
            //    Console.WriteLine($"Relations count {osmData.Relations.Count}");
            //    Console.WriteLine($"Ways count {osmData.Ways.Count}");
            //}

            //OsmIOManager.LoadBigFile(18.5484, 50.2426, 18.8179, 50.3354);


            //DateTime data1 = DateTime.Now;
            ////OsmDataManager osmDataManager = new OsmDataManager(-0.1353, 51.5019, -0.1016, 51.5132);
            //OsmDataManager osmDataManager = new OsmDataManager(9.3542, 47.02903, 9.7511, 47.3323);
            //var result = osmDataManager.GetHighways(Enum.GetValues(typeof(HighwayTypeEnum)) as HighwayTypeEnum[]);
            //Console.WriteLine(DateTime.Now - data1);



            //XmlDocument xml = new XmlDocument();
            //xml.Load("Download\\9.3542_47.02903_9.7511_47.3323.xml");
            //XmlNodeList osmNodes = xml.SelectNodes($"{Constants.osmRoot}/{Constants.osmNode}");
            //foreach (XmlNode item in (osmNodes as System.Collections.IEnumerable).Cast<XmlNode>().Where(x => x.Attributes["id"].Value == "274857079").ToList())
            //{
            //    foreach (XmlAttribute item1 in item.Attributes)
            //    {
            //        Console.WriteLine(item1.Value);
            //    }
            //    Console.WriteLine();
            //    Console.WriteLine();
            //};
            Console.ReadLine();
        }
    }
}
