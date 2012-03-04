using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMCoreNET;

namespace CMCoreNET.Builders
{
    public class BuilderCollection<T> : IDisposable
    {
        private List<T> Builders;

        public BuilderCollection() {
            Builders = new List<T>();
        }

        public T GetBuilder<Bt>() {
            var builder = ( from b in Builders
                            where b.GetType().Name == typeof(Bt).Name
                            select b).FirstOrDefault();

            if (builder == null) {
                builder = (T)typeof(Bt).GetInstance();
                Builders.Add(builder);
            }

            return builder;
        }
    
        #region IDisposable Members

        public void  Dispose()
        {
            Builders.RemoveAll(m => m != null);
            Builders = null;
        }

        #endregion
}
}
