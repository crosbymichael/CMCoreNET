using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMCoreNET.Serialization;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace CMCoreNET
{
    public static class ObjectExtensions
    {
        public static string ToJson<T>(this T helper) where T : class {
            Type myType = helper.GetType();
            if (!myType.IsSerializable)
                return null;
 
            var adapter = SerializationAdapter.GetAdapter(SerializationAdapterType.JSON);
            return adapter.Serialize(helper);
        }

        public static string ToXml<T>(this T helper) where T : class {
            Type myType = helper.GetType();
            if (!myType.IsSerializable)
                return null;

            var adapter = SerializationAdapter.GetAdapter(SerializationAdapterType.XML);
            return adapter.Serialize(helper);
        }

        public static int GetDynamicHash(this object helper) {
            int hash = 0;

            var properties = helper.GetType().GetProperties().ToList();
            foreach (var pro in properties)
            {
                var value = pro.GetValue(helper, null);
                if (value == null)
                {
                    continue;
                }
                hash = hash ^ value.GetHashCode();
            }
            return hash;
        }

        public static string DynamicToString(this object helper)
        {
            var sb = new StringBuilder();

            helper.GetType().GetProperties(
                BindingFlags.Public | 
                BindingFlags.GetProperty)
                .ToList().ForEach(p => 
            {
                sb.AppendLine(p.GetValue(helper, null).ToString());
            });
            return sb.ToString();
        }

        public static T Pop<T>(this T helper) where T : IEnumerable<T>
        {
            return helper.ElementAt(helper.Count());
        }

        public static bool IsValid<T>(this T helper) where T : class
        { 
            bool isValid = true;

            try
            {
                Validator.ValidateObject(
                    helper, 
                    new ValidationContext(helper, null, null),
                    true);
            }
            catch 
            { 
                isValid = false; 
            }
            
            return isValid;
        }
    }
}
