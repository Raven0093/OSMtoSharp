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
    }
}
