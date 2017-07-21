using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class FileToolkit
{
    // copy all files to target directory
    public static void CopyAllFilesToDirectory(string srcPath, string outPath, List<string> filter = null)
    {
        if (srcPath == null
            || string.IsNullOrEmpty(srcPath)
            || outPath == null
            || string.IsNullOrEmpty(outPath))
        {
            // throw an expection?
            return;
        }


        string[] files = Directory.GetFiles(srcPath);
        foreach (string file in files)
        {
            if (filter != null)
            {
                string ext = Path.GetExtension(file);
                if (!filter.Contains(ext))
                    continue;
            }
            string destFile = Path.Combine(outPath, Path.GetFileName(file));
            File.Copy(file, destFile, true);
        }

        // recursion
        string[] folders = Directory.GetDirectories(srcPath);
        foreach (string folder in folders)
        {
            CopyAllFilesToDirectory(folder, outPath, filter);
        }
    }

    // copy all files to target directory
    public static void CopyAllFilesWithoutMetaToDirectory(string srcPath, string outPath)
    {
        if (srcPath == null
            || string.IsNullOrEmpty(srcPath)
            || outPath == null
            || string.IsNullOrEmpty(outPath))
        {
            // throw an expection?
            return;
        }


        string[] files = Directory.GetFiles(srcPath);
        foreach (string file in files)
        {
            if (file.Contains(".meta"))
                continue;
            string destFile = Path.Combine(outPath, Path.GetFileName(file));
            File.Copy(file, destFile, true);
        }

        // recursion
        string[] folders = Directory.GetDirectories(srcPath);
        foreach (string folder in folders)
        {
            CopyAllFilesWithoutMetaToDirectory(folder, outPath);
        }
    }

    // remove directory
    public static void RemoveDirectory(string path, bool recursion = false)
    {
        if (Directory.Exists(path))
        {
            Directory.Delete(path, recursion);
        }
    }

    // remove directory
    public static void RemoveDirectoryFiles(string path, bool recursion = false)
    {
        if (Directory.Exists(path))
        {
            string[] files = Directory.GetFiles(path);
            foreach (string file in files)
            {
                File.Delete(file);
            }

            // recursion
            if (recursion)
            {
                string[] folders = Directory.GetDirectories(path);
                foreach (string folder in folders)
                {
                    RemoveDirectoryFiles(folder, recursion);
                }
            }

            // delete itself
            Directory.Delete(path);
        }
    }

    public static void GetDirectoryFilesRecursive(string path, ref List<string> dirFiles)
    {
        if (dirFiles == null)
        {
            UnityEngine.Debug.LogError("dirFiles need init outside this method");
            return;
        }
        if (Directory.Exists(path))
        {
            string[] files = Directory.GetFiles(path);
            dirFiles.AddRange(files);
        }

        string[] dirs = Directory.GetDirectories(path);
        if (dirs != null && dirs.Length > 0)
        {
            for (int i = 0; i < dirs.Length; i++)
            {
                GetDirectoryFilesRecursive(dirs[i], ref dirFiles);
            }
        }
    }

}
