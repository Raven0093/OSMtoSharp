using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSMtoSharp;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            for (int i = 0; i < 5; i++)
            {
                DateTime data = DateTime.Now;

                //Gliwice
                //OsmData osmData = OsmParser.GetDataFromOSM(OsmIOManager.LoadOsmDocument(18.67758, 50.28279, 18.69443, 50.28892));

                //London
                OsmData osmData = OsmParser.GetDataFromOSM(OsmIOManager.LoadOsmDocument(-0.1353, 51.5019, -0.1016, 51.5132));


                int a = osmData.Ways.Where(x => x.Nodes.Count != x.Nds.Count).Count();
                int b = osmData.Relations.Where(x => x.Members.Where(y => y.Value != null).Count() > 0).Count();
                Console.WriteLine($"Ways count != Nds count {a}");
                Console.WriteLine($"Member != null count {b}");
                Console.WriteLine(DateTime.Now - data);
                Console.WriteLine($"Nodes count {osmData.Nodes.Count}");
                Console.WriteLine($"Relations count {osmData.Relations.Count}");
                Console.WriteLine($"Ways count {osmData.Ways.Count}");
            }

            //OsmIOManager.LoadBigFile(18.5484, 50.2426, 18.8179, 50.3354);


            //DateTime data = DateTime.Now;
            //OsmDataManager osmDataManager = new OsmDataManager(-0.1353, 51.5019, -0.1016, 51.5132);
            //var result = osmDataManager.GetHighways(Enum.GetValues(typeof(HighwayTypeEnum)) as HighwayTypeEnum[]);
            //Console.WriteLine(DateTime.Now - data);

            Console.ReadLine();
        }
    }
}
