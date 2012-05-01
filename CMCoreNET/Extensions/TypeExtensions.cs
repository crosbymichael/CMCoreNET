using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace CMCoreNET
{
    public static class TypeExtensions
    {
        public static object GetInstance(this Type typeHelper) 
        {
            string name = typeHelper.Name;
            if (name == typeof(string).Name || name == typeof(String).Name)
                return string.Empty;
            else if (name == typeof(int).Name)
                return 0;
            else
                return Activator.CreateInstance(typeHelper);
        }

        public static string GetQualifiedName(this Type typeHelper) 
        {
            return string.Format("{0}.{1}", typeHelper.Namespace, typeHelper.Name);
        }

        public static System.Attribute GetAttributeOfType<T>(this Type helper) 
        {
            return (System.Attribute)helper.GetCustomAttributes(typeof(T), false).ToList().FirstOrDefault();
        }

        public static IEnumerable<System.Attribute> GetAttributesOfType<T>(this Type helper)
        {
            var objlist = helper.GetCustomAttributes(typeof(T), false).ToList();
            List<System.Attribute> attrs = new List<Attribute>();

            objlist.ForEach(o => attrs.Add((System.Attribute)o));
            return attrs;
        }

        public static bool ImplementsInterface<T>(this Type typeHelper)
        {
            var iface = typeHelper.GetInterface(string.Format("{0}.{1}", typeof(T).Namespace, typeof(T).Name), true);
            return (iface == null) ? false : true;
        }
    }
}
