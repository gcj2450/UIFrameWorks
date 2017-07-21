using System.Collections;
using System.Collections.Generic;
using JsonFx.Json;
using System.IO;

public class JsonStringConverter
{

    /// <summary>Json字符串转类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <returns></returns>
    public T JsonStringToClass<T>(string value)
    {
        if (!string.IsNullOrEmpty(value))
        {
            //return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(value);
            return JsonFx.Json.JsonReader.Deserialize<T>(value);
        }
        else
        {
            return default(T);
        }
    }
    /// <summary>JSON字符串转类List
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="json"></param>
    /// <returns></returns>
	public static List<T> JsonStringToClassList<T>(string json) where T : class
    {
        List<T> tList = new List<T>();

        if (json != null && json.Length > 1)
        {
            T[] list = JsonReader.Deserialize<T[]>(json);
            foreach (T t in list)
            {
                tList.Add(t);
            }
        }
        return tList;
    }
    /// <summary>  Json字符串转类数组
    /// </summary>
    static public T[] JsonToClasseArray<T>(string json) where T : class
    {
        //Debug.Log(json);  
        T[] list = JsonReader.Deserialize<T[]>(json);
        return list;
    }
    /// <summary>Json文件转类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path">文件地址</param>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public static T JsonFileToClass<T>(string path)
    {
        var streamReader = new StreamReader(path);
        string data = streamReader.ReadToEnd();
        streamReader.Close();

        if (!string.IsNullOrEmpty(data))
        {
            //return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(value);
            return JsonFx.Json.JsonReader.Deserialize<T>(data);
        }
        else
        {
            return default(T);
        }
    }
    /// <summary>Json文件转类List
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public static List<T> JsonFileToClasseList<T>(string path)
    {
        var streamReader = new StreamReader(path);
        string data = streamReader.ReadToEnd();
        streamReader.Close();
        List<T> tList = new List<T>();
        if (!string.IsNullOrEmpty(data))
        {
            T[] list = JsonReader.Deserialize<T[]>(data);
            foreach (T t in list)
            {
                tList.Add(t);
            }
        }
        return tList;
    }
    /// <summary>Json文件转类数组
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public static T[] JsonFileToClasseArray<T>(string path)
    {
        var streamReader = new StreamReader(path);
        string data = streamReader.ReadToEnd();
        streamReader.Close();
        //List<T> tList = new List<T>();
        if (!string.IsNullOrEmpty(data))
        {
            T[] list = JsonReader.Deserialize<T[]>(data);
            return list;
        }
        else
        {
            return default(T[]);
        }
    }
    /// <summary>类转json字符串，并且保存到文件中
    /// </summary>
    /// <param name="path"></param>
    /// <param name="fileName"></param>
    /// <param name="_obj"></param>
    public static void ClassToJsonAndSave(string path, string fileName, object _obj)
    {
        string objToStr = ClassToJson(_obj);
        SaveString(path, fileName, objToStr);
    }
    /// <summary>保存字符串到本地的txt文本中
    /// </summary>
    /// <param name="path"></param>
    /// <param name="fileName"></param>
    /// <param name="_obj"></param>
    public static void SaveString(string path, string fileName, string _obj)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        var streamWriter = new StreamWriter(path + fileName);
        streamWriter.Write(_obj);
        streamWriter.Close();
    }
    /// <summary>类转json字符串
    /// </summary>
    /// <param name="ob"></param>
    /// <returns></returns>
    public static string ClassToJson(object ob)
    {
        return JsonFx.Json.JsonWriter.Serialize(ob);
    }

    //====参阅最好用的 unity3d Json数据传输，插件JsonFx ！！=========
    //http://blog.csdn.net/wangping1288888/article/details/9336247


    /// <summary>根据一个JSON，得到一个类
    /// </summary>  
    static public T JsonToClass<T>(string json) where T : class
    {
        T t = JsonReader.Deserialize<T>(json);
        return t;
    }


}