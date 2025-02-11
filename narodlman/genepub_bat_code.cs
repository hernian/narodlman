using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace narodlman
{
    public partial class genepub_bat(string pathZip, BookInfo bookInfo)
    {
        private readonly string _pathZip = pathZip;
        private readonly BookInfo _bookInfo = bookInfo;
        private readonly string _pathNaroZip2EPub = Properties.Settings.Default.PathNaroZip2EPub;
    }
}
