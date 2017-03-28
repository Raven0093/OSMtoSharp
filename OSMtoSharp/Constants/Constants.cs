using System;
using System.Collections.Generic;
using System.Text;

namespace OSMtoSharp
{
    public class Constants
    {
        public const string osmRoot = "osm";
        public const string osmBounds = "bounds";
        public const string osmNode = "node";
        public const string osmWay = "way";
        public const string osmRelation = "relation";
        public const double latLonDivisionShift = 0.015;
        public const double latLonBoundsShift = 0.0000001;
        public const int maxTryDownloadDocument = 60;
        public const string DownloadFolder = "Download";
        public const int waitDowloadMilisec = 1000;

    }
}
