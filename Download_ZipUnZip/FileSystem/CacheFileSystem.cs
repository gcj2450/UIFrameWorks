using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Collections.Generic;

public class CacheFileSystem
{
	private string _cachePath = "";
    public string cachePath
    {
        get
        {
            return (_cachePath);
        }
    }

    public CacheFileSystem()
    {
        Init();
    }

    //////////////////////////////////////
    // function

    // init
    public  void    Init()
    {
            _cachePath = Application.persistentDataPath;
    }

    public  string  CacheFilePath(
        string  cacheType = ""    
    )
    {
        _cachePath = Application.persistentDataPath + "/cache/" + cacheType + "/";
        if (!Directory.Exists(_cachePath))
        {
            Directory.CreateDirectory(_cachePath);
        }

        return (_cachePath);
    }

    // has file
    public  bool    Exists(
        string  filename    
    )
    {
        return (File.Exists(_cachePath + filename));
    }

    // write file at cache path
    public  bool    WriteToCacheFile(
        string  filename,
        string  fileContent
    )
    {
        return (WriteFile(_cachePath + filename, fileContent));
    }

    // write file at cache path
    public  bool    WriteToCacheFile(
        string  cacheType,
        string  filename,
        string  fileContent
    )
    {
		CacheFilePath(cacheType);
        string path = Application.persistentDataPath + "/cache/" + cacheType + "/" + filename;
        return (WriteFile(path, fileContent));
    }

    // write file by absolution path
    public  bool    WriteFile(
        string  filePath,
        string  fileContent
    )
    {
        try
        {
            StreamWriter sw = new StreamWriter(filePath, false, System.Text.Encoding.UTF8);
            sw.Write(fileContent);
            sw.Close();

            return true;
        }
        catch (Exception e)
        {
            Debug.Log(e.StackTrace);
            return false;
        }
    }

    // delete file
    public  void    DeleteFile(
        string  filename    
    )
    {
        string  path = _cachePath + filename;
        if (File.Exists(path))
            File.Delete(path);
    }

    // clean path
    public  bool    CleanCachePath(
        string  cacheType    
    )
    {
        if (String.IsNullOrEmpty(cacheType))
            return false;

        try
        {
            string path = Application.persistentDataPath + "/cache/" + cacheType + "/";
            string[] filesInDir = Directory.GetFiles(path);
            foreach (string filePath in filesInDir)
            {
                File.Delete(filePath);
            }
            return true;
        }
        catch (Exception e)
        {
            Debug.Log(e.StackTrace);
            return false;
        }
    }

		// clean path
		public  bool    CleanCacheFiles(
			string  cacheType, List<string>  cacheFilterFileNameList    
			)
		{
			if (cacheFilterFileNameList == null)
				return false;
			
			try
			{
				string path = Application.persistentDataPath + "/cache/" + cacheType + "/";
				string[] filesInDir = Directory.GetFiles(path);
				foreach (string filePath in filesInDir)
				{
					string fileName = Path.GetFileNameWithoutExtension(filePath);
					//UnityEngine.Debug.LogError(fileName);
					if(cacheFilterFileNameList.Contains(fileName))
					{
						File.Delete(filePath);
					}
				}
				return true;
			}
			catch (Exception e)
			{
				Debug.Log(e.StackTrace);
				return false;
			}
		}

        public string[] ReadLinesFromCacheFile(string filename)
        {
            string path = _cachePath + filename;

            return (ReadFileLines(path));
        }

        // read file at cache path
        public  string  ReadFromCacheFile(
        string  filename    
    )
    {
        string path = _cachePath + filename;

        return (ReadFile(path));
    }

    // read file at cache path
    public  string  ReadFromCacheFile(
        string  cacheType,
        string  filename
    )
    {
        string path = Application.persistentDataPath + "/cache/" + cacheType + "/" + filename;
		
		if(Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
		{
			string cahcePath = Application.persistentDataPath + "/cache/" + cacheType;
			if(!Directory.Exists(cahcePath))
			{
				Directory.CreateDirectory(cahcePath);
			}
			// temp add by guonan, some file not copy to cacheType fouder
			if(!File.Exists(path))
			{
				string srcPath = Path.Combine(Application.persistentDataPath, "res") + "/" + filename;

				if(File.Exists(srcPath))
					File.Copy(srcPath, path);
				else
					UnityEngine.Debug.LogError("Path not exist : " + srcPath);
			}
		}
		CacheFilePath(cacheType);

        return (ReadFile(path));
    }

        public string[] ReadFileLines(string filePath)
        {
            if (String.IsNullOrEmpty(filePath))
            {
                UnityEngine.Debug.LogError("filePath is null : " + filePath);
                return null;
            }
            if (!File.Exists(filePath))
            {
                return null;
            }

            return File.ReadAllLines(filePath);
        }
        // read file by absolution path
        public  string  ReadFile(string  filePath)
    {
        if (String.IsNullOrEmpty(filePath))
		{
			UnityEngine.Debug.LogError("filePath is null : " + filePath);
			return null;
		}
        if (!File.Exists(filePath))
		{
//			UnityEngine.Debug.LogError("filePath not exist : " + filePath);
    		return null;
		}

            //	HDebug.LogError(filePath);
        string vo = File.ReadAllText(filePath);

        return vo;
    }
}