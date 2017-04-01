using System;
using System.Collections.Generic;
using System.Text;

namespace OSMtoSharp
{
    public enum RailwayTypeEnum
    {
        [Enum("abandoned")]
        Abandoned,
        [Enum("construction")]
        Construction,
        [Enum("disused")]
        Disused,
        [Enum("funicular")]
        Funicular,
        [Enum("light_rail")]
        Light_rail,
        [Enum("miniature")]
        Miniature,
        [Enum("monorail")]
        Monorail,
        [Enum("narrow_gauge")]
        Narrow_gauge,
        [Enum("preserved")]
        Preserved,
        [Enum("rail")]
        Rail,
        [Enum("subway")]
        Subway,
        [Enum("tram")]
        Tram,
    }
}
