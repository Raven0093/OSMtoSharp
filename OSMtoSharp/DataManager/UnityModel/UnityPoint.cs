using System;
using System.Collections.Generic;
using System.Text;

namespace OSMtoSharp
{
    public struct UnityPoint : IUnityModel
    {
        public float Lat { get; set; }
        public float Lon { get; set; }
    }
}
