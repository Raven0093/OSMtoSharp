using System;
using System.Collections.Generic;
using System.Text;

namespace OSMtoSharp
{
    public enum TagKeyEnum
    {
        [Enum("")]
        None,
        [Enum("power")]
        Power,
        [Enum("source")]
        Source,
        [Enum("addr:city")]
        AddrCity,
        [Enum("highway")]
        Highway,
        [Enum("name")]
        Name,
        [Enum("proposed")]
        Proposed,
        [Enum("building")]
        Building,
        [Enum("entrance")]
        Entrance,
        [Enum("height")]
        Height,
        [Enum("building:levels")]
        Building_levels,
        [Enum("building:fireproof")]
        Building_fireproof,
        [Enum("min_level")]
        Min_level,
        [Enum("max_level")]
        Max_level,
        [Enum("non_existent_levels")]
        Non_existent_levels,
        [Enum("railway")]
        Railway,
    }
}
