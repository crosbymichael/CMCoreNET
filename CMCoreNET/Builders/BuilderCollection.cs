using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMCoreNET;

namespace CMCoreNET.Builders
{
    public class BuilderCollection<T> : IDisposable
    {
        private Dictionary<Type, List<T>> builders;
        private T defaultBuilder;

        public BuilderCollection() {
            builders = new Dictionary<Type, List<T>>();
        }

        public T DefaultBuilder
        {
            get { return this.defaultBuilder; }
            set { this.defaultBuilder = value; }
        }

        /// <summary>
        /// Get builders for a specified model type.
        /// Default builders are included.
        /// </summary>
        /// <typeparam name="MType"></typeparam>
        /// <returns></returns>
        public IEnumerable<T> GetBuildersForType(Type type) {
            return this.builders[type];
        }

        public void Add(Type modelType, T builder)
        {
            var builderList = this.builders[modelType];
            if (builderList == null)
            {
                List<T> list = new List<T>();
                list.Add(this.defaultBuilder);
                list.Add(builder);
                this.builders[modelType] = list;
                return;
            }

            if (!ContainsBuilder(builderList, builder))
            {
                builderList.Add(builder);
            }
        }

        private bool ContainsBuilder(IEnumerable<T> list, T builder)
        {
            return list.Any(b => b.GetType().Name == builder.GetType().Name);
        }
    
        #region IDisposable Members

        public void  Dispose()
        {
            foreach (Type key in this.builders.Keys)
            {
                this.builders.Remove(key);
            }
            builders = null;
        }

        #endregion
    }
}
