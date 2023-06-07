using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CryptoSecurity
{
    internal class RSABase
    {
        private System.Security.Cryptography.RSA rsa = null;
        public string? PublicKey { get; set; }
        public string? PrivateKey { get; set; }
        public string? PublicKeyJson { get; set; }
        public string? PrivateKeyJson { get; set; }

        int size = 2048;

        public int SizeRSA
        {
            get
            {
                return this.size;
            }
        }

        public string GenerationKeys()
        {
            //return this.GenerationKeys(1024);
            return this.GenerationKeys(size);
        }

        public string GenerationKeys(int keySize)
        {
            string result = string.Empty;
            try
            {
                rsa = System.Security.Cryptography.RSA.Create();
                rsa.KeySize = keySize;
                //-->
                RSAParameters param = rsa.ExportParameters(true);
                PublicKey = this.GetPublicKey(param);
                PrivateKey = this.GetPrivateKey(param);

                PublicKeyJson = this.GetKeyJson(param, false);
                PrivateKeyJson = this.GetKeyJson(param, true);

            }
            catch (Exception)
            {
                result = string.Empty;
                throw;
            }

            return result;
        }

        public byte[] RSAEncrypt(byte[] DataToEncrypt, RSAParameters RSAKeyInfo)
        {
            bool DoOAEPPadding = true;
            return RSAEncrypt(DataToEncrypt, RSAKeyInfo, DoOAEPPadding);
        }

        public byte[] RSAEncrypt(byte[] DataToEncrypt, RSAParameters RSAKeyInfo, bool DoOAEPPadding)
        {
            byte[] encryptedData;

            try
            {
                //Create a new instance of RSACryptoServiceProvider.
                using RSACryptoServiceProvider RSA = new();
                //Import the RSA Key information. This only needs
                //toinclude the public key information.
                RSA.ImportParameters(RSAKeyInfo);
                //Encrypt the passed byte array and specify OAEP padding.  
                //OAEP padding is only available on Microsoft Windows XP or
                //later.  
                encryptedData = RSA.Encrypt(DataToEncrypt, DoOAEPPadding);
            }
            catch (CryptographicException)
            {
                encryptedData = null;
            }

            return encryptedData;
        }

        public byte[] RSADecrypt(byte[] DataToDecrypt, RSAParameters RSAKeyInfo)
        {
            bool DoOAEPPadding = true;
            return RSADecrypt(DataToDecrypt, RSAKeyInfo, DoOAEPPadding);
        }

        public byte[] RSADecrypt(byte[] DataToDecrypt, RSAParameters RSAKeyInfo, bool DoOAEPPadding)
        {
            byte[] decryptedData;

            try
            {
                //Create a new instance of RSACryptoServiceProvider.
                using RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();

                //Import the RSA Key information. This needs
                //to include the private key information.
                RSA.ImportParameters(RSAKeyInfo);

                //Decrypt the passed byte array and specify OAEP padding.  
                //OAEP padding is only available on Microsoft Windows XP or
                //later.  
                decryptedData = RSA.Decrypt(DataToDecrypt, DoOAEPPadding);
                //decryptedData = RSA.Decrypt(DataToDecrypt, true); //--> Por causa da library no PHP não esta colocando false
            }
            catch (CryptographicException)
            {
                decryptedData = null;
            }
            return decryptedData;
        }

        public byte[] HashAndSignBytes(byte[] DataToSign, RSAParameters Key)
        {
            byte[] hashSignBytes;

            try
            {
                // Create a new instance of RSACryptoServiceProvider using the 
                // key from RSAParameters.  
                RSACryptoServiceProvider RSAalg = new();

                RSAalg.ImportParameters(Key);

                // Hash and sign the data. Pass a new instance of SHA1CryptoServiceProvider
                // to specify the use of SHA1 for hashing.
                hashSignBytes = RSAalg.SignData(DataToSign, new SHA1CryptoServiceProvider());
            }
            catch (CryptographicException)
            {
                hashSignBytes = null;
            }

            return hashSignBytes;
        }

        public bool VerifySignedHash(byte[] DataToVerify, byte[] SignedData, RSAParameters Key)
        {
            bool result;

            try
            {
                // Create a new instance of RSACryptoServiceProvider using the 
                // key from RSAParameters.
                RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider();

                RSAalg.ImportParameters(Key);

                // Verify the data using the signature.  Pass a new instance of SHA1CryptoServiceProvider
                // to specify the use of SHA1 for hashing.
                result = RSAalg.VerifyData(DataToVerify, new SHA1CryptoServiceProvider(), SignedData);

            }
            catch (CryptographicException)
            {
                result = false;
            }

            return result;
        }

        private string GetPublicKey(RSAParameters param)
        {
            string result = "<RSAKeyValue>";
            result += string.Format("<Modulus>{0}</Modulus>", Convert.ToBase64String(param.Modulus));
            result += string.Format("<Exponent>{0}</Exponent>", Convert.ToBase64String(param.Exponent));
            result += "</RSAKeyValue>";

            return result;
        }

        private string GetPrivateKey(RSAParameters param)
        {
            string result = "<RSAKeyValue>";
            result += string.Format("<Modulus>{0}</Modulus>", Convert.ToBase64String(param.Modulus));
            result += string.Format("<Exponent>{0}</Exponent>", Convert.ToBase64String(param.Exponent));
            result += string.Format("<P>{0}</P>", Convert.ToBase64String(param.P));
            result += string.Format("<Q>{0}</Q>", Convert.ToBase64String(param.Q));
            result += string.Format("<DP>{0}</DP>", Convert.ToBase64String(param.DP));
            result += string.Format("<DQ>{0}</DQ>", Convert.ToBase64String(param.DQ));
            result += string.Format("<InverseQ>{0}</InverseQ>", Convert.ToBase64String(param.InverseQ));
            result += string.Format("<D>{0}</D>", Convert.ToBase64String(param.D));
            result += "</RSAKeyValue>";

            return result;
        }

        private string GetKeyJson(RSAParameters parameters, bool IsIncludePrivateParameters)
        {
            RSAParametersJson parasJson = null;

            if (IsIncludePrivateParameters)
            {
                parasJson = new RSAParametersJson()
                {
                    Modulus = parameters.Modulus != null ? Convert.ToBase64String(parameters.Modulus) : null,
                    Exponent = parameters.Exponent != null ? Convert.ToBase64String(parameters.Exponent) : null,
                    P = parameters.P != null ? Convert.ToBase64String(parameters.P) : null,
                    Q = parameters.Q != null ? Convert.ToBase64String(parameters.Q) : null,
                    DP = parameters.DP != null ? Convert.ToBase64String(parameters.DP) : null,
                    DQ = parameters.DQ != null ? Convert.ToBase64String(parameters.DQ) : null,
                    InverseQ = parameters.InverseQ != null ? Convert.ToBase64String(parameters.InverseQ) : null,
                    D = parameters.D != null ? Convert.ToBase64String(parameters.D) : null
                };
            }
            else
            {
                parasJson = new RSAParametersJson()
                {
                    Modulus = parameters.Modulus != null ? Convert.ToBase64String(parameters.Modulus) : null,
                    Exponent = parameters.Exponent != null ? Convert.ToBase64String(parameters.Exponent) : null
                };
            }

            return JsonConvert.SerializeObject(parasJson);
        }


        public RSAParameters FromXmlString(string xmlString)
        {
            RSAParameters parameters = new RSAParameters();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlString);

            if (xmlDoc.DocumentElement.Name.Equals("RSAKeyValue"))
            {
                foreach (XmlNode node in xmlDoc.DocumentElement.ChildNodes)
                {
                    switch (node.Name)
                    {
                        case "Modulus": parameters.Modulus = Convert.FromBase64String(node.InnerText); break;
                        case "Exponent": parameters.Exponent = Convert.FromBase64String(node.InnerText); break;
                        case "P": parameters.P = Convert.FromBase64String(node.InnerText); break;
                        case "Q": parameters.Q = Convert.FromBase64String(node.InnerText); break;
                        case "DP": parameters.DP = Convert.FromBase64String(node.InnerText); break;
                        case "DQ": parameters.DQ = Convert.FromBase64String(node.InnerText); break;
                        case "InverseQ": parameters.InverseQ = Convert.FromBase64String(node.InnerText); break;
                        case "D": parameters.D = Convert.FromBase64String(node.InnerText); break;
                    }
                }
            }
            else
            {
                throw new Exception("Invalid XML RSA key.");
            }
            return parameters;
        }

        public RSAParameters FromJsonString(string jsonString)
        {
            RSAParameters parameters = new RSAParameters();

            try
            {
                if (!string.IsNullOrEmpty(jsonString))
                {
                    parameters = new RSAParameters();

                    var paramsJson = JsonConvert.DeserializeObject<RSAParametersJson>(jsonString);

                    parameters.Modulus = paramsJson.Modulus != null ? Convert.FromBase64String(paramsJson.Modulus) : null;
                    parameters.Exponent = paramsJson.Exponent != null ? Convert.FromBase64String(paramsJson.Exponent) : null;
                    parameters.P = paramsJson.P != null ? Convert.FromBase64String(paramsJson.P) : null;
                    parameters.Q = paramsJson.Q != null ? Convert.FromBase64String(paramsJson.Q) : null;
                    parameters.DP = paramsJson.DP != null ? Convert.FromBase64String(paramsJson.DP) : null;
                    parameters.DQ = paramsJson.DQ != null ? Convert.FromBase64String(paramsJson.DQ) : null;
                    parameters.InverseQ = paramsJson.InverseQ != null ? Convert.FromBase64String(paramsJson.InverseQ) : null;
                    parameters.D = paramsJson.D != null ? Convert.FromBase64String(paramsJson.D) : null;
                }
            }
            catch
            {
                throw new Exception("Invalid JSON RSA key.");
            }

            return parameters;
        }

        public RSAParameters FromJsonString(RSAParametersJson paramsJson)
        {
            return FromJsonString(paramsJson, false);
        }

        public RSAParameters FromJsonString(RSAParametersJson paramsJson, bool publicKey)
        {
            RSAParameters parameters = new RSAParameters();

            try
            {
                if (paramsJson != null)
                {
                    parameters = new RSAParameters();

                    if (publicKey)
                    {
                        parameters.Modulus = paramsJson.Modulus != null ? Convert.FromBase64String(paramsJson.Modulus) : null;
                        parameters.Exponent = paramsJson.Exponent != null ? Convert.FromBase64String(paramsJson.Exponent) : null;
                    }
                    else
                    {
                        parameters.Modulus = paramsJson.Modulus != null ? Convert.FromBase64String(paramsJson.Modulus) : null;
                        parameters.Exponent = paramsJson.Exponent != null ? Convert.FromBase64String(paramsJson.Exponent) : null;
                        parameters.P = paramsJson.P != null ? Convert.FromBase64String(paramsJson.P) : null;
                        parameters.Q = paramsJson.Q != null ? Convert.FromBase64String(paramsJson.Q) : null;
                        parameters.DP = paramsJson.DP != null ? Convert.FromBase64String(paramsJson.DP) : null;
                        parameters.DQ = paramsJson.DQ != null ? Convert.FromBase64String(paramsJson.DQ) : null;
                        parameters.InverseQ = paramsJson.InverseQ != null ? Convert.FromBase64String(paramsJson.InverseQ) : null;
                        parameters.D = paramsJson.D != null ? Convert.FromBase64String(paramsJson.D) : null;
                    }
                }
            }
            catch
            {
                throw new Exception("Invalid JSON RSA key.");
            }

            return parameters;
        }

        public string ConvertToBase64String(byte[] value)
        {
            return Convert.ToBase64String(value);
        }

        public byte[] ConvertoBase64ToByteArray(string value)
        {
            return Convert.FromBase64String(value);
        }

        public byte[] ConvertoStringToByteArray(string value)
        {
            ASCIIEncoding byteConverter = new ASCIIEncoding();
            return byteConverter.GetBytes(value);
        }

        public string ConvertoByteArrayToString(byte[] value)
        {
            ASCIIEncoding byteConverter = new ASCIIEncoding();
            return byteConverter.GetString(value);

        }
    }
}
