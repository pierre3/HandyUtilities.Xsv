using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace HandyUtil.Extensions.System.IO
{
    public static class StreamExt
    {
        public async static Task<byte[]> ReadBytesAsync(this Stream stream)
        { 
            var buffer = new byte[stream.Length];
            await stream.ReadAsync(buffer,0,(int)stream.Length);
            return buffer;
        }
    }
}
