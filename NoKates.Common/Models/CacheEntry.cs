using System;

namespace NoKates.Common.Models
{
    internal class CacheEntry
    {
        public DateTime Expires { get; set; }
        public string Name { get; set; }
        public string Params { get; set; }
        public string Result { get; set; }
    }
}
