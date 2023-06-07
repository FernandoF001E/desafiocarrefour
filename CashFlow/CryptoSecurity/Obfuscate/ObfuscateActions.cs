using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CryptoSecurity
{
    internal class ObfuscateActions
    {
        private string preffix;

        public ObfuscateActions(string preffixSeed)
        {
            preffix = preffixSeed;
        }

        public bool HasObfuscate(string text)
        {
            bool result = false;

            if (string.IsNullOrEmpty(text))
                throw new ArgumentNullException("obfuscatedText not found");

            if (text.StartsWith(preffix))
            {
                result = true;
            }

            return result;
        }

        public string Obfuscate(string clearText)
        {
            if (string.IsNullOrEmpty(clearText))
                throw new ArgumentNullException("clearText");

            Obfuscator of = new Obfuscator();

            return preffix + of.Obfuscate(clearText);
        }

        public string Desobfuscate(string obfuscatedText)
        {
            if (string.IsNullOrEmpty(obfuscatedText))
                throw new ArgumentNullException("obfuscatedText not found");
            if (obfuscatedText.StartsWith(preffix))
            {
                obfuscatedText = obfuscatedText.Substring(preffix.Length);
                Desobfuscator dof = new Desobfuscator();
                return dof.Desobfuscate(obfuscatedText);
            }
            return obfuscatedText;
        }

        public string CreateSalt(int size)
        {
            RNGCryptoServiceProvider rng = new();
            byte[] buff = new byte[size];
            rng.GetBytes(buff);
            return Convert.ToBase64String(buff);
        }

        public string CreatePasswordHash(string pwd, string salt, int size)
        {
            string saltAndPwd = string.Concat(salt, pwd, salt).ToLowerInvariant();
            SHA256 sha256 = SHA256.Create();
            sha256.Initialize();
            sha256.ComputeHash(Encoding.UTF8.GetBytes(saltAndPwd));
            byte[] hash = sha256.Hash;
            byte[] outHash = new byte[size];
            if (size < hash.Length - 1)
            {
                int offsetWindow = hash.Length - 1 - size;
                int offset = hash[hash.Length - 1] % offsetWindow;
                Array.Copy(hash, offset, outHash, 0, size);
            }
            else
            {
                hash.CopyTo(outHash, 0);
            }
            string passwordHash = Convert.ToBase64String(outHash, Base64FormattingOptions.None);

            return passwordHash;
        }

        public string CreateShortPasswordHash(string pwd, string username)
        {
            return CreatePasswordHash(pwd, username, 8).Substring(0, 10);
        }
    }
}
