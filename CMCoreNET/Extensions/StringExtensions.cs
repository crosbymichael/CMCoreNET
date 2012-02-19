using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace CMCoreNET
{
    public static class StringExtensions
    {
        public static byte[] GetBytes(this string stringHelper)
        {
            if (string.IsNullOrEmpty(stringHelper)) return null;
            return Encoding.Default.GetBytes(stringHelper);
        }

        public static string ReplaceWithProperties(this string stringHelper, object property) {
            StringBuilder sb = new StringBuilder();
            sb.Append(stringHelper);
            PropertyInfo[] pi = property.GetType().GetProperties();

            foreach (var p in pi)
            {
                string name = ("{" + p.Name + "}");
                if (p.PropertyType == typeof(String))
                {
                    if (stringHelper.Contains(name))
                    {
                        string value = p.GetValue(property, null) as string;
                        sb.Replace(name, value);
                    }
                }
            }

            return sb.ToString();
        }
    }
}
