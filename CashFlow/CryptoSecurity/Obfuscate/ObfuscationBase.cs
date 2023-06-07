using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoSecurity
{
    internal class ObfuscationBase
    {
        private const string SEED = "Se você quiser descobrir os segredos do Universo, pense em termos de energia, frequência e vibração.";

        protected const int KEYLENGTH = 16;
        protected const int SEEDLENGTH = 4;
        private static readonly byte[] C = Encoding.UTF8.GetBytes(SEED);

        protected static byte[] Swap(byte[] shuffle, short[] swappers, bool up)
        {
            int top = swappers.Length - 2;
            for (int i = 0; i <= top; i += 2)
            {
                int j = up ? i : (top - i);
                int a = swappers[j] % shuffle.Length;
                int b = swappers[j + 1] % shuffle.Length;
                Swap(shuffle, a, b);
            }
            return shuffle;
        }

        private static void Swap(byte[] array, int a, int b)
        {
            byte c = array[a];
            array[a] = array[b];
            array[b] = c;
        }

        protected static short[] GenerateSwappers(int length, byte[] RSeed)
        {
            byte[] arcSeed = new byte[C.Length + 4];
            RSeed.CopyTo(arcSeed, 0);
            C.CopyTo(arcSeed, 4);
            ModifiedARC4 arc2 = new ModifiedARC4(RSeed);
            short[] swappers = new short[length * 2];
            byte[] swappersBytes = arc2.Encrypt(new byte[(swappers.Length * 2) / 3]);
            int window = swappersBytes.Length;
            for (int i = 0; i < swappers.Length; i++)
            {
                swappers[i] = Math.Abs((short)(((swappersBytes[WrapDownAround(i, window)]) + 256 * swappersBytes[i % window]) & 0xFFFF));
            }
            return swappers;
        }

        private static int WrapDownAround(int i, int window)
        {
            int down = (window - i - 1);
            while (down < 0)
                down += window;
            return down % window;
        }
    }
}
