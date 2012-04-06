using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Json;

namespace CMCoreNET.Serialization
{
    public class JsonSerializer : SerializationAdapter
    {
        protected override string SerializeData(object data)
        {
            string json;
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(data.GetType());
            
            using (MemoryStream stream = new MemoryStream())
            {
                serializer.WriteObject(stream, data);
                byte[] binaryJson = stream.ToArray();
                json = binaryJson.GetString();
            }
            return json;
        }

        protected override object DeserializeData(Stream data, Type type)
        {
            object deserializedObject = null;
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(type);
            
            data.Position = 0;
            deserializedObject = serializer.ReadObject(data);
          
            return deserializedObject;
        }
    }
}
