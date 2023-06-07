using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoSecurity
{
    public class Base64
    {
        public static string Base64Encode(string plainText)
        {
            string result = "";

            if (!string.IsNullOrEmpty(plainText))
            {
                var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
                result = Convert.ToBase64String(plainTextBytes);
            }

            return result;
        }

        public static string Base64Decode(string textBase64)
        {
            string result = "";

            if (!string.IsNullOrEmpty(textBase64))
            {
                var base64EncodedBytes = Convert.FromBase64String(textBase64);
                return Encoding.UTF8.GetString(base64EncodedBytes);
            }

            return result;
        }
    }
}
