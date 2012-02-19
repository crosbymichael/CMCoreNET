using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMCoreNET;

namespace CMCoreNET.Builders
{
    public abstract class BuildDirector<Bt, Bm, Bp>
    {
        protected List<Bt> Builders;
        protected Bp Product;
        protected Bm Materials;

        public BuildDirector()
        {
            Builders = new List<Bt>();
        }

        public Bp BuildProduct(Bm materials) {
            this.Materials = materials;
            CreateProductInstance();
            SetupBuilders();
            return Build();
        }

        protected virtual void CreateProductInstance() {
            this.Product = (Bp)typeof(Bp).GetInstance();
        }

        protected void AddBuilder(Bt builder) {
            if (builder != null)
                Builders.Add(builder);
        }

        protected void RemoveBuilderByType(Type builderType) {
            var builder = Builders.FirstOrDefault(b => b.GetType().Name == builderType.Name);
            if (builder == null) return;

            Builders.Remove(builder);
        }

        protected void RemoveAllBuilders() {
            Builders = null;
            Builders = new List<Bt>();
        }

        protected abstract void SetupBuilders();
        protected abstract Bp Build();
    }
}
