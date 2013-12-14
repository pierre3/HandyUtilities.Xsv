using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandyUtil.Extensions.System
{
    public static partial class ByteArrayExt
    {
        
        public static string GetString(this byte[] source, Encoding encoding)
        {
            return encoding.GetString(source);
        }

        public static string GetString(this byte[] source)
        {
            return Encoding.Default.GetString(source);
        }

    }
}
