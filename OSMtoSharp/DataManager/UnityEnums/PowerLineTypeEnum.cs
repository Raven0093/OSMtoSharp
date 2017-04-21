using System;
using System.Collections.Generic;
using System.Text;

namespace OSMtoSharp
{
    public enum PowerLineTypeEnum
    {
        [Enum("line")]
        Line,
        [Enum("minor_line")]
        MinorLine,
    }

    public enum PowerTowerTypeEnum
    {
        [Enum("tower")]
        Tower,
        [Enum("pole")]
        Pole


    }
}
