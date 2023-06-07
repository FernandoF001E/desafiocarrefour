using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CryptoSecurity
{
    internal class Obfuscator : ObfuscationBase
    {
        private static readonly RandomNumberGenerator rng = RandomNumberGenerator.Create();

        public string Obfuscate(string text)
        {
            byte[] ARC4Key = GetRandomBytes(KEYLENGTH);
            ModifiedARC4 modifiedARC = new ModifiedARC4(ARC4Key);
            byte[] ARC4Enc = modifiedARC.Encrypt(text);
            byte[] shuffle = Concat(ARC4Key, ARC4Enc);
            byte[] RSeed = GetRandomBytes(SEEDLENGTH);
            short[] swappers = GenerateSwappers(shuffle.Length, RSeed);
            byte[] swapped = Swap(shuffle, swappers, true);
            byte[] result = Concat(swapped, RSeed);
            return Convert.ToBase64String(result);
        }

        protected static byte[] Concat(byte[] a, byte[] b)
        {
            byte[] c = new byte[a.Length + b.Length];
            a.CopyTo(c, 0);
            b.CopyTo(c, a.Length);
            return c;
        }

        protected static byte[] GetRandomBytes(int length)
        {
            byte[] bytes = new byte[length];
            rng.GetNonZeroBytes(bytes);
            return bytes;
        }
    }
}
