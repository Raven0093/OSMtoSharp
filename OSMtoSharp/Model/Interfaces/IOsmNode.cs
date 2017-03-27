using System;
using System.Collections.Generic;
using System.Xml;

namespace OSMtoSharp
{
    public interface IOsmNode
    {
        long Changeset { get; set; }
        long Id { get; set; }
        Dictionary<TagKeyEnum, string> Tags { get; set; }
        DateTime Timestamp { get; set; }
        long Uid { get; set; }
        string User { get; set; }
        int Version { get; set; }
        bool Visible { get; set; }

        void FillChildren(XmlNodeList nodeList);
    }
}