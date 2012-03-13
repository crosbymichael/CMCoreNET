using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMCoreNET.Builders
{
    public abstract class ModelBuildDirector<TBuilder>
    {
        private BuilderCollection<TBuilder> builders;

        public ModelBuildDirector()
        {
            this.builders = new BuilderCollection<TBuilder>();
        }

        protected TBuilder DefaultBuilder {
            get { return this.builders.DefaultBuilder; }
            set { this.builders.DefaultBuilder = value; }
        }

        protected void Add<MType>(TBuilder builder)
        {
            this.builders.Add(typeof(MType), builder);
        }

        protected IEnumerable<TBuilder> GetBuilders(Type type)
        {
            return this.builders.GetBuildersForType(type);
        }
    }
}
