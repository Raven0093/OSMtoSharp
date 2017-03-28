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

#if VERBOSE
            Console.WriteLine($"BigFile split to {files.Count} files");
#endif

            return files;
        }

        private static XmlDocument CreateBigFile(double minLong, double minLat, double maxLong, double maxLat, double lonLatShift)
        {
            Dictionary<string, bool> nodesDictionary = new Dictionary<string, bool>();
            Dictionary<string, bool> waysDictionary = new Dictionary<string, bool>();
            Dictionary<string, bool> relationsDictionary = new Dictionary<string, bool>();

            List<FileData> files = GenerateFileDataList(minLong, minLat, maxLong, maxLat, lonLatShift);

            ThreadPoolManager threadPoolManager = new ThreadPoolManager(ThreadPoolCallback);
            threadPoolManager.Invoke(files);

            XmlDocument XmlResultDocument = new XmlDocument();
            XmlDocument XmlCurrentDocuments;
            if (File.Exists(files[0].Name))
            {
                XmlResultDocument.Load(files[0].Name);
            }
            else
            {
                throw new Exception("Pusta lista!!!!@@@@###@@!!");
            }

            XmlNode osmMainRoot = XmlResultDocument.SelectNodes($"{Constants.osmRoot}")[0];
            XmlNodeList osmRootNodeNodes = XmlResultDocument.SelectNodes($"{Constants.osmRoot}/{Constants.osmNode}");
            XmlNodeList osmRootWayNodes = XmlResultDocument.SelectNodes($"{Constants.osmRoot}/{Constants.osmWay}");
            XmlNodeList osmRootRelationNodes = XmlResultDocument.SelectNodes($"{Constants.osmRoot}/{Constants.osmRelation}");

            foreach (XmlNode osmRootNodeNode in osmRootNodeNodes)
            {
                nodesDictionary[osmRootNodeNode.Attributes["id"].Value] = true;
            }
            foreach (XmlNode osmRootWayNode in osmRootWayNodes)
            {
                nodesDictionary[osmRootWayNode.Attributes["id"].Value] = true;
            }
            foreach (XmlNode osmRootRelationNode in osmRootRelationNodes)
            {
                nodesDictionary[osmRootRelationNode.Attributes["id"].Value] = true;
            }


            for (int i = 1; i < files.Count; i++)
            {

                if (File.Exists(files[i].Name))
                {
                    XmlCurrentDocuments = new XmlDocument();
                    XmlCurrentDocuments.Load(files[i].Name);

                    XmlNodeList osmNodeNodes = XmlCurrentDocuments.SelectNodes($"{Constants.osmRoot}/{Constants.osmNode}");
                    XmlNodeList osmWayNodes = XmlCurrentDocuments.SelectNodes($"{Constants.osmRoot}/{Constants.osmWay}");
                    XmlNodeList osmRelationNodes = XmlCurrentDocuments.SelectNodes($"{Constants.osmRoot}/{Constants.osmRelation}");

                    foreach (XmlNode osmNodeNode in osmNodeNodes)
                    {
                        string id = osmNodeNode.Attributes["id"].Value;
                        if (!nodesDictionary.ContainsKey(id))
                        {
                            nodesDictionary[id] = true;
                            osmMainRoot.AppendChild(osmMainRoot.OwnerDocument.ImportNode(osmNodeNode, true));
                        }
                    }

                    foreach (XmlNode osmWayNode in osmWayNodes)
                    {
                        string id = osmWayNode.Attributes["id"].Value;
                        if (!nodesDictionary.ContainsKey(id))
                        {
                            nodesDictionary[id] = true;
                            osmMainRoot.AppendChild(osmMainRoot.OwnerDocument.ImportNode(osmWayNode, true));
                        }
                    }

                    foreach (XmlNode osmRelationNode in osmRelationNodes)
                    {
                        string id = osmRelationNode.Attributes["id"].Value;
                        if (!nodesDictionary.ContainsKey(id))
                        {
                            nodesDictionary[id] = true;
                            osmMainRoot.AppendChild(osmMainRoot.OwnerDocument.ImportNode(osmRelationNode, true));
                        }
                    }
                }
            }

            XmlNode osmBoundsNodes = XmlResultDocument.SelectNodes($"{Constants.osmRoot}/{Constants.osmBounds}")[0];
            osmBoundsNodes.Attributes["minlon"].Value = minLong.ToString("0.0000000", System.Globalization.CultureInfo.InvariantCulture);
            osmBoundsNodes.Attributes["minlat"].Value = minLat.ToString("0.0000000", System.Globalization.CultureInfo.InvariantCulture);
            osmBoundsNodes.Attributes["maxlon"].Value = maxLong.ToString("0.0000000", System.Globalization.CultureInfo.InvariantCulture);
            osmBoundsNodes.Attributes["maxlat"].Value = maxLat.ToString("0.0000000", System.Globalization.CultureInfo.InvariantCulture);

            XmlResultDocument.Save(GenerateFileName(minLong, minLat, maxLong, maxLat));

            return XmlResultDocument;
        }

        private static void ThreadPoolCallback(object threadContext)
        {

            IEnumerable<FileData> files = threadContext as IEnumerable<FileData>;
            if (!Directory.Exists(Constants.DownloadFolder))
            {
                Directory.CreateDirectory(Constants.DownloadFolder);
            }
            foreach (var file in files)
            {
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
                                return;
                            }
                            Thread.Sleep(Constants.waitDowloadMilisec);
#if VERBOSE
                            Console.WriteLine($"{file.Name} - retry #{i}");
#endif
                        }
                    }
                }
            }
        }

        public static XmlDocument LoadOsmDocument(double minLong, double minLat, double maxLong, double maxLat)
        {
            string fileName = GenerateFileName(minLong, minLat, maxLong, maxLat);

            if (!File.Exists(fileName) && ((maxLong - minLong) > Constants.latLonDivisionShift || (maxLat - minLat) > Constants.latLonDivisionShift))
            {
                return CreateBigFile(minLong, minLat, maxLong, maxLat, Constants.latLonDivisionShift);
            }
            else
            {
                return LoadFile(minLong, minLat, maxLong, maxLat);
            }
        }
    }
}

