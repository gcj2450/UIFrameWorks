using UnityEngine;
using System.Collections;
using System.Linq;
using System.Security.AccessControl;
using System.IO; 

public class AndroidUtility  
{
    /// <summary>
    /// 获取内置SD卡PATH
    /// </summary>
    /// <returns></returns>
    public static string GetStoragePath()
    {
        if (Application.platform == RuntimePlatform.Android)
            return new AndroidJavaClass("android.os.Environment").CallStatic<AndroidJavaObject>("getExternalStorageDirectory").Call<string>("getPath");
        else
            return "d:/movie";
    }

    /// <summary>
    /// 获取所有SD卡PATH
    /// </summary>
    /// <returns></returns>
    public static string[] GetStoragePaths()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            var storagemanager = AndroidActivity.Current().Call<AndroidJavaObject>("getSystemService", "storage");
            var paths = storagemanager.Call<string[]>("getVolumePaths");
            return paths.Where(IsFolderCanAccess).ToArray();
        }
        else
            return new string[] {"d:/movie"};
    }

    public static bool IsFolderCanAccess(string foldername)
    {
        if (string.IsNullOrEmpty(foldername))
            return false;
        var fdir = new AndroidJavaObject("java.io.File", foldername);
        return fdir.Call<bool>("canWrite") && fdir.Call<bool>("canRead");
    }

    /// <summary>
    /// 从所有sd卡获取PATH
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string GetFilePath(string path)
    {
        var Storages = GetStoragePaths();
        for (int i = 0; i < Storages.Length; i++)
        {
            string filepath = Storages[i] + path;
            var fi = new FileInfo(filepath);
            if (fi.Exists)
                return filepath;
        }
        return null;
    }

    /// <summary>
    /// UI线程中运行
    /// </summary>
    /// <param name="r"></param>
    public static void RunOnUIThread(AndroidJavaRunnable r)
    {
        var activity = AndroidActivity.Current();
        activity.Call("runOnUiThread", r);
    }

    /// <summary>
    /// 获取Android自动数据库的值
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static AndroidJavaObject getSharedPreferences(string key)
    {
        return AndroidActivity.Current().Call<AndroidJavaObject>("getSharedPreferences", key, 0);
    }

    /// <summary>
    /// 获取包名
    /// </summary>
    /// <returns></returns>
    public static string getPackageName()
    {
        return AndroidActivity.Current().Call<string>("getPackageName");
    }

    /// <summary>
    /// 不自动锁屏
    /// </summary>
    public static void DisableScreenLock()
    {
        AndroidActivity.Current().Call<AndroidJavaObject>("getWindow")
            .Call("addFlags",128);
    }


}
