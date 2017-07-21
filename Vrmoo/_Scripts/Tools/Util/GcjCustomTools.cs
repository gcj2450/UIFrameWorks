using UnityEngine;
using System.Collections;
using System;
using System.IO;

public class GcjCustomTools{

    #region 英文与数字转换
    /// <summary>
    /// 获取英文字母
    /// </summary>
    /// <param name="digitalNumber">数字</param>
    /// <returns></returns>
    public static string  getEnglishFromDigital(long digitalNumber)
    {
        string retEnglishNumber = "";

        if (digitalNumber <= 0)
        {
            retEnglishNumber = "Z";
        }
        else
        {
            long divisorNumber = digitalNumber;
            long modNumber = 0;

            while (divisorNumber > 0)
            {
                modNumber = divisorNumber % 10;
                retEnglishNumber = getSingleEnglishFromDigital(modNumber) + retEnglishNumber;
                divisorNumber = divisorNumber / 10;
            }
        }

        return retEnglishNumber;
    }

    /// <summary>
    /// 获取数字
    /// </summary>
    /// <param name="englishStr">英文字母</param>
    /// <returns></returns>
    public static long getDigitalFromEnglish(string englishStr)
    {
        long retDigitalNumber = 0;

        string tempEnglishStr = englishStr;

        while (!string.IsNullOrEmpty(tempEnglishStr))
        {
            string singleStr = tempEnglishStr.Substring(0, 1);

            long tempValue = 0;
            long.TryParse(Math.Pow(10, tempEnglishStr.Length - 1).ToString(), out tempValue);

            retDigitalNumber += getSingleDigitalFromEnglish(singleStr) * tempValue;

            tempEnglishStr = tempEnglishStr.Substring(1);
        }

        return retDigitalNumber;
    }
    #endregion

    #region 单个英文与数字转换
    /// <summary>
    /// 获取单个英文字母
    /// </summary>
    /// <param name="digitalNumber">单个数字</param>
    /// <returns></returns>
    public static string getSingleEnglishFromDigital(long digitalNumber)
    {
        string retEnglishNumber = "";
        switch (digitalNumber)
        {
            case 0:
                retEnglishNumber = "Z";
                break;
            case 1:
                retEnglishNumber = "A";
                break;
            case 2:
                retEnglishNumber = "B";
                break;
            case 3:
                retEnglishNumber = "C";
                break;
            case 4:
                retEnglishNumber = "D";
                break;
            case 5:
                retEnglishNumber = "E";
                break;
            case 6:
                retEnglishNumber = "F";
                break;
            case 7:
                retEnglishNumber = "G";
                break;
            case 8:
                retEnglishNumber = "H";
                break;
            case 9:
                retEnglishNumber = "I";
                break;
            default:
                break;
        }
        return retEnglishNumber;
    }

    /// <summary>
    /// 获取单个数字
    /// </summary>
    /// <param name="englishStr">单个英文字母</param>
    /// <returns></returns>
    public static long getSingleDigitalFromEnglish(string englishStr)
    {
        long retDigitalNumber = 0;
        switch (englishStr)
        {
            case "Z":
                retDigitalNumber = 0;
                break;
            case "A":
                retDigitalNumber = 1;
                break;
            case "B":
                retDigitalNumber = 2;
                break;
            case "C":
                retDigitalNumber = 3;
                break;
            case "D":
                retDigitalNumber = 4;
                break;
            case "E":
                retDigitalNumber = 5;
                break;
            case "F":
                retDigitalNumber = 6;
                break;
            case "G":
                retDigitalNumber = 7;
                break;
            case "H":
                retDigitalNumber = 8;
                break;
            case "I":
                retDigitalNumber = 9;
                break;
            default:
                break;
        }
        return retDigitalNumber;
    }
    #endregion 
    /// <summary>
    /// 
    /// </summary>
    /// <param name="path"></param>
    /// <param name="tex"></param>
    /// <returns></returns>
    public static IEnumerator LoadTexture(string path, Texture2D tex)
    {
        WWW www = new WWW(path);
        yield return www;
        if (www.error == null)
        {
            tex = www.texture;
            Texture2D _texLocal = www.texture;
            byte[] pngData = _texLocal.EncodeToPNG();
            File.WriteAllBytes(Application.persistentDataPath + "/" + path.Substring(path.Length - 10, 5) + ".png", pngData);
        }
        else
        {
            Debug.Log(path);
            Debug.Log(www.error);
        }
    }

    //将本地图片转换成二进制保存起来
    public static byte[] SetImageToByteArray(string fileName)
    {
        FileStream fs = null;
        try
        {
            fs = new FileStream(fileName, FileMode.Open, System.IO.FileAccess.Read, FileShare.ReadWrite);
            //Texture2D _tex = new Texture2D();
            
            int streamLength = (int)fs.Length;
            byte[] image = new byte[streamLength];
            fs.Read(image, 0, streamLength);

            return image;
        }
        catch (Exception)
        {

            throw;

        }
        finally
        {

            fs.Close();
        }
    }
}
