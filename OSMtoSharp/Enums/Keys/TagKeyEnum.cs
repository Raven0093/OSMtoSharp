using System;
using System.Collections.Generic;
using System.Text;

namespace OSMtoSharp.Enums.Keys
{
    public enum TagKeyEnum
    {
        [Helpers.Enum("")]
        None,
        [Helpers.Enum("power")]
        Power,
        [Helpers.Enum("source")]
        Source,
        [Helpers.Enum("addr:city")]
        AddrCity,
        [Helpers.Enum("highway")]
        Highway,
        [Helpers.Enum("name")]
        Name,
        [Helpers.Enum("proposed")]
        Proposed,
        [Helpers.Enum("building")]
        Building,
        [Helpers.Enum("entrance")]
        Entrance,
        [Helpers.Enum("height")]
        Height,
        [Helpers.Enum("building:levels")]
        BuildingLevels,
        [Helpers.Enum("building:fireproof")]
        Building_fireproof,
        [Helpers.Enum("minLevel")]
        Min_level,
        [Helpers.Enum("maxLevel")]
        Max_level,
        [Helpers.Enum("non_existent_levels")]
        NonExistentLevels,
        [Helpers.Enum("railway")]
        Railway,
        [Helpers.Enum("construction")]
        Construction,
        [Helpers.Enum("min_height")]
        MinHeight,
        [Helpers.Enum("landuse")]
        Landuse,
        [Helpers.Enum("leisure")]
        Leisure,
        [Helpers.Enum("amenity")]
        Amenity,
    }
}
