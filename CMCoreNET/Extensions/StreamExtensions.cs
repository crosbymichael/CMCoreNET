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

        public static void CopyStream(this Stream input, Stream output)
        {
            byte[] buffer = new byte[8 * 1024];
            int len;
            while ((len = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, len);
            }
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
