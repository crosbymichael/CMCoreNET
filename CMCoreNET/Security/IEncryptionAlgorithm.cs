using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CMCoreNET.Security
{
    public interface IEncryptionAlgorithm
    {
        EncryptionAlgorithm Type { get; }
        string Key { set; }
        string Vector { set; }

        byte[] Encrypt(byte[] dataToEncrypt);
        byte[] Decrypt(byte[] encryptedData);
        //Stream Encrypt(Stream dataStream);
        //Stream Decrypt(Stream dataStream);
    }
}
