using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMCoreNET.Data
{
    public class SQLCEIdentity
    {
         #region Singleton

        static SQLCEIdentity instance;

        static SQLCEIdentity()
        {
            if (instance == null)
                instance = new SQLCEIdentity();
        }

        public static SQLCEIdentity Instance
        {
            get { return instance; }
        }

        #endregion

        #region Instance Methods

        private Dictionary<Type, int> identitySets;

        private SQLCEIdentity()
        {
            identitySets = new Dictionary<Type, int>();
        }

        internal void RegisterInitalIdentity<S>(int initalIdentity)
        {
            lock (this)
            {
                this.identitySets.Add(typeof(S), initalIdentity);
            }
        }

        internal int GetIdentity<S>()
        {
            lock (this)
            {
                int identity;
                var type = typeof(S);

                if (this.identitySets.TryGetValue(type, out identity))
                {
                    identity = identity + 1;
                    this.identitySets[type] = identity;
                    return identity;
                }
                throw new ArgumentNullException("S", "Object set is not registered");
            }
        }

        #endregion
    }
}
