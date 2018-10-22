using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Gseey.Framework.Common.Helpers
{/// <summary>
 /// 加密帮助类
 /// </summary>
    public class EncryptHelper
    {
        #region 属性
        /// <summary>
        /// 加密盐值
        /// </summary>
        private static string EncryptSalt
        {
            get
            {
                return ConfigHelper.Get("EncryptSalt", "sDr!4@sd$h5");
            }
        }

        /// <summary>
        /// 加密密钥
        /// </summary>
        private static string EncryptKey
        {
            get
            {
                return GetEncryptKey(ConfigHelper.Get("EncryptKey", "5df523*^&ed2"));
            }
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 获取加密密钥（长度固定8位）
        /// </summary>
        /// <param name="value">字符串</param>
        /// <returns></returns>
        private static string GetEncryptKey(string value)
        {
            var result = string.Empty;
            if (value.Length > 8)
            {
                result = value.Substring(0, 8);
            }
            else
            {
                result = value.PadRight(8, '0');
            }
            return result;
        }
        #endregion

        #region 公有方法
        /// <summary>
        /// 对字符串进行des加密
        /// </summary>
        /// <param name="value">要加密的字符串</param>
        /// <param name="encoding">编码格式，默认utf-8</param>
        /// <param name="encryptTimes">加密次数，默认1次</param>
        /// <returns></returns>
        public static string EncryptDES(string value, Encoding encoding = null, int encryptTimes = 1)
        {
            if (encryptTimes <= 1)
                encryptTimes = 1;
            var result = string.Empty;
            var tempValue = value;
            if (encoding == null)
                encoding = Encoding.UTF8;
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            des.Mode = CipherMode.ECB;
            des.Padding = PaddingMode.Zeros;
            try
            {
                for (int i = 0; i < encryptTimes; i++)
                {
                    tempValue += EncryptSalt;
                    var valueBuffer = encoding.GetBytes(tempValue);
                    var keyBuffer = encoding.GetBytes(EncryptKey);
                    des.Key = encoding.GetBytes(EncryptKey);
                    des.IV = encoding.GetBytes(EncryptKey);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(des.Key, des.IV), CryptoStreamMode.Write))
                        {
                            cs.Write(valueBuffer, 0, valueBuffer.Length);
                            cs.FlushFinalBlock();
                            tempValue = Convert.ToBase64String(ms.ToArray());
                        }
                    }
                }
                result = tempValue;
            }
            catch (Exception ex)
            {
                LogHelper.Error(string.Format("DES加密错误，{0}", ex.Message), ex);
            }
            return result;
        }

        /// <summary>
        /// 对字符串进行DES解密
        /// </summary>
        /// <param name="value">解密字符串</param>
        /// <param name="encoding">编码格式，默认utf-8</param>
        /// <param name="encryptTimes">加密次数，默认1次</param>
        /// <returns></returns>
        public static string DencryptDES(string value, Encoding encoding = null, int encryptTimes = 1)
        {
            if (encryptTimes <= 1)
                encryptTimes = 1;
            var result = string.Empty;
            var tempValue = value;
            if (encoding == null)
                encoding = Encoding.UTF8;
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            des.Mode = CipherMode.ECB;
            des.Padding = PaddingMode.Zeros;
            try
            {
                for (int i = 0; i < encryptTimes; i++)
                {
                    des.Key = encoding.GetBytes(EncryptKey);
                    des.IV = encoding.GetBytes(EncryptKey);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        var buffer = Convert.FromBase64String(tempValue);

                        using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(des.Key, des.IV), CryptoStreamMode.Write))
                        {
                            cs.Write(buffer, 0, buffer.Length);
                            cs.FlushFinalBlock();
                            tempValue = encoding.GetString(ms.ToArray().Where(m => m > 0).ToArray());
                            tempValue = tempValue.Replace(EncryptSalt, "");
                        }
                    }
                }
                result = tempValue;
            }
            catch (Exception ex)
            {
                LogHelper.Error(string.Format("DES解密错误，{0}", ex.Message), ex);
            }
            return result;
        }

        /// <summary>
        /// MD5加密字符串
        /// </summary>
        /// <param name="value">要加密的字符串</param>
        /// <param name="encryptTimes">加密次数，默认1次</param>
        /// <param name="length">16或32值之一，其它则采用.net默认MD5加密算法</param>
        /// <returns></returns>
        public static string EncryptMD5(string value, int encryptTimes = 1, int length = 0)
        {
            if (encryptTimes <= 1)
                encryptTimes = 1;
            var result = string.Empty;

            try
            {
                var tempValue = value + EncryptSalt;

                for (int i = 1; i <= encryptTimes; i++)
                {
                    var buffer = Encoding.ASCII.GetBytes(tempValue);
                    var hashValue = MD5.Create().ComputeHash(buffer);

                    switch (length)
                    {
                        case 16:
                            result = BitConverter.ToString(hashValue, 4, 8).Replace("-", "");
                            break;
                        case 32:
                            result = BitConverter.ToString(hashValue, 0, 16).Replace("-", "");
                            break;
                        default:
                            result = BitConverter.ToString(hashValue).Replace("-", "");
                            break;
                    }

                    tempValue = value + EncryptSalt;
                }
                result = tempValue;
                return result;
            }
            catch
            {
                return result;
            }
        }

        /// <summary>
        /// RES加密
        /// </summary>
        /// <param name="xmlPublicKey">公钥</param>
        /// <param name="value">待加密的字符串</param>
        /// <param name="encoding">加密编码</param>
        /// <returns></returns>
        public static string EncrpytRES(string xmlPublicKey, string value, Encoding encoding = null)
        {
            if (encoding == null)
                encoding = Encoding.UTF8;
            byte[] PlainTextBArray;
            byte[] CypherTextBArray;
            string Result;
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(xmlPublicKey);
            PlainTextBArray = encoding.GetBytes(value);
            CypherTextBArray = rsa.Encrypt(PlainTextBArray, false);
            Result = Convert.ToBase64String(CypherTextBArray);
            return Result;
        }

        /// <summary>
        /// RES解密
        /// </summary>
        /// <param name="xmlPrivateKey">私钥</param>
        /// <param name="value">待加密的字符串</param>
        /// <param name="encoding">加密编码</param>
        /// <returns></returns>
        public static string DencryptRES(string xmlPrivateKey, string value, Encoding encoding = null)
        {
            if (encoding == null)
                encoding = Encoding.UTF8;
            byte[] PlainTextBArray;
            byte[] DypherTextBArray;
            string Result;
            System.Security.Cryptography.RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(xmlPrivateKey);
            PlainTextBArray = Convert.FromBase64String(value);
            DypherTextBArray = rsa.Decrypt(PlainTextBArray, false);
            Result = encoding.GetString(DypherTextBArray);
            return Result;
        }
        #endregion

    }
}
