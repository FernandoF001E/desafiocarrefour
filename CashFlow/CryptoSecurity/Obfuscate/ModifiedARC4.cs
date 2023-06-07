using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoSecurity
{
    internal class ModifiedARC4
    {
        private const int INCREASE = 137;

        private readonly byte[] state = new byte[256];
        private byte i;
        private byte j;

        public ModifiedARC4(byte[] key)
        {
            int keySize = key.Length;

            for (int k = 0; k < 256; k++)
            {
                state[k] = (byte)((k + INCREASE) & 0xFF);
            }

            int m = 0;
            for (int k = 0; k < 256; k++)
            {
                m = (m + state[k] * key[k % keySize]) & 0xFF;
                Swap(k, m);
            }

            i = 0;
            j = 0;

            // Descarta os primeiros 1024 bytes gerados
            for (int n = 1024; n > 0; n--)
                NextMask();
        }

        public byte[] Encrypt(byte[] src)
        {
            int length = src.Length;
            byte[] dst = new byte[length];
            for (int k = 0; k < length; k++)
                dst[k] = (byte)(NextMask() ^ src[k]);
            return dst;
        }

        public byte[] Encrypt(string src)
        {
            return Encrypt(Encoding.UTF8.GetBytes(src));
        }

        public string Decrypt(byte[] src)
        {
            return Encoding.UTF8.GetString(Encrypt(src));
        }

        private byte NextMask()
        {
            i = (byte)((i + 1) & 0xFF);
            j = (byte)((j + state[i]) & 0xFF);

            Swap(j, i);

            int where = (state[i] * state[j]) & 0xFF;
            return state[where];
        }

        private void Swap(int k, int m)
        {
            byte tmp = state[m];
            state[m] = state[k];
            state[k] = tmp;
        }
    }
}
