using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace narodlman
{
    internal class Episode(int epNum, string title, DateTimeOffset lastModTime, IReadOnlyCollection<string> paras)
    {
        public readonly int EpisodeNumber = epNum;
        public readonly string Title = title;
        public readonly DateTimeOffset LastModifiedTime = lastModTime;
        public readonly IReadOnlyCollection<string> Paragraphs = paras;
    }
}
