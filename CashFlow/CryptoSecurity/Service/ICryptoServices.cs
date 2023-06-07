using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoSecurity.Service
{
    public interface ICryptoServices
    {
        string GetMD5(string input);
        string GetSHA512(string input);
        string Base64Encode(string input);
        string Base64Decode(string input);
        Dictionary<string, string> RSACreateKeys();
        string RSAEncrypt(string plainText, string publicKey);
        string RSAEncrypt(string plainText, RSAParametersJson publicKey);
        string RSADecrypt(string cipherText, string privateKey);
        string RSASigned(string clearText, string privateKey);
        string RSASigned(string clearText, RSAParametersJson privateKey);
        bool RSAIsValidSigned(string cipherText, string signed, string privateKey);
        bool RSAIsValidSigned(string cipherText, string signed, RSAParametersJson privateKey);
        bool RSAHasCipher(string cipherText);
        string Obfuscate(string clearText);
        string Desobfuscate(string obfuscatedText);
        bool HasObfuscate(string text);
        string GeneratorPassword(int tamanho);
    }
}
