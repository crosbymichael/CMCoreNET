using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using CMCoreNET.Net;

namespace CMCoreNET
{
    public static class StringExtensions
    {
        public static byte[] GetBytes(this string stringHelper)
        {
            if (string.IsNullOrEmpty(stringHelper)) return null;
            return Encoding.Default.GetBytes(stringHelper);
        }

        public static string InitWithFile(this string stringHelper, string filePath) {
            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentNullException("File path cannot be null or empty");
            if (!File.Exists(filePath)) return string.Empty;

            using (TextReader reader = new StreamReader(filePath)) {
                return reader.ReadToEnd();
            }
        }

        public static string InitWithUrl(this string stringHelper, string url) {
            if (string.IsNullOrEmpty(url))
                throw new ArgumentNullException("Url cannot be null or empty");

            UrlRequest request = new UrlRequest(url);
            var response = request.Load();
            using (TextReader reader = new StreamReader(response.GetResponseStream())) {
                return reader.ReadToEnd();
            }
        }

        public static string ReplaceWithProperties(this string stringHelper, object property) {
            if (property == null) return null;
            StringBuilder sb = new StringBuilder();
            sb.Append(stringHelper);
            PropertyInfo[] pi = property.GetType().GetProperties();

            foreach (var p in pi) {
                string name = ("{" + p.Name + "}");
                if (p.PropertyType == typeof(String)) {
                    if (stringHelper.Contains(name)) {
                        string value = p.GetValue(property, null) as string;
                        sb.Replace(name, value);
                    }
                }
            }

            return sb.ToString();
        }
    }
}
