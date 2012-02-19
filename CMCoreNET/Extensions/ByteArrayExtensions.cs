using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMCoreNET
{
    public static class ByteArrayExtensions
    {
        public static string GetString(this byte[] byteArrayHelper)
        {
            if (byteArrayHelper == null) return null;
            return Encoding.Default.GetString(byteArrayHelper);
        }
    }
}
