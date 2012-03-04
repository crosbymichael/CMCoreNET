using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using CMCoreNET;

namespace CMCoreNET.Security
{
    public static class Hash
    {
        #region Public Methods

        public static string MD5(string value) {
            return CreateHash(HashFactory
                .Create(HashingAlgorithm.MD5), value);
        }

        public static string SHA1(string value) {
            return CreateHash(HashFactory
                .Create(HashingAlgorithm.SHA1), value);
        }

        public static string SHA256(string value) {
            return CreateHash(HashFactory
                .Create(HashingAlgorithm.SHA256), value);
        }

        #endregion

        #region Private Methods

        private static string CreateHash(
            HashAlgorithm hashingAlgorithm, 
            string value) {
                return HexDigest(
                    hashingAlgorithm.ComputeHash(value.GetBytes()));
        }

        private static string HexDigest(byte[] value) {
            StringBuilder sb = new StringBuilder(value.Length * 2);
            foreach (var b in value) {
                sb.Append(b.ToString("X2"));
            }
            return sb.ToString();
        }

        #endregion
    }
}
