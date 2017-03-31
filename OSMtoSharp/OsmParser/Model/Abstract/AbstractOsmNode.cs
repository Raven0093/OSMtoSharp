using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace OSMtoSharp
{
    public abstract class AbstractOsmNode : IOsmNode
    {
        public static OsmData parent;

        public long Id { get; set; }



        public Dictionary<TagKeyEnum, string> Tags { get; set; }

        protected AbstractOsmNode(System.Xml.XmlAttributeCollection attributes)
        {
            Tags = new Dictionary<TagKeyEnum, string>();

            this.Id = long.Parse(attributes["id"].Value);

        }

        protected AbstractOsmNode()
        {
            Tags = new Dictionary<TagKeyEnum, string>();
        }
    }
}
