using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using CMCoreNET;
using System.IO;

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

        public static string MD5(byte[] value)
        {
            return CreateHash(
                HashFactory.Create(HashingAlgorithm.MD5),
                value);
        }

        public static string SHA1(byte[] value)
        {
            return CreateHash(
                HashFactory.Create(HashingAlgorithm.SHA1),
                value);
        }

        public static string SHA256(byte[] value)
        {
            return CreateHash(
                HashFactory.Create(HashingAlgorithm.SHA256),
                value);
        }

        public static string MD5(Stream value)
        {
            return CreateHash(
                HashFactory.Create(HashingAlgorithm.MD5),
                value);
        }

        public static string SHA1(Stream value)
        {
            return CreateHash(
                HashFactory.Create(HashingAlgorithm.SHA1),
                value);
        }

        public static string SHA256(Stream value)
        {
            return CreateHash(
                HashFactory.Create(HashingAlgorithm.SHA256),
                value);
        }

        #endregion

        #region Private Methods

        private static string CreateHash(
            HashAlgorithm algo,
            byte[] value)
        {
            return HexDigest(
                    algo.ComputeHash(value));
        }

        private static string CreateHash(
            HashAlgorithm algo,
            Stream value)
        {
            return HexDigest(
                    algo.ComputeHash(value));
        }

        private static string CreateHash(
            HashAlgorithm algo, 
            string value) 
        {
            return CreateHash(algo, value.GetBytes());
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
