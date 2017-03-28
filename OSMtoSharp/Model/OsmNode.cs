using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Xml;

namespace OSMtoSharp
{
    public class OsmNode : AbstractOsmNode
    {
        public OsmPoint Point { get; set; }

        public OsmNode(System.Xml.XmlAttributeCollection attributes, OsmData parent) : base(attributes, parent)
        {
            Point = new OsmPoint()
            {
                Lat = double.Parse(attributes["lat"].Value, CultureInfo.InvariantCulture),
                Lon = double.Parse(attributes["lon"].Value, CultureInfo.InvariantCulture)
            };



        }

        public override void FillChildren(XmlNodeList childNodes)
        {
            foreach (XmlNode osmNodeChild in childNodes)
            {
                try
                {
                    string key = osmNodeChild.Attributes["k"].Value;
                    string value = osmNodeChild.Attributes["v"].Value;
                    TagKeyEnum tagKey = EnumExtensions.GetTagKeyEnum<TagKeyEnum>(key);
                    if (tagKey != TagKeyEnum.None)
                    {
                        Tags[tagKey] = value;
                    }
                }
                catch (Exception)
                {

                    //TODO
                }

            }
        }
    }
}
