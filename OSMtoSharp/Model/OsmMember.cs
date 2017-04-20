using System;
using System.Collections.Generic;
using System.Text;

namespace OSMtoSharp
{
    public class OsmMember
    {
        public RelationMemberTypeEnum Type { get; set; }
        public long Ref { get; set; }
        public IOsmNode Value { get; set; }
        public RelationMemberRoleEnum Role { get; set; }

    }
}
