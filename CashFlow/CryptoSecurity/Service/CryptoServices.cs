using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CryptoSecurity.Service
{
    public class CryptoServices : ICryptoServices
    {
        private static readonly string _preffix = "Zn&7469#";

        private readonly RSABase rsaBase = new RSABase();
        private readonly RSATools rsaTools = new RSATools(_preffix);
        private readonly ObfuscateActions obfuscate = new ObfuscateActions(_preffix);

        public string GetMD5(string input)
        {
            return Md5.GetHash(input);
        }

        public string GetSHA512(string input)
        {
            return Sha512.GetHash(input);
        }

        public string Base64Encode(string input)
        {
            return Base64.Base64Encode(input);
        }

        public string Base64Decode(string input)
        {
            return Base64.Base64Decode(input);
        }

        public Dictionary<string, string> RSACreateKeys()
        {
            rsaBase.GenerationKeys();

            Dictionary<string, string> data = new Dictionary<string, string>
            {
                { "public", rsaBase.PublicKey },
                { "private", rsaBase.PrivateKey },

                { "public-json", rsaBase.PublicKeyJson },
                { "private-json", rsaBase.PrivateKeyJson }
            };

            return data;
        }

        public string RSAEncrypt(string clearText, string publicKey)
        {
            return rsaTools.Encrypt(clearText, publicKey);
        }

        public string RSAEncrypt(string clearText, RSAParametersJson publicKey)
        {
            return rsaTools.Encrypt(clearText, publicKey);
        }

        public string RSADecrypt(string cipherText, string privateKey)
        {
            return rsaTools.Decrypt(cipherText, privateKey);
        }

        public string RSASigned(string clearText, string privateKey)
        {
            return rsaTools.Signed(clearText, privateKey);
        }

        public string RSASigned(string clearText, RSAParametersJson privateKey)
        {
            return rsaTools.Signed(clearText, privateKey);
        }

        public bool RSAIsValidSigned(string cipherText, string signed, string privateKey)
        {
            return rsaTools.IsValidSigned(cipherText, signed, privateKey);
        }

        public bool RSAIsValidSigned(string cipherText, string signed, RSAParametersJson privateKey)
        {
            return rsaTools.IsValidSigned(cipherText, signed, privateKey);
        }

        public bool RSAHasCipher(string cipherText)
        {
            return rsaTools.HasCipher(cipherText);
        }

        public bool HasObfuscate(string text)
        {
            return obfuscate.HasObfuscate(text);
        }

        public string Obfuscate(string clearText)
        {
            return obfuscate.Obfuscate(clearText);
        }

        public string Desobfuscate(string obfuscatedText)
        {
            return obfuscate.Desobfuscate(obfuscatedText);
        }

        public string GeneratorPassword(int tamanho)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ123456789@#!<>abcdefghijklmnopqrstuvwxyz";
            var random = new Random();
            var result = new string(
                Enumerable.Repeat(chars, tamanho)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());
            return result;
        }
    }
}
