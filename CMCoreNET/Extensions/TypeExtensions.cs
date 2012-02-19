using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace CMCoreNET
{
    public static class TypeExtensions
    {
        public static object GetInstance(this Type typeHelper) {
            return Activator.CreateInstance(typeHelper);
        }

        public static string GetQualifiedName(this Type typeHelper) {
            return string.Format("{0}.{1}", typeHelper.Namespace, typeHelper.Name);
        }
    }
}
