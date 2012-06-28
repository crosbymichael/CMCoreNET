using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using CMCoreNET.Net;
using CMCoreNET.Serialization;

namespace CMCoreNET
{
    public static class StringExtensions
    {
        public static byte[] GetBytes(this string stringHelper)
        {
            if (string.IsNullOrEmpty(stringHelper)) 
                return null;

            return Encoding.Default.GetBytes(stringHelper);
        }

        public static string InitWithFile(this string stringHelper, string filePath) 
        {
            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentNullException("File path cannot be null or empty");
           
            if (!File.Exists(filePath)) return string.Empty;

            using (TextReader reader = new StreamReader(filePath))
            {
                return reader.ReadToEnd();
            }
        }

        public static string InitWithUrl(this string stringHelper, string url) 
        {
            if (string.IsNullOrEmpty(url))
                throw new ArgumentNullException("Url cannot be null or empty");

            UrlRequest request = new UrlRequest(url);
            var response = request.Load();
            using (TextReader reader = new StreamReader(response.GetResponseStream())) 
            {
                return reader.ReadToEnd();
            }
        }

        public static string ReplaceWithProperties(this string stringHelper, object property) 
        {
            if (property == null) return null;
            string buffer = stringHelper;

            PropertyInfo[] pi = property.GetType().GetProperties();

            foreach (var p in pi)
            {
                string name = ("{" + p.Name + "}");
                if (p.PropertyType == typeof(String)) 
                {
                    if (buffer.Contains(name))
                    {
                        string value = p.GetValue(property, null) as string;
                        buffer = buffer.Replace(name, value);
                    }
                }
            }

            return buffer;
        }

        public static T JsonTo<T>(this string helper)
        {
            if (string.IsNullOrEmpty(helper))
            {
                throw new ArgumentNullException();
            }
            return GetObject<T>(helper, SerializationAdapterType.JSON);
        }

        public static T XmlTo<T>(this string helper)
        {
            if (string.IsNullOrEmpty(helper))
            {
                throw new ArgumentNullException();
            }
            return GetObject<T>(helper, SerializationAdapterType.XML);
        }

        public static T AsEnum<T>(this string helper)
        {
            if (string.IsNullOrEmpty(helper))
            {
                return default(T);
            }
            return (T)Enum.Parse(typeof(T), helper, true);
        }

        private static T GetObject<T>(string contents, SerializationAdapterType type)
        {
            var adapter = SerializationAdapter.GetAdapter(type);
            return (T)adapter.Deserialize(contents, typeof(T));
        }

        public static void ForEachLine(this string helper, Action<string> action)
        {
            using (StringReader reader = new StringReader(helper))
            {
                string line = null;
                while ((line = reader.ReadLine()) != null)
                {
                    action(line);
                }
            }
        }

        public static string FromBase64(this string helper)
        {
            return Convert
                .FromBase64String(helper)
                .GetString();
        }

        public static string ToBase64(this string helper)
        {
            return Convert
                .ToBase64String(helper.GetBytes());
        }
    }
}
