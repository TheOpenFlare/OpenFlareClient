// <copyright file="Security.cs" company="POQDavid">
//     Copyright (c) James Tuley. All rights reserved.
//     Copyright (c) POQDavid. All rights reserved.
// </copyright>
// <author>James Tuley</author>
// <author>POQDavid</author>
// <summary>
// This is the Security class.
// This work (Modern Encryption of a String C#, by James Tuley),
// identified by James Tuley, is free of known copyright restrictions.
// https://gist.github.com/4336842
// http://creativecommons.org/publicdomain/mark/1.0/
// Note that this source is modified a bit by poqdavid
// </summary>
namespace OpenFlareClient
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Security;
    using System.Text;
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Crypto.Digests;
    using Org.BouncyCastle.Crypto.Engines;
    using Org.BouncyCastle.Crypto.Generators;
    using Org.BouncyCastle.Crypto.Modes;
    using Org.BouncyCastle.Crypto.Parameters;
    using Org.BouncyCastle.Security;

    /// <summary>
    /// Description of Security.
    /// </summary>
    internal class Security
    {
        /// <summary>
        /// Description of AES
        /// </summary>
        public static class AES
        {
            /// <summary>
            /// Iterations for encryption and decryption
            /// </summary>
            public static readonly int Iterations = 10000;

            /// <summary>
            /// KeyBitSize for encryption and decryption
            /// </summary>
            public static readonly int KeyBitSize = 256;

            /// <summary>
            /// MacBitSize for encryption and decryption
            /// </summary>
            public static readonly int MacBitSize = 128;

            /// <summary>
            /// MinPasswordLength for encryption and decryption
            /// </summary>
            public static readonly int MinPasswordLength = 12;

            /// <summary>
            /// NonceBitSize for encryption and decryption
            /// </summary>
            public static readonly int NonceBitSize = 128;

            /// <summary>
            /// SaltBitSize for encryption and decryption
            /// </summary>
            public static readonly int SaltBitSize = 128;

            /// <summary>
            /// SecureRandom for encryption
            /// </summary>
            private static readonly SecureRandom Random = new SecureRandom();

            /// <summary>
            /// To decrypt, encrypted SecureString
            /// </summary>
            /// <param name="encryptedMessage">SecureString to decrypt</param>
            /// <param name="password">Password for encrypted SecureString</param>
            /// <param name="nonSecretPayloadLength">nonSecretPayloadLength for decryption</param>
            /// <returns>Returns decrypt SecureString</returns>
            public static SecureString Decrypt(SecureString encryptedMessage, SecureString password = null, int nonSecretPayloadLength = 0)
            {
                if (encryptedMessage.IsNullOrEmpty())
                {
                    return string.Empty.SecureString();
                }

                if (encryptedMessage.IsNullOrWhiteSpace())
                {
                    throw new ArgumentException("Encrypted Message Required!", "encryptedMessage");
                }

                var cipherText = Convert.FromBase64String(encryptedMessage.UnsecureString());
                if (password == null)
                {
                    password = Security.Generator.HWID;
                }

                var plainText = Decrypt(cipherText, password, nonSecretPayloadLength);

                cipherText = null;

                return plainText == null ? null : Encoding.UTF8.GetString(plainText).SecureString();
            }

            /// <summary>
            /// To decrypt, encrypted data
            /// </summary>
            /// <param name="encryptedMessage">Data to decrypt</param>
            /// <param name="password">Password for encrypted data</param>
            /// <param name="nonSecretPayloadLength">nonSecretPayloadLength for decryption</param>
            /// <returns>Returns decrypt data</returns>
            public static byte[] Decrypt(byte[] encryptedMessage, SecureString password = null, int nonSecretPayloadLength = 0)
            {
                if (password == null)
                {
                    password = Security.Generator.HWID;
                }

                ////User Error Checks
                if (password.IsNullOrWhiteSpace() || password.UnsecureString().Length < MinPasswordLength)
                {
                    throw new ArgumentException(string.Format("Must have a password of at least {0} characters!", MinPasswordLength), "password");
                }

                if (encryptedMessage == null || encryptedMessage.Length == 0)
                {
                    throw new ArgumentException("Encrypted Message Required!", "encryptedMessage");
                }

                var generator = new Pkcs5S2ParametersGenerator();

                ////Grab Salt from Payload
                var salt = new byte[SaltBitSize / 8];
                Array.Copy(encryptedMessage, nonSecretPayloadLength, salt, 0, salt.Length);

                generator.Init(PbeParametersGenerator.Pkcs5PasswordToBytes(password.UnsecureString().ToCharArray()), salt, Iterations);

                ////Generate Key
                var key = (KeyParameter)generator.GenerateDerivedMacParameters(KeyBitSize);

                return Decrypt(encryptedMessage, key.GetKey(), salt.Length + nonSecretPayloadLength);
            }

            /// <summary>
            /// To decrypt, encrypted data
            /// </summary>
            /// <param name="encryptedMessage">Data to decrypt</param>
            /// <param name="key">Key for encrypted data</param>
            /// <param name="nonSecretPayloadLength">nonSecretPayloadLength for decryption</param>
            /// <returns>Returns decrypt data</returns>
            public static byte[] Decrypt(byte[] encryptedMessage, byte[] key, int nonSecretPayloadLength = 0)
            {
                ////User Error Checks
                if (key == null || key.Length != KeyBitSize / 8)
                {
                    throw new ArgumentException(string.Format("Key needs to be {0} bit!", KeyBitSize), "key");
                }

                if (encryptedMessage == null || encryptedMessage.Length == 0)
                {
                    throw new ArgumentException("Encrypted Message Required!", "encryptedMessage");
                }

                using (var cipherStream = new MemoryStream(encryptedMessage))
                using (var cipherReader = new BinaryReader(cipherStream))
                {
                    ////Grab Payload
                    var nonSecretPayload = cipherReader.ReadBytes(nonSecretPayloadLength);

                    ////Grab Nonce
                    var nonce = cipherReader.ReadBytes(NonceBitSize / 8);

                    var cipher = new GcmBlockCipher(new AesFastEngine());
                    var parameters = new AeadParameters(new KeyParameter(key), MacBitSize, nonce, nonSecretPayload);
                    cipher.Init(false, parameters);

                    ////Decrypt Cipher Text
                    var cipherText = cipherReader.ReadBytes(encryptedMessage.Length - nonSecretPayloadLength - nonce.Length);
                    var plainText = new byte[cipher.GetOutputSize(cipherText.Length)];

                    try
                    {
                        var len = cipher.ProcessBytes(cipherText, 0, cipherText.Length, plainText, 0);
                        cipher.DoFinal(plainText, len);
                    }
                    catch (InvalidCipherTextException)
                    {
                        ////Return null if it doesn't authenticate
                        return null;
                    }

                    return plainText;
                }
            }

            /// <summary>
            /// To encrypt, plain data
            /// </summary>
            /// <param name="secretMessage">SecureString to encrypt</param>
            /// <param name="password">Password for encrypting</param>
            /// <param name="nonSecretPayload">nonSecretPayload for encryption</param>
            /// <returns>Returns encrypted data</returns>
            public static SecureString Encrypt(SecureString secretMessage, SecureString password = null, byte[] nonSecretPayload = null)
            {
                if (secretMessage.IsNullOrEmpty())
                {
                    return string.Empty.SecureString();
                }

                var plainText = Encoding.UTF8.GetBytes(secretMessage.UnsecureString());
                if (password == null)
                {
                    password = Security.Generator.HWID;
                }

                var cipherText = Encrypt(plainText, password, nonSecretPayload);

                plainText = null;

                return Convert.ToBase64String(cipherText).SecureString();
            }

            /// <summary>
            /// To encrypt, plain data
            /// </summary>
            /// <param name="secretMessage">Data to encrypt</param>
            /// <param name="password">Password for encrypting</param>
            /// <param name="nonSecretPayload">nonSecretPayload for encryption</param>
            /// <returns>Returns encrypted data</returns>
            public static byte[] Encrypt(byte[] secretMessage, SecureString password = null, byte[] nonSecretPayload = null)
            {
                nonSecretPayload = nonSecretPayload ?? new byte[] { };
                if (password == null)
                {
                    password = Security.Generator.HWID;
                }

                ////User Error Checks
                if (password.IsNullOrWhiteSpace() || password.UnsecureString().Length < MinPasswordLength)
                {
                    throw new ArgumentException(string.Format("Must have a password of at least {0} characters!", MinPasswordLength), "password");
                }

                if (secretMessage == null || secretMessage.Length == 0)
                {
                    return null;
                }

                var generator = new Pkcs5S2ParametersGenerator();

                ////Use Random Salt to minimize pre-generated weak password attacks.
                var salt = new byte[SaltBitSize / 8];
                Random.NextBytes(salt);

                generator.Init(PbeParametersGenerator.Pkcs5PasswordToBytes(password.UnsecureString().ToCharArray()), salt, Iterations);

                ////Generate Key
                var key = (KeyParameter)generator.GenerateDerivedMacParameters(KeyBitSize);

                ////Create Full Non Secret Payload
                var payload = new byte[salt.Length + nonSecretPayload.Length];
                Array.Copy(nonSecretPayload, payload, nonSecretPayload.Length);
                Array.Copy(salt, 0, payload, nonSecretPayload.Length, salt.Length);

                return Encrypt(secretMessage, key.GetKey(), payload);
            }

            /// <summary>
            /// To encrypt, plain data
            /// </summary>
            /// <param name="secretMessage">Data to encrypt</param>
            /// <param name="key">Key for encrypting</param>
            /// <param name="nonSecretPayload">nonSecretPayload for encryption</param>
            /// <returns>Returns encrypted data</returns>
            public static byte[] Encrypt(byte[] secretMessage, byte[] key, byte[] nonSecretPayload = null)
            {
                ////User Error Checks
                if (key == null || key.Length != KeyBitSize / 8)
                {
                    throw new ArgumentException(string.Format("Key needs to be {0} bit!", KeyBitSize), "key");
                }

                if (secretMessage == null || secretMessage.Length == 0)
                {
                    return null;
                }

                ////Non-secret Payload Optional
                nonSecretPayload = nonSecretPayload ?? new byte[] { };

                ////Using random nonce large enough not to repeat
                var nonce = new byte[NonceBitSize / 8];
                Random.NextBytes(nonce, 0, nonce.Length);

                var cipher = new GcmBlockCipher(new AesFastEngine());
                var parameters = new AeadParameters(new KeyParameter(key), MacBitSize, nonce, nonSecretPayload);
                cipher.Init(true, parameters);

                ////Generate Cipher Text With Auth Tag
                var cipherText = new byte[cipher.GetOutputSize(secretMessage.Length)];
                var len = cipher.ProcessBytes(secretMessage, 0, secretMessage.Length, cipherText, 0);
                cipher.DoFinal(cipherText, len);

                ////Assemble Message
                using (var combinedStream = new MemoryStream())
                {
                    using (var binaryWriter = new BinaryWriter(combinedStream))
                    {
                        ////Prepend Authenticated Payload
                        binaryWriter.Write(nonSecretPayload);
                        ////Prepend Nonce
                        binaryWriter.Write(nonce);
                        ////Write Cipher Text
                        binaryWriter.Write(cipherText);
                    }

                    return combinedStream.ToArray();
                }
            }

            /// <summary>
            /// Generates new key
            /// </summary>
            /// <returns>Returns new key</returns>
            public static byte[] NewKey()
            {
                var key = new byte[KeyBitSize / 8];
                Random.NextBytes(key);
                return key;
            }
        }

        /// <summary>
        /// Generates a 16 byte Unique Identification code of a computer
        /// </summary>
        public static class Generator
        {
            /// <summary>
            /// For storing HWID
            /// </summary>
            private static SecureString hwid = string.Empty.SecureString();

            /// <summary>
            /// Gets or sets HWID
            /// </summary>
            public static SecureString HWID
            {
                get
                {
                    return hwid;
                }

                set
                {
                    hwid = HWID;
                }
            }

            /// <summary>
            /// Generates a HWID
            /// </summary>
            /// <returns>Returns HWID</returns>
            public static SecureString GenerateHWIDValue()
            {
                ////You don't need to generate the HWID again if it has already been generated. This is better for performance
                ////Also, your HWID generally doesn't change when your computer is turned on but it can happen.
                ////It's up to you if you want to keep generating a HWID or not if the function is called.
                if (hwid.IsNullOrEmpty())
                {
                    hwid = hwid.AppendSecureString("CPU >> ".SecureString());
                    hwid = hwid.AppendSecureString(CpuId());
                    hwid = hwid.AppendSecureString("\nBIOS >> ".SecureString());
                    hwid = hwid.AppendSecureString(BiosId());
                    hwid = hwid.AppendSecureString("\nBASE >> ".SecureString());
                    hwid = hwid.AppendSecureString(BaseId());
                    hwid = hwid.AppendSecureString("\nDISK >> ".SecureString());
                    hwid = hwid.AppendSecureString(DiskId());
                    hwid = hwid = GetHash(hwid);
                }

                return hwid;
            }

            /// <summary>
            /// Motherboard ID
            /// </summary>
            /// <returns>Returns motherboard ID</returns>
            private static SecureString BaseId()
            {
                return Identifier("Win32_BaseBoard".SecureString(), "SerialNumber".SecureString());
            }

            /// <summary>
            /// BIOS Identifier
            /// </summary>
            /// <returns>Returns BIOS Id</returns>
            private static SecureString BiosId()
            {
                SecureString temp = Identifier("Win32_BIOS".SecureString(), "SerialNumber".SecureString());
                temp = temp.AppendSecureString(Identifier("Win32_BIOS".SecureString(), "IdentificationCode".SecureString()));
                return temp;
            }

            /// <summary>
            /// CPU id
            /// </summary>
            /// <returns>Returns CPU id</returns>
            private static SecureString CpuId()
            {
                ////Uses first CPU identifier available in order of preference
                ////Don't get all identifiers, as it is very time consuming
                SecureString retVal = Identifier("Win32_Processor".SecureString(), "UniqueId".SecureString());
                if (!retVal.IsNullOrEmpty())
                {
                    return retVal;
                }

                retVal = Identifier("Win32_Processor".SecureString(), "ProcessorId".SecureString());
                if (!retVal.IsNullOrEmpty())
                {
                    return retVal;
                }

                retVal = Identifier("Win32_Processor".SecureString(), "Name".SecureString());
                ////If no Name, use Manufacturer
                if (retVal.IsNullOrEmpty())
                {
                    retVal = Identifier("Win32_Processor".SecureString(), "Manufacturer".SecureString());
                }

                return retVal;
            }

            /// <summary>
            /// Main physical hard drive ID
            /// </summary>
            /// <returns>Returns physical hard drive ID</returns>
            private static SecureString DiskId()
            {
                SecureString temp = Identifier("Win32_DiskDrive".SecureString(), "Model".SecureString());
                temp = temp.AppendSecureString(Identifier("Win32_DiskDrive".SecureString(), "Manufacturer".SecureString()));
                temp = temp.AppendSecureString(Identifier("Win32_DiskDrive".SecureString(), "Signature".SecureString()));
                temp = temp.AppendSecureString(Identifier("Win32_DiskDrive".SecureString(), "TotalHeads".SecureString()));
                return temp;
            }

            /// <summary>
            /// Generates MD5 Hash
            /// </summary>
            /// <param name="s">SecureString to generate hash from it</param>
            /// <returns>Returns MD5 hash</returns>
            private static SecureString GetHash(SecureString s)
            {
                MD5Digest digest = new MD5Digest();
                digest.BlockUpdate(Encoding.ASCII.GetBytes(s.UnsecureString()), 0, Encoding.ASCII.GetBytes(s.UnsecureString()).Length);
                byte[] hash = new byte[16];
                digest.DoFinal(hash, 0);

                return GetHexString(hash);
            }

            /// <summary>
            /// Gets Hex data fron bytes
            /// </summary>
            /// <param name="bt">Bytes convert to hex</param>
            /// <returns>Returns Hex</returns>
            private static SecureString GetHexString(IList<byte> bt)
            {
                string s = string.Empty;
                for (int i = 0; i < bt.Count; i++)
                {
                    byte b = bt[i];
                    int n = b;
                    int n1 = n & 15;
                    int n2 = (n >> 4) & 15;
                    if (n2 > 9)
                    {
                        s += ((char)(n2 - 10 + 'A')).ToString(CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        s += n2.ToString(CultureInfo.InvariantCulture);
                    }

                    if (n1 > 9)
                    {
                        s += ((char)(n1 - 10 + 'A')).ToString(CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        s += n1.ToString(CultureInfo.InvariantCulture);
                    }

                    if ((i + 1) != bt.Count && (i + 1) % 2 == 0)
                    {
                        s += "-";
                    }
                }

                return s.SecureString();
            }

            /// <summary>
            /// Return a hardware identifier
            /// </summary>
            /// <param name="wmiClass">WMI class</param>
            /// <param name="wmiProperty">WMI property</param>
            /// <param name="wmiMustBeTrue">WMI MustBeTrue</param>
            /// <returns>Returns identifier</returns>
            private static SecureString Identifier(SecureString wmiClass, SecureString wmiProperty, SecureString wmiMustBeTrue)
            {
                string result = string.Empty;
                System.Management.ManagementClass mc = new System.Management.ManagementClass(wmiClass.UnsecureString());
                System.Management.ManagementObjectCollection moc = mc.GetInstances();
                foreach (System.Management.ManagementBaseObject mo in moc)
                {
                    if (mo[wmiMustBeTrue.UnsecureString()].ToString() != "True")
                    {
                        continue;
                    }

                    ////Only get the first one
                    if (result != string.Empty)
                    {
                        continue;
                    }

                    try
                    {
                        result = mo[wmiProperty.UnsecureString()].ToString();
                        break;
                    }
                    catch
                    {
                    }
                }

                return result.SecureString();
            }

            /// <summary>
            /// Return a hardware identifier
            /// </summary>
            /// <param name="wmiClass">WMI class</param>
            /// <param name="wmiProperty">WMI property</param>
            /// <returns>Returns identifier</returns>
            private static SecureString Identifier(SecureString wmiClass, SecureString wmiProperty)
            {
                string result = string.Empty;
                System.Management.ManagementClass mc = new System.Management.ManagementClass(wmiClass.UnsecureString());
                System.Management.ManagementObjectCollection moc = mc.GetInstances();
                foreach (System.Management.ManagementBaseObject mo in moc)
                {
                    ////Only get the first one
                    if (result != string.Empty)
                    {
                        continue;
                    }

                    try
                    {
                        result = mo[wmiProperty.UnsecureString()].ToString();
                        break;
                    }
                    catch
                    {
                    }
                }

                return result.SecureString();
            }
        }
    }
}