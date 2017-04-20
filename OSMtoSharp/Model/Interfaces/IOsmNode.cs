using System;
using System.Collections.Generic;
using System.Xml;

namespace OSMtoSharp
{
    public interface IOsmNode
    {
        long Id { get; set; }
        Dictionary<TagKeyEnum, string> Tags { get; set; }
    }
}