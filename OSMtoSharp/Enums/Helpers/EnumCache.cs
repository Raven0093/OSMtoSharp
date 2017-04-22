using System;
using System.Collections.Generic;
using System.Text;

namespace OSMtoSharp.Enums.Helpers
{
    public interface IEnumCache
    {
        Dictionary<string, object> keys { get; set; }
    }
    public class EnumCache<T> : IEnumCache
    {
        public Dictionary<string, T> keys;
        public EnumCache()
        {
            keys = new Dictionary<string, T>();
        }

        Dictionary<string, object> IEnumCache.keys
        {
            get
            {
                Dictionary<string, object> result = new Dictionary<string, object>();
                foreach (var item in keys)
                {
                    result[item.Key] = item.Value as object;
                }
                return result;
            }

            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
