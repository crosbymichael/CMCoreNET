using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using CMCoreNET;

namespace CMCoreNET.Security.Encryption
{
    public class AES : EncryptionBase<AesManaged>, IEncryptionAlgorithm
    {
        #region Fields

        #endregion

        #region Properties

        #endregion

        public AES() {
            this.Type = EncryptionAlgorithm.AES;
        }

        #region Public Methods

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

        #endregion
    }
}
