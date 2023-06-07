using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoSecurity
{
    internal class Desobfuscator : ObfuscationBase
    {
        public void split(byte[] origin, int splitPoint, out byte[] bottom, out byte[] top)
        {
            int length = origin.Length;
            if (splitPoint < 0)
                splitPoint += length;
            else if (splitPoint >= length)
                splitPoint = length;
            bottom = new byte[splitPoint];
            top = new byte[length - splitPoint];
            Array.Copy(origin, 0, bottom, 0, splitPoint);
            Array.Copy(origin, splitPoint, top, 0, length - splitPoint);
        }

        public string Desobfuscate(string text)
        {
            byte[] RSeed, swapped, ARC4Key, ARC4Enc;
            byte[] obfuscatedData = Convert.FromBase64String(text);
            split(obfuscatedData, -ObfuscationBase.SEEDLENGTH, out swapped, out RSeed);
            short[] swappers = ObfuscationBase.GenerateSwappers(swapped.Length, RSeed);
            byte[] shuffle = Swap(swapped, swappers, false);
            split(shuffle, ObfuscationBase.KEYLENGTH, out ARC4Key, out ARC4Enc);
            ModifiedARC4 modifiedARC = new ModifiedARC4(ARC4Key);
            return modifiedARC.Decrypt(ARC4Enc);
        }
    }
}
