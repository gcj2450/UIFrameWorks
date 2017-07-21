//using System.Threading;
//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;
//using System;
//using System.IO;
//using System.Runtime.Serialization.Formatters.Binary;

///// <summary>
///// 网络请求管理类
///// </summary>
//public class RequestManager : MonoBehaviour
//{
//    private static RequestManager _instance;
//    public static RequestManager Instance
//    {
//        get
//        {
//            if (_instance == null)
//            {
//                _instance = FindObjectOfType(typeof(RequestManager)) as RequestManager;
//                if (_instance == null)
//                {
//                    var requestManager = new GameObject("RequestManager");
//                    _instance = requestManager.AddComponent<RequestManager>();
//                    _instance.Initialize("Vrmoo");
//                }
//            }

//            return _instance;
//        }
//    }

//    private List<RequestData> _requestList = new List<RequestData>();
//    private const int MaxRequestThread = 2;
//    private const int MaxRetryTimes = 2;
//    private int _currentRequestNumber;

//    private string CacheFolder
//    {
//        get
//        {
//            return string.Format("{0}/RequestCache/{1}", _persistentDataPath, _cacheFolder);
//        }
//    }

//    private string CacheFile
//    {
//        get
//        {
//            return string.Format("{0}/Request.dat", CacheFolder);
//        }
//    }

//    private string _cacheFolder = "";
//    private string _persistentDataPath;
//    private Dictionary<string, string> _cacheDictionary;

//    void Awake()
//    {
        
//    }
//    void Demo()
//    {
//        RequestManager.Instance.Initialize("OnlineVideo");  //初始化RequestManager，并设置缓存位置
//        Action<RequestData, string> callback = jsonObjectCallback;
//        RequestData requestData;
//        requestData = new RequestData("http://img.static.mojing.cn/resource/onlyhuawei/olhw_1.js", callback, true);
//        RequestManager.Instance.AddRequest(requestData);
//    }

//    private void jsonObjectCallback(RequestData requestData, string jsonObjects)
//    {
//        string curUrl = "当初请求的地址";
//        if (!requestData.Url.Equals(curUrl))    //验证一下地址，这一步可以不做
//        {

//        }
//        //解析jsonObjects，发给需要的接口

//    }

//    public void Initialize(string cacheFolder)
//    {
//        _requestList = new List<RequestData>();
//        _cacheFolder = cacheFolder;
//        _persistentDataPath = Application.persistentDataPath;
//        LoadCache();
//    }

//    public void AddRequest(RequestData request)
//    {
//        if(request == null) return;
//        if (request.UseCache && request.RetryTimes == 0)
//        {
//            string cacheResult = GetFromCache(request.Url);
//            if (!string.IsNullOrEmpty(cacheResult))
//            {
//                request.IsLocal = true;
//                request.Callback(request, cacheResult);
//            }
//        }

//        _requestList.Add(request);
//        DoRequest();
//    }

//    private void DoRequest()
//    {
//        if (_requestList.Count > 0 && _currentRequestNumber < MaxRequestThread)
//        {
//            _currentRequestNumber++;
//            RequestData curData = _requestList[0];
//            StartCoroutine(DoRequestAsync(curData));
//            _requestList.RemoveAt(0);
//        }
//    }

//    private IEnumerator DoRequestAsync(RequestData request)
//    {
//        var www = new WWW(request.Url);
//        yield return www;

//        _currentRequestNumber--;
//        if (www.error == null)
//        {
//            request.IsLocal = false;
//            request.Callback(request, www.text);
//            if (request.UseCache)
//            {
//                SaveIntoCache(request.Url, www.text);
//            }
//            DoRequest();
//        }
//        else
//        {
//            if (request.RetryTimes < MaxRetryTimes)
//            {
//                request.RetryTimes++;
//                AddRequest(request);
//            }
//            else
//            {
//                if (GetFromCache(request.Url) == null)
//                {
//                    request.IsLocal = false;
//                    request.Callback(request, null);
//                }
//                DoRequest();
//            }
//        }
//    }

//    private void SaveIntoCache(string url, string text)
//    {
//        if (string.IsNullOrEmpty(text)) return;
//        if (_cacheDictionary.ContainsKey(url))
//        {
//            _cacheDictionary[url] = text;
//        }
//        else
//        {
//            _cacheDictionary.Add(url, text);
//        }
//        ThreadPool.QueueUserWorkItem(p => SaveCache());
//    }

//    public string GetFromCache(string url)
//    {
//        if (_cacheDictionary.ContainsKey(url))
//        {
//            return _cacheDictionary[url];
//        }

//        return null;
//    }

//    private void LoadCache()
//    {
//        lock (_instance)
//        {
//            var cacheFile = CacheFile;
//            if (File.Exists(cacheFile))
//            {
//                FileStream readstream = null;
//                try
//                {
//                    readstream = new FileStream(cacheFile, FileMode.Open, FileAccess.Read, FileShare.Read);
//                    var formatter = new BinaryFormatter();
//                    _cacheDictionary = (Dictionary<string, string>) formatter.Deserialize(readstream);
//                }
//                catch (Exception ex)
//                {
//                    Debug.Log(string.Format("load cache exception:{0}-----------ex:{1}", cacheFile, ex.Message));
//                    _cacheDictionary = new Dictionary<string, string>();
//                }
//                finally
//                {
//                    if (readstream != null)
//                    {
//                        readstream.Close();
//                    }
//                }
//            }
//            else
//            {
//                _cacheDictionary = new Dictionary<string, string>();
//            }
//        }
//    }

//    private void SaveCache()
//    {
//        lock (_instance)
//        {
//            var cacheFile = CacheFile;
//            if (!Directory.Exists(CacheFolder))
//            {
//                Directory.CreateDirectory(CacheFolder);
//            }

//            FileStream fileStream = null;
//            try
//            {
//                if (File.Exists(cacheFile))
//                {
//                    File.Delete(cacheFile);
//                }
//                fileStream = new FileStream(cacheFile, FileMode.Create, FileAccess.Write, FileShare.None);
//                var formatter = new BinaryFormatter();
//                formatter.Serialize(fileStream, _cacheDictionary);
//            }
//            catch (Exception ex)
//            {
//                Debug.Log(string.Format("save cache exception:{0}-----------ex:{1}", cacheFile, ex.Message));
//            }
//            finally
//            {
//                if (fileStream != null)
//                {
//                    fileStream.Close();
//                }
//            }
//        }
//    }
//}

//public class RequestData
//{
//    public string Url { get;  set; }
//    public Action<RequestData, string> Callback { get; private set; }
//    public bool UseCache { get; private set; }
//    public int RetryTimes { get; set; }
//    public object Parameter { get; set; }
//    /// <summary>
//    /// 是否是本地数据
//    /// </summary>
//    public bool IsLocal { get; set; }

//    public RequestData(string url, Action<RequestData, string> callback, bool useCache = true)
//    {
//        Url = url;
//        Callback = callback;
//        UseCache = useCache;
//    }
//}