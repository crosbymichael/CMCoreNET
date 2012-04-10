using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMCoreNET.Mappers
{
    public interface IObjectMapper
    {
        T Map<T>(object obj);
        T Map<T>(object obj, Func<T> constructor);
    }

    public class MapperBucket
    {
        #region Singleton

        public static MapperBucket Instance
        {
            get;
            private set;
        }

        static MapperBucket()
        {
            if (Instance == null)
                Instance = new MapperBucket();
        }

        private MapperBucket()
        {
            this.Mappers = new Dictionary<Type, IObjectMapper>();
        }

        #endregion

        #region Properties

        private Dictionary<Type, IObjectMapper> Mappers;

        #endregion

        #region Public Methods

        public void Add<T>(IObjectMapper mapper)
        {
            this.Mappers.Add(typeof(T), mapper);
        }

        public T MapTo<T>(object data)
        {
            var mapper = this.Mappers[typeof(T)];
            return mapper.Map<T>(data);
        }

        public T MapTo<T>(object data, Func<T> constructor)
        {
            var mapper = this.Mappers[typeof(T)];
            return mapper.Map<T>(data, constructor);
        }

        #endregion
    }
}
