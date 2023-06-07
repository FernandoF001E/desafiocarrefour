using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CryptoSecurity
{
    public class Md5
    {
        public static string GetHash(string input)
        {
            string result = "";

            if (!string.IsNullOrEmpty(input))
            {
                MD5 md5 = MD5.Create();
                byte[] data = md5.ComputeHash(Encoding.UTF8.GetBytes(input));


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
