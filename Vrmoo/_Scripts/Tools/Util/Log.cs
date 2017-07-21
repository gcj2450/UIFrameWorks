#define SHOW_LOG

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * 日志输出类
 * 发布游戏时请屏蔽SHOW_LOG宏
 * */
public class Log : MonoBehaviour {	
	
	private static ArrayList logList = new ArrayList();					// 存日志数据的列表
	private static int logFontSize = 18;								// 字体宽高(设置布局用)
	private static int maxRow = Screen.height / logFontSize;			// 日志行数(自动计算)
	private static Color [] logColor = new Color[LogType_Size] {Color.white,Color.green,Color.yellow,Color.red};
	
	//日志类型
	private const int LogType_Normal = 0;
	private const int LogType_Debug = 1;
	private const int LogType_Warning = 2;
	private const int LogType_Error = 3;
	private const int LogType_Size = 4;	
	
	//日志结构
	public class LogInfo
	{
		public LogInfo(string content,int type)
		{
			strContent = content;
			logType = type;
		}
		public string 	strContent;
		public int 		logType;
	};
	
	void OnGUI()
	{
#if SHOW_LOG
		LogInfo info;
		for (int i = 0; i < logList.Count; i++) 
		{ 
			info = logList[i] as LogInfo;
			GUIStyle style = new GUIStyle();
			style.normal.background = null;
            style.fontSize = logFontSize;
			style.normal.textColor = logColor[info.logType];
			GUI.Label(new Rect(0,logFontSize*i,info.strContent.Length*logFontSize,logFontSize),info.strContent,style);
		}
#endif
	}
	
	//输出调试内容
	public static void D(string str)
	{
		if(null == str)
		{
			return;
		}
#if SHOW_LOG
		if(logList.Count >= maxRow)
		{
			logList.RemoveAt(0);
		}
		logList.Add(new LogInfo(str,LogType_Debug));
#endif
	}

    //输出警告内容
	public static void W(string str)
	{
		if(null == str)
		{
			return;
		}
#if SHOW_LOG
		if(logList.Count >= maxRow)
		{
			logList.RemoveAt(0);
		}
		logList.Add(new LogInfo(str,LogType_Warning));
#endif
	}

    //输出错误内容
	public static void E(string str)
	{
		if(null == str)
		{
			return;
		}
#if SHOW_LOG
		if(logList.Count >= maxRow)
		{
			logList.RemoveAt(0);
		}
		logList.Add(new LogInfo(str,LogType_Error));
#endif
	}
	
	//保留原来接口
	public static void Clear()
	{
		logList.Clear();
	}
}
