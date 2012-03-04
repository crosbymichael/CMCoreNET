using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using CMCoreNET;

namespace CMCoreNET.Security.Encryption
{
    public class Rijndael : EncryptionBase<RijndaelManaged>, IEncryptionAlgorithm
    {

        public Rijndael() {
            this.Type = EncryptionAlgorithm.Rijndael;
        }

        public byte[] Encrypt(byte[] dataToEncrypt)
        {
            this.Data = dataToEncrypt;
            this.Encrypt();
            return this.Data;
        }

        public byte[] Decrypt(byte[] encryptedData)
        {
            this.Data = encryptedData;
            this.Decrypt();
            return this.Data;
        }
    }
}
