using OSMtoSharp.Enums;
using OSMtoSharp.Enums.Helpers;
using OSMtoSharp.Enums.Keys;
using OSMtoSharp.Enums.Values;
using OSMtoSharp.Model;
using OSMtoSharp.Model.Abstract;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Xml;

namespace OSMtoSharp.FileManagers
{

    public class OsmParser
    {
        public static OsmData Parse(string fileName, double minLon, double minLat, double maxLon, double maxLat)
        {
            OsmData osmData = new OsmData();
            AbstractOsmNode.parent = osmData;

            OsmNode currentNode = null;
            OsmWay currentWay = null;
            OsmRelation currentRelation = null;
            bool currentIsNode = false;
            bool currentIsWay = false;
            bool currentIsRelation = false;

            if (!File.Exists(fileName))
            {
                throw new FileNotFoundException();
            }

            using (XmlReader reader = XmlReader.Create(fileName))
            {
                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        if (reader.NodeType == XmlNodeType.Element)
                        {
                            if (reader.Name == Constants.Constants.osmNode)
                            {
                                try
                                {
                                    double lat = double.Parse(reader.GetAttribute(Constants.Constants.LatString), CultureInfo.InvariantCulture);
                                    double lon = double.Parse(reader.GetAttribute(Constants.Constants.LonString), CultureInfo.InvariantCulture);

                                    if (osmData.bounds.MinLat <= lat && osmData.bounds.MaxLat >= lat && osmData.bounds.MinLon <= lon && osmData.bounds.MaxLon >= lon)
                                    {
                                        OsmNode newOsmNode = new OsmNode()
                                        {
                                            Id = long.Parse(reader.GetAttribute(Constants.Constants.IdString)),
                                            Point = new Point()
                                            {
                                                Lat = double.Parse(reader.GetAttribute(Constants.Constants.LatString), CultureInfo.InvariantCulture),
                                                Lon = double.Parse(reader.GetAttribute(Constants.Constants.LonString), CultureInfo.InvariantCulture)
                                            }
                                        };

                                        osmData.Nodes.Add(newOsmNode.Id, newOsmNode);
                                        currentNode = newOsmNode;
                                        currentIsNode = true;
                                        currentIsWay = false; 
                                        currentIsRelation = false;
                                    }
                                    else
                                    {
                                        currentIsNode = false;
                                        currentIsWay = false;
                                        currentIsRelation = false;
                                    }

                                }
                                catch (Exception ex)
                                {
#if VERBOSE
                                    // Console.WriteLine(ex.Message);
#endif
                                }
                            }

                            else if (reader.Name == Constants.Constants.osmWay)
                            {
                                try
                                {
                                    OsmWay newOsmWay = new OsmWay()
                                    {
                                        Id = long.Parse(reader.GetAttribute(Constants.Constants.IdString)),
                                    };

                                    osmData.Ways.Add(newOsmWay.Id, newOsmWay);
                                    currentWay = newOsmWay;
                                    currentIsNode = false;
                                    currentIsWay = true;
                                    currentIsRelation = false;
                                }
                                catch (Exception ex)
                                {
#if VERBOSE
                                    //Console.WriteLine(ex.Message);
#endif
                                }
                            }

                            else if (reader.Name == Constants.Constants.osmRelation)
                            {
                                try
                                {
                                    OsmRelation newOsmRelation = new OsmRelation()
                                    {
                                        Id = long.Parse(reader.GetAttribute(Constants.Constants.IdString))
                                    };

                                    osmData.Relations.Add(newOsmRelation.Id, newOsmRelation);
                                    currentRelation = newOsmRelation;
                                    currentIsNode = false;
                                    currentIsWay = false;
                                    currentIsRelation = true;
                                }
                                catch (Exception ex)
                                {
#if VERBOSE
                                    //Console.WriteLine(ex.Message);
#endif
                                }
                            }

                            else if (reader.Name == Constants.Constants.osmTag)
                            {
                                try
                                {
                                    string key = reader.GetAttribute(Constants.Constants.KeyString);
                                    string value = reader.GetAttribute(Constants.Constants.ValueString);
                                    TagKeyEnum tagKey = EnumExtensions.GetTagKeyEnum<TagKeyEnum>(key);

                                    if (tagKey != TagKeyEnum.None)
                                    {
                                        if (currentIsNode && currentNode != null)
                                        {
                                            currentNode.Tags[tagKey] = value;
                                        }
                                        else if (currentIsWay && currentWay != null)
                                        {
                                            currentWay.Tags[tagKey] = value;
                                        }
                                        else if (currentIsRelation && currentRelation != null)
                                        {
                                            currentRelation.Tags[tagKey] = value;
                                        }

                                    }
                                }
                                catch (Exception ex)
                                {
#if VERBOSE
                                    // Console.WriteLine(ex.Message);
#endif
                                }
                            }

                            else if (reader.Name == Constants.Constants.osmNd)
                            {
                                try
                                {
                                    long refId = long.Parse(reader.GetAttribute(Constants.Constants.refString));

                                    if (currentWay != null && currentIsWay)
                                    {
                                        currentWay.Nds.Add(refId);
                                    }
                                }
                                catch (Exception ex)
                                {
#if VERBOSE
                                    // Console.WriteLine(ex.Message);
#endif
                                }

                            }

                            else if (reader.Name == Constants.Constants.osmMember)
                            {
                                try
                                {
                                    string role = reader.GetAttribute(Constants.Constants.roleString);
                                    RelationMemberRoleEnum roleEnum = EnumExtensions.GetTagKeyEnum<RelationMemberRoleEnum>(role);

                                    string type = reader.GetAttribute(Constants.Constants.typeString);
                                    RelationMemberTypeEnum typeEnum = EnumExtensions.GetTagKeyEnum<RelationMemberTypeEnum>(type);

                                    if (currentIsRelation && currentRelation != null)
                                    {
                                        currentRelation.Members.Add(new OsmMember()
                                        {
                                            Ref = long.Parse(reader.GetAttribute(Constants.Constants.refString)),
                                            Role = roleEnum,
                                            Type = typeEnum
                                        });
                                    }
                                }
                                catch (Exception ex)
                                {
#if VERBOSE
                                    //Console.WriteLine(ex.Message);
#endif
                                }

                            }

                            else if (reader.Name == Constants.Constants.osmBounds)
                            {
                                double fileMinLat;
                                double fileMinLon;
                                double fileMaxLat;
                                double fileMaxLon;

                                try
                                {
                                    fileMinLat = double.Parse(reader.GetAttribute(Constants.Constants.MinLatString), System.Globalization.CultureInfo.InvariantCulture);
                                    fileMinLon = double.Parse(reader.GetAttribute(Constants.Constants.MinLonString), System.Globalization.CultureInfo.InvariantCulture);
                                    fileMaxLat = double.Parse(reader.GetAttribute(Constants.Constants.MaxLatString), System.Globalization.CultureInfo.InvariantCulture);
                                    fileMaxLon = double.Parse(reader.GetAttribute(Constants.Constants.MaxLonString), System.Globalization.CultureInfo.InvariantCulture);
                                }
                                catch (Exception)
                                {
                                    throw new Exception("FIle bad bounds");
                                }


                                osmData.bounds = new OsmBounds();


                                if (fileMaxLat <= minLat && fileMaxLon <= minLon && fileMinLat >= maxLat && fileMinLon >= maxLon)
                                {
                                    break;
                                }

                                if (fileMinLat >= minLat)
                                {
                                    osmData.bounds.MinLat = fileMinLat;
                                }
                                else
                                {
                                    osmData.bounds.MinLat = minLat;
                                }

                                if (fileMinLon >= minLon)
                                {
                                    osmData.bounds.MinLon = fileMinLon;
                                }
                                else
                                {
                                    osmData.bounds.MinLon = minLon;
                                }

                                if (fileMaxLat >= maxLat)
                                {
                                    osmData.bounds.MaxLat = maxLat;
                                }
                                else
                                {
                                    osmData.bounds.MaxLat = fileMaxLat;
                                }

                                if (fileMaxLon >= maxLon)
                                {
                                    osmData.bounds.MaxLon = maxLon;
                                }
                                else
                                {
                                    osmData.bounds.MaxLon = fileMaxLon;
                                }
                            }
                        }
                    }
                }
            }
            return osmData;
        }
    }
}
