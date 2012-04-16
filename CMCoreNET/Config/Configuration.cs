using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMCoreNET.Config
{
    public class Configuration<T>
    {
        public Configuration()
        {
            if (!typeof(T).IsSerializable)
            {
                throw new InvalidOperationException("T must be serializable");
            }
        }
    }
}
