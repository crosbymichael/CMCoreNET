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
            return GetBytes(helper, (int)helper.Length);
        }

        public static byte[] ToArray(this Stream helper, int length)
        {
            return GetBytes(helper, length);
        }

        static byte[] GetBytes(Stream stream, int length)
        {
            byte[] data = new byte[length];
            int read = 0;

            while (read > 0)
            {
                read = stream.Read(data, 0, 4096);
            }
            return data;
        }
    }
}
