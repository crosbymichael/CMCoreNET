using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace CMCoreNET
{
    public static class Property
    {
        public static string Name<T>(Expression<Func<T, object>> expression)
        {
            var body = expression.Body as MethodCallExpression;

            return body.ToString();
        }
    }
}
