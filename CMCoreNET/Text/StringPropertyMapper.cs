using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace CMCoreNET.Text
{
    public static class StringPropertyMapper
    {
        public static string ReplaceProperties(string stringToMap, object model) {
            StringBuilder sb = new StringBuilder();
            sb.Append(stringToMap);
            PropertyInfo[] propertyInfo = model.GetType().GetProperties();

            foreach (var property in propertyInfo) {
                string name = ("{" + property.Name + "}");
                if (property.PropertyType == typeof(String)) {
                    if (stringToMap.Contains(name)) {
                        string value = property.GetValue(model, null) as string;
                        sb.Replace(name, value);
                    }
                }
            }
            return sb.ToString();
        }
    }
}
