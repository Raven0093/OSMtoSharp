using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Xml;

namespace OSMtoSharp
{
    public class OsmIOManager
    {
        private static long jobs;

        private static object jobsLock;

        private static List<XmlDocument> XmlDocuments;

        private static string GenerateFileUrl(double minLong, double minLat, double maxLong, double maxLat)
        {
            return string.Format("http://api.openstreetmap.org/api/0.6/map?bbox={0},{1},{2},{3}",
                minLong.ToString("0.#######", System.Globalization.CultureInfo.InvariantCulture),
                minLat.ToString("0.#######", System.Globalization.CultureInfo.InvariantCulture),
                maxLong.ToString("0.#######", System.Globalization.CultureInfo.InvariantCulture),
                maxLat.ToString("0.#######", System.Globalization.CultureInfo.InvariantCulture));
        }

        private static string GenerateFileName(double minLong, double minLat, double maxLong, double maxLat)
        {
            return string.Format("{0}{1}{2}_{3}_{4}_{5}.xml",
                Constants.DownloadFolder,
                Path.DirectorySeparatorChar,
                minLong.ToString("0.#######", System.Globalization.CultureInfo.InvariantCulture),
                minLat.ToString("0.#######", System.Globalization.CultureInfo.InvariantCulture),
                maxLong.ToString("0.#######", System.Globalization.CultureInfo.InvariantCulture),
                maxLat.ToString("0.#######", System.Globalization.CultureInfo.InvariantCulture));
        }

        private static XmlDocument LoadFile(double minLong, double minLat, double maxLong, double maxLat)
        {
            string url = GenerateFileUrl(minLong, minLat, maxLong, maxLat);

            string fileName = GenerateFileName(minLong, minLat, maxLong, maxLat);

            if (!Directory.Exists(Constants.DownloadFolder))
            {
                Directory.CreateDirectory(Constants.DownloadFolder);
            }
            if (!File.Exists(fileName))
            {
                try
                {
                    WebClient Client = new WebClient();
                    Client.DownloadFile(url, fileName);
                }
                catch (Exception ex)
                {
                    throw new DownloadFileException(ex.Message);
                }

            }

            XmlDocument document = new XmlDocument();
            document.Load(fileName);

            return document;
        }

        private static List<FileData> GenerateFileDataList(double minLong, double minLat, double maxLong, double maxLat, double lonLatShift)
        {
            List<FileData> files = new List<FileData>();

            double newMinLong = minLong;
            double newMinLat = minLat;
            double newMaxLong = 0;
            double newMaxLat = 0;
            bool endLat = false;
            bool endLon = false;
            newMaxLong = minLong + lonLatShift;
            if (newMaxLong > maxLong)
            {
                newMaxLong = maxLong;
                endLon = true;

            }

            newMaxLat = minLat + lonLatShift;
            if (newMaxLat > maxLat)
            {
                newMaxLat = maxLat;
                endLat = true;
            }

            while (!(endLat && endLon))
            {
                files.Add(new FileData()
                {
                    MaxLat = newMaxLat,
                    MaxLong = newMaxLong,
                    MinLat = newMinLat,
                    MinLong = newMinLong,
                    Name = GenerateFileName(newMinLong, newMinLat, newMaxLong, newMaxLat),
                    Url = GenerateFileUrl(newMinLong, newMinLat, newMaxLong, newMaxLat)
                });

                if (!endLat)
                {
                    newMinLat = newMaxLat + Constants.latLonBoundsShift;
                    newMaxLat = newMaxLat + lonLatShift;
                    if (newMaxLat >= maxLat)
                    {
                        newMaxLat = maxLat;
                        endLat = true;
                    }


                }
                else
                {
                    endLat = false;
                    newMinLat = minLat;
                    newMaxLat = minLat + lonLatShift;
                    if (newMaxLat >= maxLat)
                    {
                        newMaxLat = maxLat;
                        endLat = true;
                    }
                    newMinLong = newMaxLong + Constants.latLonBoundsShift;
                    newMaxLong = newMaxLong + lonLatShift;
                    if (newMaxLong >= maxLong)
                    {
                        newMaxLong = maxLong;
                        endLon = true;
                    }


                }

            }
            files.Add(new FileData()
            {
                MaxLat = newMaxLat,
                MaxLong = newMaxLong,
                MinLat = newMinLat,
                MinLong = newMinLong,
                Name = GenerateFileName(newMinLong, newMinLat, newMaxLong, newMaxLat),
                Url = GenerateFileUrl(newMinLong, newMinLat, newMaxLong, newMaxLat)
            });

            return files;
        }

        private static XmlDocument CreateBigFile(double minLong, double minLat, double maxLong, double maxLat, double lonLatShift)
        {
            XmlDocuments = new List<XmlDocument>();
            jobsLock = new object();
            jobs = 0;
            List<FileData> files = GenerateFileDataList(minLong, minLat, maxLong, maxLat, lonLatShift);

            foreach (var file in files)
            {
                lock (jobsLock)
                {
                    jobs++;
                }
                ThreadPool.QueueUserWorkItem(ThreadPoolCallback, file);
            }

            while (jobs > 0)
            {
                Thread.Sleep(200);
            }

            if (XmlDocuments.Count > 1)
            {
                XmlNode osmMainRoot = XmlDocuments[0].SelectNodes($"{Constants.osmRoot}")[0];
                for (int i = 1; i < XmlDocuments.Count; i++)
                {

                    XmlNodeList osmNodeNodes = XmlDocuments[i].SelectNodes($"{Constants.osmRoot}/{Constants.osmNode}");
                    XmlNodeList osmWayNodes = XmlDocuments[i].SelectNodes($"{Constants.osmRoot}/{Constants.osmWay}");
                    XmlNodeList osmRelationNodes = XmlDocuments[i].SelectNodes($"{Constants.osmRoot}/{Constants.osmRelation}");
                    XmlNodeList[] lists = new XmlNodeList[] { osmNodeNodes, osmWayNodes, osmRelationNodes };
                    foreach (XmlNodeList list in lists)
                    {
                        foreach (XmlNode node in list)
                        {
                            osmMainRoot.AppendChild(osmMainRoot.OwnerDocument.ImportNode(node, true));
                        }
                    }
                }

                XmlNode osmBoundsNodes = XmlDocuments[0].SelectNodes($"{Constants.osmRoot}/{Constants.osmBounds}")[0];
                osmBoundsNodes.Attributes["minlon"].Value = minLong.ToString("0.0000000", System.Globalization.CultureInfo.InvariantCulture);
                osmBoundsNodes.Attributes["minlat"].Value = minLat.ToString("0.0000000", System.Globalization.CultureInfo.InvariantCulture);
                osmBoundsNodes.Attributes["maxlon"].Value = maxLong.ToString("0.0000000", System.Globalization.CultureInfo.InvariantCulture);
                osmBoundsNodes.Attributes["maxlat"].Value = maxLat.ToString("0.0000000", System.Globalization.CultureInfo.InvariantCulture);
            }

            XmlDocuments[0].Save(GenerateFileName(minLong, minLat, maxLong, maxLat));

            return XmlDocuments[0];
        }

        private static void ThreadPoolCallback(object threadContext)
        {

            FileData file = threadContext as FileData;
            if (file != null)
            {
                if (!Directory.Exists(Constants.DownloadFolder))
                {
                    Directory.CreateDirectory(Constants.DownloadFolder);
                }
                if (!File.Exists(file.Name))
                {
                    WebClient Client = new WebClient();
                    int i = 0;
                    while (true)
                    {
                        try
                        {
                            Client.DownloadFile(file.Url, file.Name);
                            break;
                        }
                        catch (Exception)
                        {

                            i++;
                            if (i > Constants.maxTryDownloadDocument)
                            {
                                break;
                            }

                        }
                    }

                }
            }

            XmlDocument document = new XmlDocument();
            document.Load(file.Name);

            lock (jobsLock)
            {
                XmlDocuments.Add(document);
                jobs--;
            }
        }

        public static XmlDocument LoadOsmDocument(double minLong, double minLat, double maxLong, double maxLat)
        {
            string fileName = GenerateFileName(minLong, minLat, maxLong, maxLat);

            if (!File.Exists(fileName) && ((maxLong - minLong) > Constants.latLonDivisionShift || (maxLat - minLat) > Constants.latLonDivisionShift))
            {
                CreateBigFile(minLong, minLat, maxLong, maxLat, Constants.latLonDivisionShift);
                return LoadFile(minLong, minLat, maxLong, maxLat);
            }
            else
            {
                return LoadFile(minLong, minLat, maxLong, maxLat);
            }
        }
    }
}

