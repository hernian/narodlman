using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace narodlman
{
    internal class EpisodeInfo
    {
        public int EpisodeNumber { get; set; }
        public string Title { get; set; } = string.Empty;

        public string Url { get; set; } = string.Empty;
        public DateTimeOffset LastModifiedTime { get; set; }
    }
}
