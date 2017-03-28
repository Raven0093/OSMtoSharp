﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Xml;

namespace OSMtoSharp
{
    public class OsmWay : AbstractOsmNode
    {
        public List<OsmNd> Nds { get; set; }
        public List<OsmNode> Nodes { get; set; }

        public OsmWay(XmlAttributeCollection attributes, OsmData parent) : base(attributes, parent)
        {
            Nds = new List<OsmNd>();
            Nodes = new List<OsmNode>();
        }

        public override void FillChildren(XmlNodeList childNodes)
        {
            foreach (XmlNode osmNodeChild in childNodes)
            {
                if (osmNodeChild.Name == "tag")
                {
                    string key = osmNodeChild.Attributes["k"].Value;
                    string value = osmNodeChild.Attributes["v"].Value;
                    TagKeyEnum tagKey = EnumExtensions.GetTagKeyEnum<TagKeyEnum>(key);
                    if (tagKey != TagKeyEnum.None)
                    {
                        Tags[tagKey] = value;
                    }
                }
                else if (osmNodeChild.Name == "nd")
                {
                    Nds.Add(new OsmNd() { Ref = long.Parse(osmNodeChild.Attributes["ref"].Value) });


                }
                else
                {
                    //TODO
                }
            }
        }

        public void FillNodes()
        {
            foreach (var nd in Nds)
            {
                if (parent.Nodes.ContainsKey(nd.Ref))
                {
                    Nodes.Add(parent.Nodes[nd.Ref]);
                }
            }
        }
    }
}
