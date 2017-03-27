using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Xml;

namespace OSMtoSharp
{
    public class OsmRelation : AbstractOsmNode
    {
        public List<OsmMember> Members { get; set; }

        public OsmRelation(XmlAttributeCollection attributes, OsmData parent) : base(attributes, parent)
        {
            Members = new List<OsmMember>();
        }

        public override void FillChildren(XmlNodeList childNodes)
        {
            foreach (XmlNode osmNodeChild in childNodes)
            {
                if (osmNodeChild.Name == "tag")
                {
                    string key = osmNodeChild.Attributes["k"].Value;
                    string value = osmNodeChild.Attributes["v"].Value;
                    TagKeyEnum tagKey = EnumExtensions.GetTagKeyEnum<TagKeyEnum>(key);
                    if (tagKey != TagKeyEnum.None)
                    {
                        Tags[tagKey] = value;
                    }
                }
                else if (osmNodeChild.Name == "member")
                {
                    string role = osmNodeChild.Attributes["role"].Value;
                    RelationMemberRoleEnum roleEnum = EnumExtensions.GetTagKeyEnum<RelationMemberRoleEnum>(role);

                    string type = osmNodeChild.Attributes["type"].Value;
                    RelationMemberTypeEnum typeEnum = EnumExtensions.GetTagKeyEnum<RelationMemberTypeEnum>(type);

                    Members.Add(new OsmMember()
                    {
                        Ref = long.Parse(osmNodeChild.Attributes["ref"].Value),
                        Role = roleEnum,
                        Type = typeEnum
                    });


                }
                else
                {
                    //TODO
                }
            }
            ThreadPool.QueueUserWorkItem(ThreadPoolCallback);
        }

        public override void ThreadPoolCallback(object threadContext)
        {
            lock (parent.jobsLock)
            {
                parent.jobs++;
            }

            foreach (var member in Members)
            {
                if (member.Type == RelationMemberTypeEnum.Node)
                {
                    foreach (var item in parent.Nodes)
                    {
                        if (item.Id == member.Ref)
                        {
                            member.Value = item as OsmNode;
                            break;
                        }
                    }
                }
                else if (member.Type == RelationMemberTypeEnum.Way)
                {
                    foreach (var item in parent.Ways)
                    {
                        if (item.Id == member.Ref)
                        {
                            member.Value = item as OsmWay;
                            break;
                        }
                    }
                }

            }
            lock (parent.jobsLock)
            {
                parent.jobs--;
            }
        }
    }
}
