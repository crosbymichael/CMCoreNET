using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace CMCoreNET.Extensions
{
    public static class TypeExtensions
    {
        public static T GetInstance<T>(this Type typeHelper) {
            return Activator.CreateInstance<T>();
        }
    }
}
