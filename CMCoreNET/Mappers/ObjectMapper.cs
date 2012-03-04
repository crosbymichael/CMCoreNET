using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMCoreNET;
using System.Reflection;

namespace CMCoreNET.Mappers
{
    public static class ObjectMapper
    {
        public static object MapDataToType(object fromObject, Type toType)
        {
            var output = toType.GetInstance();
            List<PropertyInfo> fromProperties = fromObject.GetType().GetProperties().ToList();
            List<PropertyInfo> toProperties = toType.GetProperties().ToList();

            var matchingProperties =
                from f in fromProperties
                join t in toProperties
                on f.Name equals t.Name
                where f.PropertyType == t.PropertyType
                select new { Name = f.Name, Value = f.GetValue(fromObject, null) };

            foreach (var match in matchingProperties)
            {
                toType.GetProperty(match.Name).SetValue(output, match.Value, null);
            }

            return output;
        }
    }
}
