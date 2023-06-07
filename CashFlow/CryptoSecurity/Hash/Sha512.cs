using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CryptoSecurity
{
    public class Sha512
    {
        public static string GetHash(string input)
        {
            string result = "";

            if (!string.IsNullOrEmpty(input))
            {
                SHA512 sha512 = SHA512.Create();
                byte[] data = sha512.ComputeHash(Encoding.UTF8.GetBytes(input));

                StringBuilder stringBuilder = new StringBuilder();

                for (int i = 0; i < data.Length; i++)
                {
                    stringBuilder.Append(data[i].ToString("x2"));
                }

                result = stringBuilder.ToString().ToUpper();
            }

            return result;
        }
    }
}
