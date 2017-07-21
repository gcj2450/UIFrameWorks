using UnityEngine;
using System.Collections;
using System.Security.Cryptography;
using System.IO;
//using Horizon;
using System;
using System.Text;

public class Encrypt
{
    #region "定义加密字串变量"
    private SymmetricAlgorithm mCSP;  //声明对称算法变量
    private const string CIV = "YmFzZTY0IGVuY29kZWQgc3RyaW5n";  //初始化向量
    private const string CKEY = "jkHuIy9D/9i="; //密钥（常量）
    #endregion
    /// <summary>
    /// 实例化
    /// </summary>
    public Encrypt()
    {
        mCSP = new DESCryptoServiceProvider();  //定义访问数据加密标准 (DES) 算法的加密服务提供程序 (CSP) 版本的包装对象,此类是SymmetricAlgorithm的派生类
    }
    /// <summary>
    /// 加密字符串
    /// </summary>
    /// <param name="Value">需加密的字符串</param>
    /// <returns></returns>
    public string EncryptString(string Value)
    {
        ICryptoTransform ct; //定义基本的加密转换运算
        MemoryStream ms; //定义内存流
        CryptoStream cs; //定义将内存流链接到加密转换的流
        byte[] byt;
        //CreateEncryptor创建(对称数据)加密对象
        ct = mCSP.CreateEncryptor(Convert.FromBase64String(CKEY), Convert.FromBase64String(CIV)); //用指定的密钥和初始化向量创建对称数据加密标准
        byt = Encoding.UTF8.GetBytes(Value); //将Value字符转换为UTF-8编码的字节序列
        ms = new MemoryStream(); //创建内存流
        cs = new CryptoStream(ms, ct, CryptoStreamMode.Write); //将内存流链接到加密转换的流
        cs.Write(byt, 0, byt.Length); //写入内存流
        cs.FlushFinalBlock(); //将缓冲区中的数据写入内存流，并清除缓冲区
        cs.Close(); //释放内存流
        return Convert.ToBase64String(ms.ToArray()); //将内存流转写入字节数组并转换为string字符
    }
    /// <summary>
    /// 解密字符串
    /// </summary>
    /// <param name="Value">要解密的字符串</param>
    /// <returns>string</returns>
    public string DecryptString(string Value)
    {
        ICryptoTransform ct; //定义基本的加密转换运算
        MemoryStream ms; //定义内存流
        CryptoStream cs; //定义将数据流链接到加密转换的流
        byte[] byt;
        ct = mCSP.CreateDecryptor(Convert.FromBase64String(CKEY), Convert.FromBase64String(CIV)); //用指定的密钥和初始化向量创建对称数据解密标准
        byt = Convert.FromBase64String(Value); //将Value(Base 64)字符转换成字节数组
        ms = new MemoryStream();
        cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);
        cs.Write(byt, 0, byt.Length);
        cs.FlushFinalBlock();
        cs.Close();
        return Encoding.UTF8.GetString(ms.ToArray()); //将字节数组中的所有字符解码为一个字符串
    }

    //------------------------

    //=========加密字符串

    private static byte[] AESKeys = { 0x41, 0x72, 0x65, 0x79, 0x6F, 0x75, 0x6D, 0x79, 0x53, 0x6E, 0x6F, 0x77, 0x6D, 0x61, 0x6E, 0x3F };

    /// <summary>
    /// AES加密
    /// </summary>
    /// <param name="encryptString"></param>
    /// <param name="encryptKey"></param>
    /// <returns></returns>
    public static string AESEncode(string encryptString, string encryptKey)
    {
        return AESEncodeStr(encryptString, encryptKey);
    }

    private static string AESEncodeStr(string encryptString, string encryptKey)
    {
        encryptKey = encryptKey.Trim();
        if (encryptKey.Length > 32)
        {
            encryptKey.Substring(0, 32);
        }
        encryptKey = encryptKey.PadRight(32, ' ');

        RijndaelManaged rijndaelProvider = new RijndaelManaged();
        rijndaelProvider.Key = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 32));
        rijndaelProvider.IV = AESKeys;
        ICryptoTransform rijndaelEncrypt = rijndaelProvider.CreateEncryptor();

        byte[] inputData = Encoding.UTF8.GetBytes(encryptString);
        byte[] encryptedData = rijndaelEncrypt.TransformFinalBlock(inputData, 0, inputData.Length);

        return Convert.ToBase64String(encryptedData);
    }


    /// <summary>
    /// MD5 加密
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string MD5(string str)
    {
        string cl = str;
        string pwd = "";
        System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();// 加密后是一个字节类型的数组 
        byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(cl));
        for (int i = 0; i < s.Length; i++)
        {// 通过使用循环，将字节类型的数组转换为字符串，此字符串是常规字符格式化所得 
            pwd = pwd + s[i].ToString("x").PadLeft(2, '0');// 将得到的字符串使用十六进制类型格式。格式后的字符是小写的字母，如果使用大写（X）则格式后的字符是大写字符 
        }
        return pwd;
    }
}
