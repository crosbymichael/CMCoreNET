using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMCoreNET.Serialization;

namespace CMCoreNET
{
    public static class ByteArrayExtensions
    {
        public static string GetString(this byte[] byteArrayHelper)
        {
            if (byteArrayHelper == null) 
                return null;
            return Encoding.Default.GetString(byteArrayHelper);
        }

        public static T JsonTo<T>(this byte[] helper)
        {
            if (helper == null && helper.Length == 0)
            {
                throw new ArgumentNullException();
            }
            return GetObject<T>(helper, SerializationAdapterType.JSON);
        }

        public static T XmlTo<T>(this byte[] helper)
        {
            if (helper == null && helper.Length == 0)
            {
                throw new ArgumentNullException();
            }
            return GetObject<T>(helper, SerializationAdapterType.XML);
        }

        private static T GetObject<T>(byte[] contents, SerializationAdapterType type)
        {
            var adapter = SerializationAdapter.GetAdapter(type);
            return (T)adapter.Deserialize(contents, typeof(T));
        }
    }
}
