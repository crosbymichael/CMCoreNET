﻿using System;
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
            string json = null;
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(data.GetType());

            using (MemoryStream stream = new MemoryStream())
            {
                serializer.WriteObject(stream, data);
                byte[] binaryJson = stream.ToArray();
                json = binaryJson.GetString();
            }
            return json;
        }

        protected override object DeserializeData(byte[] data, Type type)
        {
            object deserializedObject = null;
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(type);

            using (MemoryStream stream = new MemoryStream(data))
            {
                deserializedObject = serializer.ReadObject(stream);
            }

            return deserializedObject;
        }
    }
}
