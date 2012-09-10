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

        public static T GetAttributeOfType<T>(this Type helper) where T : Attribute
        {
            return helper.GetAttributesOfType<T>()
                .FirstOrDefault();
        }

        public static IEnumerable<T> GetAttributesOfType<T>(this Type helper) where T : Attribute
        {
            return helper.GetCustomAttributes(typeof(T), false)
                .Cast<T>();
        }

        public static bool ImplementsInterface<T>(this Type typeHelper)
        {
            var iface = typeHelper.GetInterface(string.Format("{0}.{1}", typeof(T).Namespace, typeof(T).Name), true);
            return (iface == null) ? false : true;
        }
    }
}
