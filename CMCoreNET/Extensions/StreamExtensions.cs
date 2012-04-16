using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CMCoreNET
{
    public static class StreamExtensions
    {
        public static byte[] ToArray(this Stream helper)
        {
            byte[] data = new byte[helper.Length];
            int read = 0;

            while (read > 0)
            {
                read = helper.Read(data, 0, 4096);
            }
            return data;
        }
    }
}
