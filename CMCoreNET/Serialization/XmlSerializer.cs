﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CMCoreNET.Serialization
{
    public class XmlSerializer : SerializationAdapter
    {
        protected override string SerializeData(object data)
        {
            string xml = null;
            System.Xml.Serialization.XmlSerializer serializer =
                new System.Xml.Serialization.XmlSerializer(data.GetType());

            using (MemoryStream stream = new MemoryStream())
            {
                serializer.Serialize(stream, data);
                byte[] binaryXml = stream.ToArray();
                xml = binaryXml.GetString();
            }

            return xml;
        }

        protected override object DeserializeData(byte[] data, Type type)
        {
            object dataObject = null;
            System.Xml.Serialization.XmlSerializer serializer =
                new System.Xml.Serialization.XmlSerializer(type);

            using (MemoryStream stream = new MemoryStream(data))
            {
                dataObject = serializer.Deserialize(stream);
            }

            return dataObject;
        }
    }
}
