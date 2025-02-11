using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace narodlman
{
    internal class DateTimeUtil
    {
        public static string GetDateTimeString(DateTimeOffset dto)
        {
            return dto.LocalDateTime.ToString("yyyy/MM/dd HH:mm");
        }
    }
}
