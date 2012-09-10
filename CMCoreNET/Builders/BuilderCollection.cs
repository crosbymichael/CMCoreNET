using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMCoreNET;

namespace CMCoreNET.Builders
{
    public class BuilderCollection<T> : IDisposable
    {
        Dictionary<string, List<T>> builders;
        T defaultBuilder;

        public BuilderCollection() {
            builders = new Dictionary<string, List<T>>();
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
            return this.builders[type.Name];
        }

        public void Add(Type modelType, T builder)
        {
            List<T> builderList = null;
            if (builders.Keys.Count > 0)
            {
                builderList = this.builders
                    .Where(m => m.Key == modelType.Name)
                    .FirstOrDefault()
                    .Value;
            }

            if (builderList == null)
            {
                List<T> list = new List<T>();
                list.Add(this.defaultBuilder);
                list.Add(builder);
                this.builders[modelType.Name] = list;
                return;
            }

            if (!ContainsBuilder(builderList, builder))
            {
                builderList.Add(builder);
            }
        }

        bool ContainsBuilder(IEnumerable<T> list, T builder)
        {
            return list.Any(b => b.GetType().Name == builder.GetType().Name);
        }
    
        #region IDisposable Members

        public void  Dispose()
        {
            foreach (string key in this.builders.Keys)
            {
                this.builders.Remove(key);
            }
            builders = null;
        }

        #endregion
    }
}
