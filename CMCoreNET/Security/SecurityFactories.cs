using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMCoreNET;
using System.Security.Cryptography;
using System.Net.NetworkInformation;

namespace CMCoreNET.Security
{
    #region Enums

    public enum HashingAlgorithm {
        MD5,
        SHA1,
        SHA256,
        SHA512
    }

    public enum EncryptionAlgorithm {
        AES,
        Rijndael
    }

    #endregion

    #region Factories

    public static class HashFactory {
        
        internal static HashAlgorithm Create(HashingAlgorithm algoType) {
            HashAlgorithm hashingInstance = null;
            switch (algoType) {
                case HashingAlgorithm.MD5:
                    hashingInstance = MD5.Create();
                    break;
                case HashingAlgorithm.SHA1:
                    hashingInstance = SHA1Managed.Create();
                    break;
                case HashingAlgorithm.SHA256:
                    hashingInstance = SHA256Managed.Create();
                    break;
                case HashingAlgorithm.SHA512:
                    hashingInstance = SHA512Managed.Create();
                    break;
                default:
                    throw new Exception("Incorrect use of HashingAlgorithm factory");
            }
            return hashingInstance;
        }
    }

    public static class EncryptionFactory {
        
        public static IEncryptionAlgorithm Create(EncryptionAlgorithm algoType) {
            IEncryptionAlgorithm encryptionInstance = null;
            switch (algoType) { 
                case EncryptionAlgorithm.AES:
                    encryptionInstance = new Encryption.AES();
                    break;
                case EncryptionAlgorithm.Rijndael:
                    encryptionInstance = new Encryption.Rijndael();
                    break;
                default:
                    throw new Exception("Incorrect use of EncryptionAlgoritm factory");
            }
            return encryptionInstance;
        }
    }

    internal static class SeedFactory {

        public static string CreateSeed() {

            string macAddress = (from iface in NetworkInterface.GetAllNetworkInterfaces()
                                 where iface.OperationalStatus == OperationalStatus.Up
                                 select iface).FirstOrDefault().GetPhysicalAddress().ToString();
            return macAddress;
        }
    }

    #endregion
}
