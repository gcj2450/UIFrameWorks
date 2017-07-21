using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class Build
{
    [MenuItem("Haobel/CompressToPck")]
    public static void CompressToPck()
    {
        string targetPath = Application.dataPath.Substring(0, Application.dataPath.Length - 6);
        targetPath += "AssetBundles";
        ProcessCommand("compressRes.bat", AddQuotationToString(targetPath) + " " + AddQuotationToString(Application.streamingAssetsPath) + "/");
        Build_CopyToLocalResAndroid();
    }
    public static void Build_CopyToLocalResAndroid()
    {
        string dest = "AssetBundles/exp_local_res"; // default
        Build_CopyToLocalRes(dest);
    }

    private static void Build_CopyToLocalRes(
        string localResPath,
        List<string> filterExtension = null
    )
    {
        if (!Directory.Exists(localResPath))
        {
            Directory.CreateDirectory(localResPath);
        }
        FileToolkit.CopyAllFilesToDirectory("AssetBundles/Android", localResPath, filterExtension);
    }

    static void ProcessCommand(string commandName, string argument)
    {
        System.Diagnostics.ProcessStartInfo start = new System.Diagnostics.ProcessStartInfo(commandName);//设置运行的命令行文件问ping.exe文件，这个文件系统会自己找到
        //如果是其它exe文件，则有可能需要指定详细路径，如运行winRar.exe
        start.FileName = Application.dataPath + "\\Editor\\compressRes.bat";
        start.CreateNoWindow = false;
        start.Arguments = argument;//设置命令参数
        start.UseShellExecute = true;//是否指定操作系统外壳进程启动程序
        System.Diagnostics.Process p = System.Diagnostics.Process.Start(start);
        p.WaitForExit();//等待程序执行完退出进程
    }

    /// <summary>
    /// 将路径字符串首尾加上单引号
    /// </summary>
    /// <param name="arg"></param>
    /// <returns></returns>
    public static string AddQuotationToString(string arg)
    {
        return ("\"" + arg + "\"");
    }
}
