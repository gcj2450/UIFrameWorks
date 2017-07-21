using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using System.Net;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine.UI;
/// <summary>
/// 多线程断点下载
/// ObjectGetter.Instance.DownloadAsync("http://o9tkm8ot9.bkt.clouddn.com/film02.mp4", setDownloadPercent);
/// </summary>
public class ObjectGetter : MonoBehaviour
{
    //下载文件保留字
    public static string PERSIST_EXP = ".cdel";
    public static string CachePath = Application.persistentDataPath + "/DownloadFileCache/";

    private static ObjectGetter _instance;
    /// <summary>
    /// Singleton,方便各模块访问
    /// </summary>
    public static ObjectGetter Instance
    {
        get
        {
            if (_instance == null)
            {
                var app = FindObjectOfType(typeof(ObjectGetter)) as ObjectGetter;
                if (app == null)
                {
                    var appObject = new GameObject("ObjectGetter");
                    app = appObject.AddComponent<ObjectGetter>();
                }
                _instance = app;
            }

            return _instance;
        }
    }
    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        if (!Directory.Exists(CachePath))
            Directory.CreateDirectory(CachePath);
        DontDestroyOnLoad(this);
    }

    /// <summary>协程分块下载网络文件
    /// </summary>
    /// <param name=”file_url”></param>
    /// <param name=”localSaveUrl”></param>
    public IEnumerator CoroutineBlockDownFile(string file_url)
    {
        if (string.IsNullOrEmpty(file_url))
        {
            yield return null;
        }
        //string ext_name = Path.GetExtension(file_url);
        string file_name = Path.GetFileName(file_url);
        if (!Directory.Exists(CachePath))
            Directory.CreateDirectory(CachePath);
        string localSaveUrl = CachePath + "/" +file_name;
        //获取远程文件的数据流
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(file_url);
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        Stream stream = response.GetResponseStream();
        long totalBytes = response.ContentLength;
        long totalDownloadBytes = 0;
        Debug.Log(totalBytes);
        FileStream fs = new FileStream(localSaveUrl, FileMode.Create);
        int bufferSize = 2048;
        byte[] bytes = new byte[bufferSize];
        float vv = 0;
        try
        {
            int length = stream.Read(bytes, 0, bufferSize);
            while (length > 0)
            {
                totalDownloadBytes += length;
                vv = totalDownloadBytes / totalBytes;
                Debug.Log("Download Percent : "+vv);
                fs.Write(bytes, 0, length);
                length = stream.Read(bytes, 0, bufferSize);
            }
            stream.Close();
            fs.Close();
            response.Close();
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message.ToString());
        }
    }

    /// <summary>
    /// 多线程异步下载文件，支持断点续传
    /// </summary>
    /// <param name="downloadUrl"></param>
    /// <param name="downloadCallBack"></param>
    public void DownloadAsync(string downloadUrl, Action<float,string>downloadCallBack)
    {
        Loom.RunAsync(() =>
        {
            simpleDownload(downloadUrl, downloadCallBack);
        });
    }
    
    //断点续传下载文件
    private void simpleDownload(string downloadUrl, Action<float, string> downloadCallBack)
    {
        string extension = Path.GetExtension(downloadUrl);
        //string fileName = FileExeUtil.MD5Encrypt(downloadUrl) + extension; //根据URL获取文件的名字
        string fileName = downloadUrl.Substring(downloadUrl.LastIndexOf("/") + 1, downloadUrl.Length -downloadUrl.LastIndexOf("/") -1);
        if(!Directory.Exists(CachePath))
            Directory.CreateDirectory(CachePath);
        string localPath = CachePath + fileName;
        Debug.Log(localPath);
        if (File.Exists(localPath))
        {
            Debug.Log("文件己存在！");
            Loom.QueueOnMainThread(() =>
            {
                if (downloadCallBack != null)
                    downloadCallBack(1, localPath);
            });
            return;
        }
        else
        {
            localPath = localPath + PERSIST_EXP; //临时文件路径
            HttpWebRequest request = getWebRequest(downloadUrl, 0);
            WebResponse response = null;
            FileStream writer = new FileStream(localPath, FileMode.OpenOrCreate, FileAccess.Write);
            long lStartPos = writer.Length;
            long currentLength = 0;
            long totalLength = 0;//总大小
            if (File.Exists(localPath))//断点续传
            {
                try
                {
                    response = request.GetResponse();
                }
                catch (Exception ex)
                {
                    Debug.Log(ex.Message);
                }
                if (response != null)
                {
                    long sTotal = response.ContentLength;
                    if (sTotal == lStartPos)
                    {
                        if (writer != null)
                        {
                            writer.Close();
                            writer.Dispose();
                        }
                        File.Move(localPath, localPath.Replace(PERSIST_EXP, ""));
                        Debug.Log("下载完成!");
                        Loom.QueueOnMainThread(() =>
                        {
                            if (downloadCallBack != null)
                                downloadCallBack(1, localPath);
                        });
                        return;
                    }
                    request = getWebRequest(downloadUrl, (int)lStartPos);
                    writer.Seek(lStartPos, SeekOrigin.Begin);
                    //response = request.GetResponse();
                    totalLength = response.ContentLength + lStartPos; //
                    currentLength = lStartPos; //
                }
            }
            else
            {
                try
                {
                    response = request.GetResponse();
                }
                catch (Exception ex)
                {
                    Debug.Log(ex.Message);
                }
                totalLength = response.ContentLength;
            }
            Stream reader = response.GetResponseStream();
            byte[] buff = new byte[1024];
            int c = 0; //实际读取的字节数
            while ((c = reader.Read(buff, 0, buff.Length)) > 0)
            {
                currentLength += c;
                writer.Write(buff, 0, c);
                float curL = currentLength;
                float totalL = totalLength;
                float progressPercent = (float)(curL / totalL);
                Loom.QueueOnMainThread(() =>
                {
                    if (downloadCallBack != null)
                        downloadCallBack(progressPercent, localPath);
                });
                writer.Flush();
            }
            if (writer != null)
            {
                writer.Close();
                writer.Dispose();
            }
            //close(writer);
            if (currentLength == totalLength)
            {
                File.Move(localPath, localPath.Replace(PERSIST_EXP, ""));
                Loom.QueueOnMainThread(() =>
                {
                    if (downloadCallBack != null)
                        downloadCallBack(1, localPath);
                });
                Debug.Log("下载完成!");
            }
            if (reader != null)
            {
                reader.Close();
                reader.Dispose();
                response.Close();
            }
        }
    }

    public static HttpWebRequest getWebRequest(string url, int lStartPos)
    {
        HttpWebRequest request = null;
        try
        {
            request = (System.Net.HttpWebRequest)HttpWebRequest.Create(url);
            request.AddRange(lStartPos); //设置Range值
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }
        return request;
    }
}
