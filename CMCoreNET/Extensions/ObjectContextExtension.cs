using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using CMCoreNET.Data;

namespace CMCoreNET
{
    public static class ObjectContextExtension
    {
        public static void RegisterIdentity<D, S>(
            this D dataStore,
            Func<D, ObjectSet<S>> setFunc)
            where D : ObjectContext
            where S : class
        { 
            var objectSet = setFunc(dataStore);
            int initalCount = objectSet.Count();

            SQLCEIdentity.Instance.RegisterInitalIdentity<S>(initalCount);
        }

        public static int GetIdentity<S>(this S objectSet) where S : class
        {
            return SQLCEIdentity.Instance.GetIdentity<S>();
        }
    }
}
