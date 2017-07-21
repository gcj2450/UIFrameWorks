//=========================================================================================================
//Note: XML processcing,  can not save multiple-array!!!
//Date Created: 2012/04/17 by 风宇冲
//Date Modified: 2012/04/19 by 风宇冲
//=========================================================================================================
using UnityEngine;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using System;

public class XmlSaver
{	
	//内容加密
	public string Encrypt(string toE)
	{
		//加密和解密采用相同的key,具体自己填，但是必须为32位//
		byte[] keyArray = UTF8Encoding.UTF8.GetBytes("12348578906543367877723456789012");
		RijndaelManaged rDel = new RijndaelManaged();
		rDel.Key = keyArray;
		rDel.Mode = CipherMode.ECB;
		rDel.Padding = PaddingMode.PKCS7;
		ICryptoTransform cTransform = rDel.CreateEncryptor();
		
		byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toE);
		byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray,0,toEncryptArray.Length);
	
		return Convert.ToBase64String(resultArray,0,resultArray.Length);
	}
	
	//内容解密
	public string Decrypt(string toD)
	{
		//加密和解密采用相同的key,具体值自己填，但是必须为32位//
		byte[] keyArray = UTF8Encoding.UTF8.GetBytes("12348578906543367877723456789012");
		
		RijndaelManaged rDel = new RijndaelManaged();
		rDel.Key = keyArray;
		rDel.Mode = CipherMode.ECB;
		rDel.Padding = PaddingMode.PKCS7;
		ICryptoTransform cTransform = rDel.CreateDecryptor();
		
		byte[] toEncryptArray = Convert.FromBase64String(toD);
		byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray,0,toEncryptArray.Length);
		
		return UTF8Encoding.UTF8.GetString(resultArray);
	}
	
	public string SerializeObject(object pObject,System.Type ty)
	{
	   string XmlizedString = null;
	   MemoryStream memoryStream  = new MemoryStream();
	   XmlSerializer xs  = new XmlSerializer(ty); 
	   XmlTextWriter xmlTextWriter  = new XmlTextWriter(memoryStream, Encoding.UTF8);
	   xs.Serialize(xmlTextWriter, pObject);
	   memoryStream = (MemoryStream)xmlTextWriter.BaseStream;
       XmlizedString = UTF8ByteArrayToString(memoryStream.ToArray());
	   return XmlizedString;
	}
	
	public object DeserializeObject(string pXmlizedString , System.Type ty)
	{
	   XmlSerializer xs  = new XmlSerializer(ty);
	   MemoryStream memoryStream  = new MemoryStream(StringToUTF8ByteArray(pXmlizedString));
	   //XmlTextWriter xmlTextWriter   = new XmlTextWriter(memoryStream, Encoding.UTF8);
	   return xs.Deserialize(memoryStream);
	}
	
	//创建XML文件
	public void CreateXML(string fileName,string thisData)
	{
	   //string xxx = Encrypt(thisData);
        string xxx = thisData;
        StreamWriter writer;
        writer = File.CreateText(fileName);
        writer.Write(xxx);
        writer.Close();
	}

    //更新 XML文件
    public void UpdateXML(string fileName, string node, string nodeValue)
    {
        //LoadXML(fileName);
        //Debug.Log(LoadXML(fileName));

        XmlDocument doc = new XmlDocument();
        doc.Load(fileName);
        XmlNode root = doc.DocumentElement;
        XmlNode age = root.SelectSingleNode(node);
        age.InnerText = nodeValue;
        doc.Save(fileName);

        //XmlDocument xmlDoc = new XmlDocument();
        //xmlDoc.Load(fileName);//加载xml文件，文件
        //XmlNode xns = xmlDoc.SelectSingleNode("items");//查找要修改的节点

        //XmlNodeList xnl = xns.ChildNodes;//取出所有的子节点

        //foreach (XmlNode xn in xnl)
        //{
        //    XmlElement xe = (XmlElement)xn;//将节点转换一下类型
        //    if (xe.GetAttribute("类别") == "文学")//判断该子节点是否是要查找的节点
        //    {
        //        xe.SetAttribute("类别", "娱乐");//设置新值
        //    }
        //    else//为了有更明显的效果，所以不管是否是符合条件的子节点，我都给一个操作
        //    {
        //        xe.SetAttribute("类别", "文学");
        //    }

        //    XmlNodeList xnl2 = xe.ChildNodes;//取出该子节点下面的所有元素
        //    foreach (XmlNode xn2 in xnl2)
        //    {
        //        XmlElement xe2 = (XmlElement)xn2;//转换类型
        //        if (xe2.Name == "price")//判断是否是要查找的元素
        //        {
        //            if (xe2.InnerText == "10.00")//判断该元素的值并设置该元素的值
        //                xe2.InnerText = "15.00";
        //            else
        //                xe2.InnerText = "10.00";
        //        }
        //        //break;//这里为了明显效果 我注释了break,用的时候不用，这个大家都明白的哈
        //    }
        //    //break;
        //}
        //xmlDoc.Save(fileName);//再一次强调 ，一定要记得保存的该XML文件
    }
	
	//读取XML文件
	public string LoadXML(string fileName)
	{
	   StreamReader sReader = File.OpenText(fileName);
	   string dataString = sReader.ReadToEnd();
	   sReader.Close();
	   //string xxx = Decrypt(dataString);
       string xxx = dataString;
	   return xxx;
	}
	
	//判断是否存在文件
	public bool hasFile(String fileName)
	{
	   return File.Exists(fileName);
	}
	public string UTF8ByteArrayToString(byte[] characters  )
	{     
	   UTF8Encoding encoding  = new UTF8Encoding();
	   string constructedString  = encoding.GetString(characters);
	   return (constructedString);
	}
	
	public byte[] StringToUTF8ByteArray(String pXmlString )
	{
	   UTF8Encoding encoding  = new UTF8Encoding();
	   byte[] byteArray  = encoding.GetBytes(pXmlString);
	   return byteArray;
	}
}
