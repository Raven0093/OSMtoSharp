using System;
using System.Collections.Generic;
using System.Text;

namespace OSMtoSharp
{
    public class OsmBounds
    {
        public double MinLat { get; set; }
        public double MinLon { get; set; }
        public double MaxLat { get; set; }
        public double MaxLon { get; set; }

        public OsmBounds(System.Xml.XmlAttributeCollection attributes)
        {
            try
            {
                MinLat = double.Parse(attributes["minlat"].Value, System.Globalization.CultureInfo.InvariantCulture);
                MinLon = double.Parse(attributes["minlon"].Value, System.Globalization.CultureInfo.InvariantCulture);
                MaxLat = double.Parse(attributes["maxlat"].Value, System.Globalization.CultureInfo.InvariantCulture);
                MaxLon = double.Parse(attributes["maxlon"].Value, System.Globalization.CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {

            }
        }
    }
}

