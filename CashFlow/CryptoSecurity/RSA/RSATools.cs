using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CryptoSecurity
{
    internal class RSATools
    {
        private string preffix;
        private readonly RSABase rsaBase = new RSABase();

        public RSATools(string preffixSeed)
        {
            preffix = preffixSeed;
        }

        public enum eLibCipher
        {
            CSharp,
            PHP,
            Java
        }

        public string Encrypt(string clearText, string publicKey)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(rsaBase.SizeRSA);
            RSAParameters parameters = rsaBase.FromXmlString(publicKey);
            rsa.ImportParameters(parameters);

            return this.Encrypt(clearText, rsa);
        }

        public string Encrypt(string clearText, RSAParametersJson publicKey)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(rsaBase.SizeRSA);
            RSAParameters parameters = rsaBase.FromJsonString(publicKey, true);
            rsa.ImportParameters(parameters);

            return this.Encrypt(clearText, rsa);
        }

        public string Encrypt(string clearText, RSACryptoServiceProvider provider)
        {
            string result = string.Empty;
            string cipherStr = string.Empty;
            byte[] encryptedData;

            encryptedData = rsaBase.RSAEncrypt(rsaBase.ConvertoStringToByteArray(clearText), provider.ExportParameters(false), false);

            if (encryptedData != null && encryptedData.Length > 0)
            {
                cipherStr = rsaBase.ConvertToBase64String(encryptedData);
            }

            //--> Add preffix
            result = preffix + Convert.ToInt32(eLibCipher.CSharp).ToString() + cipherStr;


            return result;
        }

        public string Decrypt(string cipherText, string privateKey)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(rsaBase.SizeRSA);
            RSAParameters parameters = rsaBase.FromXmlString(privateKey);
            rsa.ImportParameters(parameters);

            return this.Decrypt(cipherText, rsa);
        }

        public string Decrypt(string cipherText, RSACryptoServiceProvider provider)
        {
            string clearText = string.Empty;
            string data = string.Empty;
            bool oaep = false;
            byte[] decryptedData;

            if (HasCipher(cipherText))
            {
                //--> removendo lixo (preffixo e tecnologia usada) da criptografia
                data = cipherText.Substring(preffix.Length + 1);

                //--> Identificando tecnologia que gerou a criptografia dos dados
                if (Convert.ToInt32(cipherText.Substring(preffix.Length, 1)) > 0)
                {
                    oaep = true;
                }

                decryptedData = rsaBase.RSADecrypt(rsaBase.ConvertoBase64ToByteArray(data),
                                                    provider.ExportParameters(true),
                                                    oaep);
                if (decryptedData != null && decryptedData.Length > 0)
                {
                    clearText = rsaBase.ConvertoByteArrayToString(decryptedData);
                }
            }
            else throw new ArgumentNullException("obfuscatedText not found");

            return clearText;
        }

        public bool HasCipher(string cipherText)
        {
            bool result = false;

            try
            {
                if (cipherText.Substring(0, preffix.Length).Equals(preffix))
                {
                    result = true;
                }
            }
            catch
            {
                result = false;
            }

            return result;
        }

        public string Signed(string clearText, RSAParametersJson privateKey)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(rsaBase.SizeRSA);
            RSAParameters parameters = rsaBase.FromJsonString(privateKey);
            rsa.ImportParameters(parameters);
            return this.Signed(clearText, rsa);
        }

        public string Signed(string clearText, string privateKey)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(rsaBase.SizeRSA);
            RSAParameters parameters = rsaBase.FromXmlString(privateKey);
            rsa.ImportParameters(parameters);
            return this.Signed(clearText, rsa);
        }

        public string Signed(string clearText, RSACryptoServiceProvider provider)
        {
            string signed = string.Empty;

            byte[] dataToEncrypt = rsaBase.ConvertoStringToByteArray(clearText);

            RSAParameters privateKey = provider.ExportParameters(true);
            // Hash and sign the data.
            byte[] signedData = rsaBase.HashAndSignBytes(dataToEncrypt, privateKey);

            if (signedData != null && signedData.Length > 0)
            {
                signed = rsaBase.ConvertToBase64String(signedData);
            }

            return signed;
        }

        public bool IsValidSigned(string cipherText, string signed, RSAParametersJson privateKey)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(rsaBase.SizeRSA);
            RSAParameters parameters = rsaBase.FromJsonString(privateKey);
            rsa.ImportParameters(parameters);
            byte[] byteSigned = rsaBase.ConvertoBase64ToByteArray(signed);
            return this.IsValidSigned(cipherText, byteSigned, rsa);
        }

        public bool IsValidSigned(string cipherText, string signed, string privateKey)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(rsaBase.SizeRSA);
            RSAParameters parameters = rsaBase.FromXmlString(privateKey);
            rsa.ImportParameters(parameters);
            byte[] byteSigned = rsaBase.ConvertoBase64ToByteArray(signed);
            return this.IsValidSigned(cipherText, byteSigned, rsa);
        }

        public bool IsValidSigned(string cipherText, byte[] signed, RSACryptoServiceProvider provider)
        {
            string clearText = cipherText;

            if (this.HasCipher(cipherText))
            {
                clearText = this.Decrypt(cipherText, provider);
            }

            byte[] dataClear = rsaBase.ConvertoStringToByteArray(clearText);

            RSAParameters privateKey = provider.ExportParameters(true);

            return rsaBase.VerifySignedHash(dataClear, signed, privateKey);
        }
    }
}
