using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Xml;

namespace OSMtoSharp
{

    public class OsmParser
    {
        public static OsmData GetDataFromOSM(XmlDocument document)
        {
            OsmData osmData = new OsmData();

            XmlNodeList osmBoundsNodes = document.SelectNodes($"{Constants.osmRoot}/{Constants.osmBounds}");
            if (osmBoundsNodes.Count > 0)
            {
                osmData.bounds = new OsmBounds(osmBoundsNodes[0].Attributes);
            }

            XmlNodeList osmNodes = document.SelectNodes($"{Constants.osmRoot}/{Constants.osmNode}");

            foreach (XmlNode osmNode in osmNodes)
            {
                OsmNode newOsmNode = new OsmNode(osmNode.Attributes, osmData);
                newOsmNode.FillChildren(osmNode.ChildNodes);
                osmData.Nodes.Add(newOsmNode);
            }


            XmlNodeList osmWays = document.SelectNodes($"{Constants.osmRoot}/{Constants.osmWay}");

            foreach (XmlNode osmWay in osmWays)
            {
                OsmWay newOsmWay = new OsmWay(osmWay.Attributes, osmData);
                newOsmWay.FillChildren(osmWay.ChildNodes);
                osmData.Ways.Add(newOsmWay);
            }

            XmlNodeList osmRelations = document.SelectNodes($"{Constants.osmRoot}/{Constants.osmRelation}");

            foreach (XmlNode osmRelation in osmRelations)
            {
                OsmRelation newOsmRelation = new OsmRelation(osmRelation.Attributes, osmData);
                newOsmRelation.FillChildren(osmRelation.ChildNodes);
                osmData.Relations.Add(newOsmRelation);
            }


            while (osmData.jobs > 0)
            {
                Thread.Sleep(200);
            }

            return osmData;

        }
    }
}
