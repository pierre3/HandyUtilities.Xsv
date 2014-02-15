using System.Text;

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
