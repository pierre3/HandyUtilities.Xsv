using System.IO;
#if net40
using System.Threading.Tasks;
#endif

namespace HandyUtil.Extensions.System.IO
{
    public static class StreamExt
    {
#if net40
        public async static Task<byte[]> ReadBytesAsync(this Stream stream)
        { 
            var buffer = new byte[stream.Length];
            await stream.ReadAsync(buffer,0,(int)stream.Length);
            return buffer;
        }
#endif
    }
}
