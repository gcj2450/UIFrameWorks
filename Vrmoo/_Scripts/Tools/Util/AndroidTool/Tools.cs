using UnityEngine;
using System.Collections;

public class Tools: MonoBehaviour {
    private Tools() { }
    private static Tools instance;
    public static Tools Instance {
        get {
            if (instance == null)
            {
                instance = new Tools();
            }
            return instance;
        
        }
    
    }
    /// <summary>
    /// 获取目标字符串首尾字符之间的字符数据
    /// </summary>
    /// <param name="target">目标数据</param>
    /// <param name="startSearchKey">起始搜索字符</param>
    /// <param name="endSearchKey">末搜索字符</param>
    /// <returns></returns>
    public string SearchKeyResult(string target,string startSearchKey,string endSearchKey) {
        int n1, n2;
        n1 = target.IndexOf(startSearchKey, 0) + startSearchKey.Length;   //开始位置  
        n2 = target.IndexOf(endSearchKey, n1);               //结束位置  
        return target.Substring(n1, n2 - n1);   //取搜索的条数，用结束的位置-开始的位置,并返回  
    }

    /// <summary>
    /// 根据指定分隔符
    /// </summary>
    /// <param name="target">目标字段</param>
    /// <param name="spliteKeyword">分隔符</param>
    /// <param name="isTop">是截取分隔符的前部分还是后部分</param>
    /// <param name="isFirstSplit">是否是首个分隔符</param>
    /// <returns>分割后的字段</returns>
    public string SearchKeyResult(string target, string spliteKeyword, bool isTop,bool isFirstSplit)
    {
       // print(target);
        int spliteLength = spliteKeyword.Length-1;
        int lastIndex = target.LastIndexOf(spliteKeyword) + 1 + spliteLength;
        int firstIndex = target.IndexOf(spliteKeyword) + 1 + spliteLength;
        string last = "";
        try
        {
            if (isTop)
            {

                if (!isFirstSplit)
                {
                    last = target.Substring(0, lastIndex - 1);
                }
                else
                {

                    last = target.Substring(0, firstIndex - 1);
                }
            }
            else
            {
                if (!isFirstSplit)
                {
                    last = target.Substring(lastIndex, target.Length - lastIndex);
                }
                else
                {
                    last = target.Substring(firstIndex, target.Length - firstIndex);
                }
            }
        }
        catch { 
        }
        //Debug.Log(last);
        return last;
    }
    /// <summary>
    /// 针对指定分割符提取目标字符串
    /// </summary>
    /// <param name="target">目标字符串</param>
    /// <param name="spliteKeyword">分割符</param>
    /// <returns>结果字符串</returns>
    public string SearchKeyResult(string target,string spliteKeyword) {
        int Index = target.IndexOf(spliteKeyword);
        string str = target.Substring(0, Index);
        return str;
    }

    /// <summary>
    /// 把格式为（0,0,0）的字符串转化为3维向量
    /// </summary>
    /// <param name="vec">被转换维3维向量的字符串</param>
    /// <returns></returns>
  public  Vector3 ToVector3(string vec)
    {
        if (vec.Equals(""))
        {
            return new Vector3(0,0,0);
        } 
        string z = Tools.Instance.SearchKeyResult(vec, ",", false, false);
        string xy = Tools.Instance.SearchKeyResult(vec, ",", true, false);

        string y = Tools.Instance.SearchKeyResult(xy, ",", false, false);
        string x = Tools.Instance.SearchKeyResult(xy, ",", true, false);

        return new Vector3(float.Parse(x.Trim()), float.Parse(y.Trim()), float.Parse(z.Trim()));
    }
}
