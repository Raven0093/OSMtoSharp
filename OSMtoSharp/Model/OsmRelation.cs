using OSMtoSharp.Enums;
using OSMtoSharp.Enums.Values;
using OSMtoSharp.Model.Abstract;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Xml;

namespace OSMtoSharp.Model
{
    public class OsmRelation : AbstractOsmNode
    {
        public List<OsmMember> Members { get; set; }

        public OsmRelation() { Members = new List<OsmMember>(); }

        public void FillMembers()
        {
            foreach (var member in Members)
            {
                if (member.Type == RelationMemberTypeEnum.Node)
                {
                    if (parent.Nodes.ContainsKey(member.Ref))
                    {
                        member.Value = parent.Nodes[member.Ref];
                    }
                }
                else if (member.Type == RelationMemberTypeEnum.Way)
                {

                    if (parent.Ways.ContainsKey(member.Ref))
                    {
                        member.Value = parent.Ways[member.Ref];
                    }

                }

            }
        }
    }
}
