using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Linq;
using System.Text;

namespace CMCoreNET.Serialization
{
    public enum SerializationAdapterType {
        XML,
        JSON
    }

    public abstract class SerializationAdapter
    {
        public static SerializationAdapter GetAdapter(SerializationAdapterType type) {
            SerializationAdapter adapter = null;
            switch (type) { 
                case SerializationAdapterType.XML:
                    adapter = new XmlSerializer();
                    break;
                case SerializationAdapterType.JSON:
                    adapter = new JsonSerializer();
                    break;
                default:
                    throw new NotSupportedException("Incorrect adapter type");
            }
            return adapter;
        }

        #region Abstract Members

        protected abstract string SerializeData(object data);
        protected abstract object DeserializeData(byte[] data, Type type);

        #endregion


        #region Public Members

        public string Serialize(object data) {
            if (data == null)
                throw new ArgumentNullException("Data cannot be null");
            
            if (!IsSerializable(data.GetType()) && !Attribute.IsDefined(data.GetType(), typeof(DataContractAttribute)))
                throw new NotSupportedException(
                    string.Format("Type {0} is not serializable", data.GetType().Name));
            
            return SerializeData(data);
        }
        
        public object Deserialize(byte[] data, Type type) {
            if (data == null)
                throw new ArgumentNullException("Data cannot be null");

            var dataMember = type.GetCustomAttributesData();

            if (!IsSerializable(type) && !Attribute.IsDefined(type, typeof(DataContractAttribute)))
                throw new NotSupportedException(
                    string.Format("Type {0} is not serializable", type.Name));

            return DeserializeData(data, type);
        }

        public object Deserialize(string data, Type type) {
            return Deserialize(data.GetBytes(), type);
        }

        #endregion


        private bool IsSerializable(Type type) {
            return type.IsSerializable;
        }
    }
}
