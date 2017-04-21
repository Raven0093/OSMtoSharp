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
        BuildingLevels,
        [Enum("building:fireproof")]
        Building_fireproof,
        [Enum("minLevel")]
        Min_level,
        [Enum("maxLevel")]
        Max_level,
        [Enum("non_existent_levels")]
        NonExistentLevels,
        [Enum("railway")]
        Railway,
        [Enum("construction")]
        Construction,

    }
}
