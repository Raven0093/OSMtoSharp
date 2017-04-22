using OSMtoSharp.Enums;
using OSMtoSharp.Enums.Helpers;
using OSMtoSharp.Enums.Keys;
using OSMtoSharp.Enums.Values;
using OSMtoSharp.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;

namespace OSMtoSharp.FileManagers
{
    public class OsmXmlWriter
    {
        private static void WriteBounds(XmlWriter writer, OsmBounds osmBounds)
        {
            writer.WriteStartElement(Constants.Constants.osmBounds);
            writer.WriteAttributeString(Constants.Constants.MinLatString, osmBounds.MinLat.ToString(CultureInfo.InvariantCulture));
            writer.WriteAttributeString(Constants.Constants.MinLonString, osmBounds.MinLon.ToString(CultureInfo.InvariantCulture));
            writer.WriteAttributeString(Constants.Constants.MaxLatString, osmBounds.MaxLat.ToString(CultureInfo.InvariantCulture));
            writer.WriteAttributeString(Constants.Constants.MaxLonString, osmBounds.MaxLon.ToString(CultureInfo.InvariantCulture));
            writer.WriteEndElement();
        }

        private static void WriteTag(XmlWriter writer, string key, string value)
        {
            writer.WriteStartElement(Constants.Constants.osmTag);
            writer.WriteAttributeString(Constants.Constants.KeyString, key);
            writer.WriteAttributeString(Constants.Constants.ValueString, value);
            writer.WriteEndElement();
        }

        private static void WriteTags(XmlWriter writer, Dictionary<TagKeyEnum, string> tags)
        {
            foreach (var tag in tags)
            {
                WriteTag(writer, EnumExtensions.GetEnumAttributeValue(tag.Key), EnumExtensions.GetEnumAttributeValue(tag.Value));
            }



        }

        private static void WriteNode(XmlWriter writer, OsmNode osmNode)
        {
            writer.WriteStartElement(Constants.Constants.osmNode);
            writer.WriteAttributeString(Constants.Constants.IdString, osmNode.Id.ToString(CultureInfo.InvariantCulture));
            writer.WriteAttributeString(Constants.Constants.LatString, osmNode.Point.Lat.ToString(CultureInfo.InvariantCulture));
            writer.WriteAttributeString(Constants.Constants.LonString, osmNode.Point.Lon.ToString(CultureInfo.InvariantCulture));
            WriteTags(writer, osmNode.Tags);
            writer.WriteEndElement();
        }

        private static void WriteNodes(XmlWriter writer, Dictionary<long, OsmNode> nodes)
        {
            foreach (var node in nodes)
            {
                WriteNode(writer, node.Value);
            }
        }

        private static void WriteWay(XmlWriter writer, OsmWay osmWay)
        {
            writer.WriteStartElement(Constants.Constants.osmWay);

            writer.WriteAttributeString(Constants.Constants.IdString, osmWay.Id.ToString(CultureInfo.InvariantCulture));

            WriteNds(writer, osmWay.Nds);
            WriteTags(writer, osmWay.Tags);

            writer.WriteEndElement();
        }

        private static void WriteNds(XmlWriter writer, List<long> nds)
        {
            foreach (var nd in nds)
            {
                WriteNd(writer, nd);
            }
        }

        private static void WriteNd(XmlWriter writer, long id)
        {
            writer.WriteStartElement(Constants.Constants.osmNd);
            writer.WriteAttributeString(Constants.Constants.refString, id.ToString());
            writer.WriteEndElement();
        }

        private static void WriteWays(XmlWriter writer, Dictionary<long, OsmWay> ways)
        {
            foreach (var way in ways)
            {
                WriteWay(writer, way.Value);
            }
        }

        private static void WriteRelations(XmlWriter writer, Dictionary<long, OsmRelation> relations)
        {
            foreach (var relation in relations)
            {
                WriteRelation(writer, relation.Value);
            }
        }

        private static void WriteRelation(XmlWriter writer, OsmRelation osmRelation)
        {
            writer.WriteStartElement(Constants.Constants.osmRelation);

            writer.WriteAttributeString(Constants.Constants.IdString, osmRelation.Id.ToString(CultureInfo.InvariantCulture));

            WriteMembers(writer, osmRelation.Members);
            WriteTags(writer, osmRelation.Tags);

            writer.WriteEndElement();
        }

        private static void WriteMembers(XmlWriter writer, List<OsmMember> members)
        {
            foreach (var member in members)
            {
                WriteMember(writer, member);
            }
        }

        private static void WriteMember(XmlWriter writer, OsmMember member)
        {
            writer.WriteStartElement(Constants.Constants.osmNd);
            writer.WriteAttributeString(Constants.Constants.typeString, EnumExtensions.GetEnumAttributeValue(member.Type));
            writer.WriteAttributeString(Constants.Constants.refString, member.Ref.ToString());
            if (member.Role == RelationMemberRoleEnum.None)
            {
                writer.WriteAttributeString(Constants.Constants.roleString, "");
            }
            else
            {
                writer.WriteAttributeString(Constants.Constants.roleString, member.Role.ToString());
            }

            writer.WriteEndElement();
        }

        public static bool WriteOsmDataToXml(OsmData osmData, string outputFileName, double minLon, double minLat, double maxLon, double maxLat)
        {
            if (!Directory.Exists(Constants.Constants.FileFolder))
            {
                Directory.CreateDirectory(Constants.Constants.FileFolder);
            }
            if (osmData == null)
            {
                return false;
            }

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;


            using (XmlWriter outputWriter = XmlWriter.Create($"{Constants.Constants.FileFolder}{Path.DirectorySeparatorChar}{outputFileName}", settings))
            {
                outputWriter.WriteStartElement("osm");

                WriteBounds(outputWriter, osmData.bounds);
                WriteNodes(outputWriter, osmData.Nodes);
                WriteWays(outputWriter, osmData.Ways);
                WriteRelations(outputWriter, osmData.Relations);
                outputWriter.WriteEndElement();

            }


            return true;
        }




    }
}
