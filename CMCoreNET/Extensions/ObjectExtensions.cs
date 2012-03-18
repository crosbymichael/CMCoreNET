using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMCoreNET.Serialization;

namespace CMCoreNET
{
    public static class ObjectExtensions
    {
        public static string ToJson(this object helper) {
            Type myType = helper.GetType();
            if (!myType.IsSerializable)
                return null;
 
            var adapter = SerializationAdapter.GetAdapter(SerializationAdapterType.JSON);
            return adapter.Serialize(helper);
        }

        public static string ToXml(this object helper) {
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

        public static T Pop<T>(this T helper) where T : IEnumerable<T>
        {
            return helper.ElementAt(helper.Count());
        }
    }
}
