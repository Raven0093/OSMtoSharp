using OSMtoSharp.Enums;
using OSMtoSharp.Enums.Keys;
using System;
using System.Collections.Generic;
using System.Xml;

namespace OSMtoSharp.Model.Interfaces
{
    public interface IOsmNode
    {
        long Id { get; set; }
        Dictionary<TagKeyEnum, string> Tags { get; set; }
    }
}