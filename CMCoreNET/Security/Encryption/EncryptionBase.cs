using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using CMCoreNET.Security;
using CMCoreNET;

namespace CMCoreNET.Security.Encryption
{
    public enum CrytorType {
        Encrypt,
        Decrypt
    }

    public abstract class EncryptionBase<T> : IDisposable where T : System.Security.Cryptography.SymmetricAlgorithm
    {
        T Algorithm { get; set; }
        ICryptoTransform cryptor;

        protected string _key;
        protected string _iv;
        protected byte[] Data { get; set; }

        public EncryptionAlgorithm Type { get; protected set; }

        public string Key { set { this._key = value; } }

        public string Vector { set { this._iv = value; } }

        protected void Encrypt() {
            Setup(CrytorType.Encrypt);
            EncryptData();
        }

        protected void Decrypt() {
            Setup(CrytorType.Decrypt);
            DecryptData();
        }

        void Setup(CrytorType type) {
            ValidateProperties();
            if (this.Algorithm == null)
                SetupAlgorithm();
            SetupCryptor(type);
        }

        void SetupAlgorithm() {
            CreateAlgorithm();
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(this._key.GetBytes(), this._key.GetBytes());
            this.Algorithm.Key = pdb.GetBytes(this.Algorithm.KeySize / 8);
            pdb.Dispose();
            pdb = null;
            
            pdb = new PasswordDeriveBytes(this._iv.GetBytes(), this._iv.GetBytes());
            this.Algorithm.IV = pdb.GetBytes(this.Algorithm.BlockSize / 8);
            pdb.Dispose();
        }

        void SetupCryptor(CrytorType type) {
            switch (type) {
                case CrytorType.Encrypt:
                    this.cryptor = this.Algorithm.CreateEncryptor(this.Algorithm.Key, this.Algorithm.IV);
                    break;
                case CrytorType.Decrypt:
                    this.cryptor = this.Algorithm.CreateDecryptor(this.Algorithm.Key, this.Algorithm.IV);
                    break;
                default:
                    throw new Exception("Incorrect cryptor type");
            }   
        }

        void ValidateProperties() {
            if (Data == null)
                throw new NullReferenceException("Data to encrypt is null");
            if (string.IsNullOrEmpty(_key))
                throw new NullReferenceException("The encryption key is null or empty");
            if (string.IsNullOrEmpty(_iv))
                throw new NullReferenceException("the encryption vector is null or empty");
        }

        protected virtual void CreateAlgorithm() {
            this.Algorithm = (T)typeof(T).GetInstance();
        }

        void EncryptData() {
            using (MemoryStream dataStream = new MemoryStream()) { 
                using (CryptoStream cryptoStream = new CryptoStream(dataStream, this.cryptor, CryptoStreamMode.Write)) {
                    cryptoStream.Write(this.Data, 0, this.Data.Length);
                    cryptoStream.Close();
                    this.Data = dataStream.ToArray();
                }
            }
        }

        void EncryptData(Stream dataStream)
        {
            using (CryptoStream cryptoStream = new CryptoStream(dataStream, this.cryptor, CryptoStreamMode.Write))
            {
                cryptoStream.Write(this.Data, 0, this.Data.Length);
                cryptoStream.Close();
            }
        }

        void DecryptData() {
            using (MemoryStream dataStream = new MemoryStream()) {
                using (CryptoStream cryptoStream = new CryptoStream(dataStream, this.cryptor, CryptoStreamMode.Write)) {
                    cryptoStream.Write(this.Data, 0, this.Data.Length);
                    cryptoStream.Close();
                    this.Data = dataStream.ToArray();
                }
            }
        }

        public void Dispose()
        {
            if (this.Algorithm != null)
                this.Algorithm.Dispose();
            if (this.cryptor != null)
                this.cryptor.Dispose();
        }
    }
}
