
using UnityEngine;
using System.Collections;
using System;
using System.Text;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;

/// <summary>
/// 工具类
/// </summary>
public class TexGetUtil : MonoBehaviour
{
	static TexGetUtil _Instance;

	void Awake()
	{
		//DontDestroyOnLoad(gameObject);
		_Instance = this;
	}

	public static TexGetUtil Instance
	{
		get{return _Instance;}
	}
	//下载图片
	//public delegate void GetTexture(Texture txt); 
	public void downloadPicture(string picName, System.Action<Texture> getTextureCallback)  
	{  

		Regex rx = new Regex("[*%\\?\"<>|*%、、：？“”《》｜*%、、：？“”《》｜]{1,}", RegexOptions.IgnoreCase);

		MatchCollection mc = rx.Matches(picName);
		if(mc.Count > 0)
		{
	
			   Debug.LogWarning("111" + mc.ToString());
			Debug.LogWarning("path error " + picName);
			return;
		}

		if(picName.Length <= 0)
		{
			Debug.LogWarning("downloadPicture error");
			return;
		}


		StartCoroutine(GETTexture(picName, getTextureCallback));  
	}

	IEnumerator GETTexture(string picURL, System.Action<Texture> getTextureCallback)  
	{  

		//WWW wwwTexture = WWW.LoadFromCacheOrDownload(picURL,1);
		WWW wwwTexture = new WWW(picURL);  
		//WWW.LoadFromCacheOrDownload();
		yield return wwwTexture;  
		
		if (wwwTexture.error != null)  
		{  
			//GET请求失败  
			//DebugHelper.Log("error :" + wwwTexture.error);  
			Debug.LogWarning("GETTexture error :" + picURL);
			getTextureCallback(null);
		}  
		else  
		{  
			//GET请求成功  
			getTextureCallback(wwwTexture.texture);
		}  
	} 

	
	/// <summary>
	/// 检测串值是否为合法的网址格式
	/// </summary>
	/// <param name="strValue">要检测的String值</param>
	/// <returns>成功返回true 失败返回false</returns>
	public static bool CheckIsUrlFormat(string strValue)
	{
		return CheckIsFormat(@"", strValue);
	}
	
	/// <summary>
	/// 检测串值是否为合法的格式
	/// </summary>
	/// <param name="strRegex">正则表达式</param>
	/// <param name="strValue">要检测的String值</param>
	/// <returns>成功返回true 失败返回false</returns>
	public static bool CheckIsFormat(string strRegex,string strValue)
	{
		if(strValue != null && strValue.Trim() != "")
		{				
			Regex re = new Regex(strRegex);
			if (re.IsMatch(strValue))
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		return false;
	}



	//public static void LabelWarp(UILabel uiLabel) 
	//{
	//	string strContent = uiLabel.text; // UILabel中显示的内容
	//	if(strContent.Length <= 0)
	//		return;

	//	string strOut = string.Empty;
	//	// 当前配置下的UILabel是否能够包围Text内容
	//	// Wrap是NGUI中自带的方法，其中strContent表示要在UILabel中显示的内容，strOur表示处理好后返回的字符串，uiLabel.height是字符串的高度 。
	//	bool bWarp = uiLabel.Wrap(strContent, out strOut, uiLabel.height);
	//	// 如果不能，就是说Text内容不能全部显示，这个时候，我们把最后一个字符去掉，换成省略号"..."
	//	if (!bWarp)
	//	{
	//		strOut = strOut.Substring(0, strOut.Length - 1);
	//		strOut += "...";
	//	}
	//	// 如果可以包围，就是说Text内容可以完全显示，这个时候，我们不做处理，直接显示内容。
	//	uiLabel.text = strOut;
	//}  

	public static long GetHttpLength(string url)
	{
		var length = 0L;
		try
		{
			var req = (HttpWebRequest)WebRequest.CreateDefault(new Uri(url));
			req.Method = "HEAD"; 
			req.Timeout = 5000; 
			var res = (HttpWebResponse)req.GetResponse();
			if (res.StatusCode == HttpStatusCode.OK)
			{
				length =  res.ContentLength;  
			}
			
			res.Close();
			return length;
		} 
		catch (WebException wex)
		{
            Debug.Log(wex.Message);
			return 0;
		}
	}
}


