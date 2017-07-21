using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using System.Net;
using System;
using GameEntities;
using System.Collections.Generic;
using JsonFx.Json;
using System.Threading;
using UnityEngine.UI;
/// <summary>
/// 文件下载器，支持断点续传
/// 
/// </summary>
public class FileDownloader : MonoBehaviour
{
    public static string folderpath = "";

    public delegate void DownDel();
    //public static event DownDel DownloadOkEvt;

    public static FileDownloader Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    // Use this for initialization
    void Start()
    {
        folderpath = Application.persistentDataPath + "/DownloadedVideos";
        if (!Directory.Exists(folderpath))
        {
            Directory.CreateDirectory(folderpath);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    #region 获取图片
    public static IEnumerator LoadTexture(string path, Texture tex)
    {
        Debug.Log(Application.persistentDataPath);
        string dirPath = Application.persistentDataPath + "/DownloadedImgs";
        if (!Directory.Exists(dirPath))
            Directory.CreateDirectory(dirPath);

        if (!string.IsNullOrEmpty(path))
        {
            //			string imgFilePathSource = dirPath + "/" + getParam(path, "name");
            //			string pathLocalSource = "file:///" + imgFilePathSource;
            //Debug.Log(pathLocal);
            //string musicTitle = songPath.Substring(songPath.LastIndexOf("/") + 1, songPath.LastIndexOf(".") - songPath.LastIndexOf("/") - 1);
            Debug.Log(dirPath);
            string imgFilePath = dirPath + "/" + path.Substring(path.LastIndexOf("/") + 1, path.Length - path.LastIndexOf("/") - 1);//图片的本地缓存路径
            Debug.Log(path.Substring(path.LastIndexOf("/") + 1, path.Length - path.LastIndexOf("/") - 1));
            //Debug.Log(Path.GetFileName(path));
            //string imgFilePath = dirPath + "/" + Path.GetFileName(path);
            Debug.Log(imgFilePath);
            if (File.Exists(imgFilePath))
            {
                imgFilePath = "file:///" + imgFilePath;
                //Debug.Log("检测到本地有图————————");
                Resources.UnloadUnusedAssets();
                WWW wwwLoc = new WWW(imgFilePath);
                yield return wwwLoc;
                if (wwwLoc.error == null)
                {
                    //Debug.Log("图片下载完成 没有遇到错误————————");
                    tex = wwwLoc.texture;
                }
                else
                {
                    Debug.Log(imgFilePath);
                    Debug.Log(wwwLoc.error);
                }
            }
            else
            {
                Debug.Log(path);
                //Debug.Log("从网络获取图片");
                Resources.UnloadUnusedAssets();
                //WWWForm myForm = new WWWForm();
                //myForm.AddField("IsValid", "1");

                //WebRequest HWR = WebRequest.Create(path);
                //WebResponse HWRr = HWR.GetResponse();

                //Stream SR = HWRr.GetResponseStream();
                //string filename = path.Substring(path.LastIndexOf("/") + 1, path.Length - path.LastIndexOf("/") - 1);

                ////FileStream S = File.Open(Application.persistentDataPath + "/" + filename, FileMode.Create);
                //FileStream S = File.Open(imgFilePath, FileMode.Create);
                //byte[] by = new byte[1024];
                //int o = 1;
                //while (o > 0)
                //{
                //    o = SR.Read(by, 0, 1024);
                //    S.Write(by, 0, o);
                //}
                //S.Close();
                //SR.Close();
                //S.Dispose();
                //SR.Dispose();
                //HWR = null;
                //HWRr = null;

                WWW www = new WWW(path);
                yield return www;
                if (www.error == null)
                {
                    Debug.Log("下载图片完成");
                    tex = www.texture;
                    Texture2D _texLocal = www.texture;
                    byte[] pngData = _texLocal.EncodeToJPG();
                    Debug.Log(imgFilePath);
                    imgFilePath.Trim();
                    //imgFilePath = dirPath + "/" + "AA.jpg";
                    File.WriteAllBytes(imgFilePath, pngData);
                }
                else
                {
                    Debug.Log(path);
                    Debug.Log(www.error);
                    //Debug.Log("**********网络连接错误*********");
                }
            }
        }
    }

    public static IEnumerator LoadTexture(string path, RawImage tex)
    {
        //Debug.Log(Application.persistentDataPath);
        string dirPath = Application.persistentDataPath + "/DownloadedImgs";
        if (!Directory.Exists(dirPath))
            Directory.CreateDirectory(dirPath);

        if (!string.IsNullOrEmpty(path))
        {
            //			string imgFilePathSource = dirPath + "/" + getParam(path, "name");
            //			string pathLocalSource = "file:///" + imgFilePathSource;
            //Debug.Log(pathLocal);
            //string musicTitle = songPath.Substring(songPath.LastIndexOf("/") + 1, songPath.LastIndexOf(".") - songPath.LastIndexOf("/") - 1);
            //Debug.Log(dirPath);
            string imgFilePath = dirPath + "/" + path.Substring(path.LastIndexOf("/") + 1, path.Length - path.LastIndexOf("/") - 1);//图片的本地缓存路径
            //Debug.Log(path.Substring(path.LastIndexOf("/") + 1, path.Length - path.LastIndexOf("/") - 1));
            //Debug.Log(Path.GetFileName(path));
            //string imgFilePath = dirPath + "/" + Path.GetFileName(path);
            //Debug.Log(imgFilePath);
            if (File.Exists(imgFilePath))
            {
                imgFilePath = "file:///" + imgFilePath;
                //Debug.Log("检测到本地有图————————");
                Resources.UnloadUnusedAssets();
                WWW wwwLoc = new WWW(imgFilePath);
                yield return wwwLoc;
                if (wwwLoc.error == null)
                {
                    //Debug.Log("图片下载完成 没有遇到错误————————");
                    tex.texture = wwwLoc.texture;
                }
                else
                {
                    Debug.Log(imgFilePath);
                    Debug.Log(wwwLoc.error);
                }
            }
            else
            {
                Debug.Log(path);
                //Debug.Log("从网络获取图片");
                Resources.UnloadUnusedAssets();
                //WWWForm myForm = new WWWForm();
                //myForm.AddField("IsValid", "1");

                //WebRequest HWR = WebRequest.Create(path);
                //WebResponse HWRr = HWR.GetResponse();

                //Stream SR = HWRr.GetResponseStream();
                //string filename = path.Substring(path.LastIndexOf("/") + 1, path.Length - path.LastIndexOf("/") - 1);

                ////FileStream S = File.Open(Application.persistentDataPath + "/" + filename, FileMode.Create);
                //FileStream S = File.Open(imgFilePath, FileMode.Create);
                //byte[] by = new byte[1024];
                //int o = 1;
                //while (o > 0)
                //{
                //    o = SR.Read(by, 0, 1024);
                //    S.Write(by, 0, o);
                //}
                //S.Close();
                //SR.Close();
                //S.Dispose();
                //SR.Dispose();
                //HWR = null;
                //HWRr = null;

                WWW www = new WWW(path);
                yield return www;
                if (www.error == null)
                {
                    Debug.Log("下载图片完成");
                    tex.texture = www.texture;
                    Texture2D _texLocal = www.texture;
                    byte[] pngData = _texLocal.EncodeToJPG();
                    Debug.Log(imgFilePath);
                    imgFilePath.Trim();
                    //imgFilePath = dirPath + "/" + "AA.jpg";
                    File.WriteAllBytes(imgFilePath, pngData);
                }
                else
                {
                    Debug.Log(path);
                    Debug.Log(www.error);
                    //Debug.Log("**********网络连接错误*********");
                }
            }
        }
    }
    #endregion

    /// <summary>
    /// Download all audio in list 
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    public static IEnumerator DownLoadFile(string _path,string _name)
    {
        //isDownloading = true;
        Debug.Log(_path);
        WWW www = new WWW(_path);
        //check if url have a response (audio)
        if (www.error != null)
        {
            Debug.LogWarning(www.error);
            //DownloadText.text = www.error;

            yield return null;
        }
        //while downloading
        while (!www.isDone)
        {
            Debug.Log(0.2f + www.progress * 0.8f);
            //DownloadText.text = "Downloading " + title +" "+ (www.progress * 100).ToString("00") +"%..." ;
            //stop in bucle for update progress
            yield return null;
        }
        //create a new audio
        //when download is donw
        if (www.isDone || www.progress == 1)
        {

            //www.audioClip.GetType();
            File.WriteAllBytes(folderpath + "/" + _name + ".mp4", www.bytes);
            //AudioType.MPEG
            //c = www.audioClip;

            www.Dispose();
            www = null;
            Resources.UnloadUnusedAssets();
        }
    }

    /// <summary>
    /// Download all audio in list 
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    public IEnumerator DownLoadFile(string path, float _progress)
    {
        //isDownloading = true;
        WWW www = new WWW(path);
        //check if url have a response (audio)
        if (www.error != null)
        {
            Debug.LogWarning(www.error);
            //DownloadText.text = www.error;

            yield return null;
        }
        //while downloading
        while (!www.isDone)
        {
            _progress = www.progress;
            //DownloadText.text = "Downloading " + title +" "+ (www.progress * 100).ToString("00") +"%..." ;
            //stop in bucle for update progress
            yield return null;
        }
        //create a new audio
        //when download is donw
        if (www.isDone || www.progress == 1)
        {
            string folderpath = Application.persistentDataPath + "/DownloadedVideos";
            if (!Directory.Exists(folderpath))
            {
                Directory.CreateDirectory(folderpath);
            }
            string file_name = Path.GetFileName(path);
            //www.audioClip.GetType();
            File.WriteAllBytes(folderpath + "/" + file_name, www.bytes);
            //AudioType.MPEG
            //c = www.audioClip;
            www.Dispose();
            www = null;
            Resources.UnloadUnusedAssets();
        }

    }

    /// <summary>协程分块下载网络文件
    /// </summary>
    /// <param name="file_url"></param>
    /// <param name="localSaveUrl"></param>
    public IEnumerator CoroutineBlockDownFile(string file_url, float progressval)
    {
        if (string.IsNullOrEmpty(file_url))
        {
            yield return null;
        }
        //string ext_name = Path.GetExtension(file_url);
        string file_name = Path.GetFileName(file_url);
        string folderpath = Application.persistentDataPath + "/DownloadedVideos";
        if (!Directory.Exists(folderpath))
            Directory.CreateDirectory(folderpath);

        string localSaveUrl = folderpath + "/" + file_name;
        //组织存储路径和存储文件名
        //string up_folder = System.Configuration.ConfigurationManager.AppSettings["hj_up_img"].ToString();
        //up_folder = up_folder + HJ_DAL.ImgFolder._cls_space;
        //string time_span = HJ_DAL.ImgFolder.GetTimeStamp();

        //获取远程文件的数据流
        FileStream fs = new FileStream(localSaveUrl, FileMode.Create);
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(file_url);
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        Stream stream = response.GetResponseStream();

        long totalBytes = response.ContentLength;
        long totalDownloadBytes = 0;
        Debug.Log(totalBytes);
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
                Debug.Log(vv);
                progressval = vv;
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
    /// 使用网络请求分块下载，不支持断点续传
    /// </summary>
    /// <param name="file_url"></param>
    public static void BlockDownFile(string file_url)
    {
        if (string.IsNullOrEmpty(file_url))
        {
            return;
        }
        //string ext_name = Path.GetExtension(file_url);
        //string file_name = Path.GetFileName(file_url);
        Debug.Log("AAAAAAAAA");
        string file_name = file_url.Substring(file_url.LastIndexOf("/") + 1, file_url.Length - file_url.LastIndexOf("/") - 1);


        string folderpath = Application.persistentDataPath + "/DownloadedVideos";
        if (!Directory.Exists(folderpath))
            Directory.CreateDirectory(folderpath);

        string localSaveUrl = folderpath + "/" + file_name;
        //组织存储路径和存储文件名
        //string up_folder = System.Configuration.ConfigurationManager.AppSettings["hj_up_img"].ToString();
        //up_folder = up_folder + HJ_DAL.ImgFolder._cls_space;
        //string time_span = HJ_DAL.ImgFolder.GetTimeStamp();

        //获取远程文件的数据流
        FileStream fs = new FileStream(localSaveUrl, FileMode.Create);
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(file_url);
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        Stream stream = response.GetResponseStream();

        long totalBytes = response.ContentLength;
        long totalDownloadBytes = 0;

        int bufferSize = 2048;
        byte[] bytes = new byte[bufferSize];

        try
        {
            int length = stream.Read(bytes, 0, bufferSize);

            while (length > 0)
            {
                totalDownloadBytes += length;
                Debug.Log(totalDownloadBytes / totalBytes);

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
            return;
        }
    }

    private string getWebContent(string url)
    {
        try
        {
            StringBuilder sb = new StringBuilder("");
            WebRequest request = WebRequest.Create(url);
            request.Timeout = 10000;//10秒请求超时
            //StreamReader sr = new StreamReader(request.GetResponse().GetResponseStream(), Encoding.UTF8);Encoding.GetEncoding("GB2312")
            StreamReader sr = new StreamReader(request.GetResponse().GetResponseStream(), Encoding.UTF8);
            while (sr.Peek() >= 0)
            {
                sb.Append(sr.ReadLine());
                sb.Append("\n");
            }
            return sb.ToString();
        }
        catch (WebException ex)
        {
            Debug.Log(ex.Message);
            return null;
        }

    }


    //下载文件保留字
    public static string PERSIST_EXP = ".cdel";

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

    public delegate void progressDel(float val);
    //public event progressDel progressevt;

    /// <summary>
    /// 断点续传下载
    /// 使用示例：
    /// UnityThreading.ActionThread myThread=UnityThreadHelper.CreateThreadAction(Download);
    /// void(Download)
    /// {
    ///     simpleDownload(string _url,string _name, out float progressVal)
    /// }
    /// </summary>
    /// <param name="_url"></param>
    /// <param name="_name"></param>
    /// <param name="progressVal"></param>
    public void simpleDownload(string _url,string _name, out float progressVal)
    {
        string path = folderpath + "/" + _name + ".mp4";
        if (File.Exists(path))
        {
            Debug.Log("文件己存在！");
            progressVal = 1;
            return;
        }
        else
        {
            path = path + PERSIST_EXP;
            HttpWebRequest request = getWebRequest(_url, 0);
            WebResponse response = null;
            FileStream writer = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
            long lStartPos = writer.Length; ;//当前文件大小
            long currentLength = 0;
            long totalLength = 0;//总大小
            if (File.Exists(path))//断点续传
            {
                try
                {
                    response = request.GetResponse();
                }
                catch (Exception ex)
                {
                    progressVal = 0;
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
                        File.Move(path, path.Replace(PERSIST_EXP, ""));
                        print("下载完成!");
                        
                        progressVal = 1;
                        return;
                    }
                    request = getWebRequest(_url, (int)lStartPos);
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
                //if(progressevt!=null)
                //    progressevt(progressPercent);
                progressVal = progressPercent;
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
                File.Move(path, path.Replace(PERSIST_EXP, ""));

                //if (DownloadOkEvt != null)
                //    DownloadOkEvt();
                Debug.Log("下载完成!");
            }


            if (reader != null)
            {
                reader.Close();
                reader.Dispose();
                response.Close();
            }
            progressVal = 1;
        }
    }

}
