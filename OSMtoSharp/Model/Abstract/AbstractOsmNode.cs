using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace OSMtoSharp
{
    public abstract class AbstractOsmNode : IOsmNode
    {
        protected OsmData parent;

        public long Id { get; set; }
        public bool Visible { get; set; }
        public int Version { get; set; }
        public long Changeset { get; set; }
        public DateTime Timestamp { get; set; }
        public string User { get; set; }
        public long Uid { get; set; }

        public Dictionary<TagKeyEnum, string> Tags { get; set; }

        protected AbstractOsmNode(System.Xml.XmlAttributeCollection attributes, OsmData parent)
        {
            this.parent = parent;

            Tags = new Dictionary<TagKeyEnum, string>();

            this.Id = long.Parse(attributes["id"].Value);
            this.Visible = bool.Parse(attributes["visible"].Value);
            this.Version = int.Parse(attributes["version"].Value);
            this.Changeset = long.Parse(attributes["changeset"].Value);
            this.Timestamp = DateTime.ParseExact(attributes["timestamp"].Value.Replace("T", " ").Replace("Z", ""), "yyyy-MM-dd HH:mm:ss", null);
            this.User = attributes["user"].Value;
            this.Uid = long.Parse(attributes["changeset"].Value);
        }

        public abstract void FillChildren(XmlNodeList childNodes);
        public abstract void ThreadPoolCallback(Object threadContext);
    }
}
