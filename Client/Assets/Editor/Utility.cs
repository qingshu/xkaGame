/************************************************************************************
* Copyright(c) 2019 All Rights Reserved.
*创建人： lyp
*创建时间：2019-03-11 17:55:34
*版本：1.0
*设计意图：工具类
************************************************************************************/

using UnityEngine;
using System.Collections;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System;
using System.Linq;
using System.Diagnostics;

/// <summary>
/// 公用工具
/// </summary>
public class Utility
{

    /// <summary>
    /// 获取文件MD5值
    /// </summary>
    /// <param name="fileName">文件绝对路径</param>
    /// <returns>MD5值</returns>
    public static string GetMD5HashFromFile(string file)
    {
        try
        {
            FileStream fs = new FileStream(file, FileMode.Open);
            MD5 md5 = new MD5CryptoServiceProvider();
            int fsLen = (int)fs.Length;
            byte[] heByte = new byte[fsLen];
            int r = fs.Read(heByte, 0, heByte.Length);
            string myStr = System.Text.Encoding.UTF8.GetString(heByte);
            byte[] retVal = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(myStr));
            fs.Close();

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < retVal.Length; i++)
            {
                sb.Append(retVal[i].ToString("x2"));
            }
            return sb.ToString();
        }
        catch (Exception ex)
        {
            throw new Exception("获取文件MD5值失败,error:" + ex.Message);
        }
    }

    /// <summary>
    /// 根据文本获取MD5
    /// </summary>
    /// <param name="content"></param>
    /// <returns></returns>
    public static string GetMD5FromText(string content)
    {
        MD5 md5 = new MD5CryptoServiceProvider();
        byte[] retVal = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(content));

        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < retVal.Length; i++)
        {
            sb.Append(retVal[i].ToString("x2"));
        }
        return sb.ToString();
    }

    /// <summary>
    /// 判断文件是否存在
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public static bool FileExists(string filePath)
    {
        return File.Exists(filePath);
    }

    /// <summary>
    /// 判断目录是否存在
    /// </summary>
    /// <param name="pathName"></param>
    /// <returns></returns>
    public static bool DirectoryExists(string pathName)
    {
        return Directory.Exists(pathName);
    }

    /// <summary>
    /// 获取文件的文本格式信息
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public static string ReadAllText(string filePath)
    {
        if (!FileExists(filePath))
        {
            //Utility.LogError("文件不存在，路径："+filePath);
            return null;
        }

        return File.ReadAllText(filePath);
    }

    /// <summary>
    /// 获取文件的文本格式信息
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="encoding"></param>
    /// <returns></returns>
    public static string ReadAllText(string filePath, Encoding encoding)
    {
        if (!FileExists(filePath))
        {
            Utility.LogError("文件不存在，路径：" + filePath);
            return null;
        }
        return File.ReadAllText(filePath, encoding);
    }

    /// <summary>
    /// 创建文件
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="content"></param>
    public static void CreateFile(string filePath, string content)
    {
        try
        {
            FileStream fs = new FileStream(filePath, FileMode.Create);
            //获得字节数组
            byte[] data = System.Text.Encoding.Default.GetBytes(content);
            //开始写入
            fs.Write(data, 0, data.Length);
            //清空缓冲区、关闭流
            fs.Flush();
            fs.Close();
        }
        catch (System.Exception e)
        {
            Utility.LogError("创建文件失败,error:" + e);
        }
        finally
        {
            //Utility.Log("filePath:" + filePath);
        }

    }

    /// <summary>
    /// 创建目录
    /// </summary>
    /// <param name="pathName"></param>
    public static DirectoryInfo CreateDirectory(string pathName)
    {
        return Directory.CreateDirectory(pathName);
    }

    /// <summary>
    /// 删除文件
    /// </summary>
    /// <param name="filePath"></param>
    public static void DeleteFile(string filePath)
    {
        File.Delete(filePath);
    }

    /// <summary>
    /// 删除文件
    /// </summary>
    /// <param name="pathName"></param>
    /// <param name="recursive"></param>
    public static void DeleteDirectory(string pathName, bool recursive = false)
    {
        Directory.Delete(pathName, recursive);
    }

    /// <summary>
    ///  拷贝文件夹
    /// </summary>
    /// <param name="path"></param>
    /// <param name="newPath"></param>
    public static void CopyDirectory(string path, string newPath)
    {
        string[] files = Directory.GetFiles(path, "*", SearchOption.AllDirectories);
        int length = files.Length;
        for (int i = 0; i < length; i++)
        {
            string f = files[i];
            UnityEditor.EditorUtility.DisplayProgressBar("拷贝文件夹到" + newPath, f, (float)i / length);
            string newFilePath = f.Replace(path, newPath);
            FileInfo fileInfo = new FileInfo(newFilePath);
            if (!Directory.Exists(fileInfo.DirectoryName))
            {
                Directory.CreateDirectory(fileInfo.DirectoryName);
            }
            File.Copy(f, newFilePath, true);
        }
        UnityEditor.EditorUtility.ClearProgressBar();
    }

    /// <summary>
    /// 拷贝文件
    /// </summary>
    /// <param name="path"></param>
    /// <param name="newPath"></param>
    public static void CopyFile(string path, string newPath, bool overwrite = true, bool createDirIfNull = false)
    {
        if (createDirIfNull)
        {
            FileInfo fileInfo = new FileInfo(newPath);
            if (!Directory.Exists(fileInfo.DirectoryName))
            {
                Directory.CreateDirectory(fileInfo.DirectoryName);
            }
        }

        File.Copy(path, newPath, overwrite);
    }

    /// <summary>
    /// 获取名字
    /// </summary>
    /// <param name="assetPath"></param>
    /// <returns></returns>
    public static string GetFileNameWithoutExtension(string assetPath)
    {
        return Path.GetFileNameWithoutExtension(assetPath);
    }

    public static string GetFileExtension(string assetPath)
    {
        return Path.GetExtension(assetPath);
    }

    /// <summary>
    /// 获取路径
    /// </summary>
    /// <param name="assetPath"></param>
    /// <returns></returns>
    public static string GetRelativeAssetsPath(string assetPath)
    {
        return assetPath.Replace('\\', '/');
    }

    /// <summary>
    /// 获取资源在unity的路径
    /// </summary>
    /// <param name="assetPath"></param>
    /// <returns></returns>
    public static string GetUnityAssetPath(string assetPath)
    {
        return "Assets" + Path.GetFullPath(assetPath).Replace(Path.GetFullPath(Application.dataPath), "").Replace('\\', '/');
    }

    /// <summary>
    /// 删除一个存在的文件
    /// </summary>
    /// <param name="assetPath"></param>
    /// <returns></returns>
    public static void DeleteFileIfExists(string assetPath)
    {
        if (FileExists(assetPath))
        {
            DeleteFile(assetPath);
        }
    }

    /// <summary>
    /// 获取AXAsset资源的保存路径
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static string GetFileSavePath(string name)
    {
        FileInfo file = new FileInfo(name);
        string directoryName = file.Directory.FullName.Replace("Assets", "AXAssets");
        if (!DirectoryExists(directoryName))
        {
            CreateDirectory(directoryName);
        }
        return directoryName + "/" + file.Name + ".ax";
    }

    /// <summary>
    /// 根据拓展名过滤器获取文件
    /// </summary>
    /// <param name="directory">目录路径</param>
    /// <param name="fitter">过滤器</param>
    /// <param name="option">查找方式</param>
    /// <returns></returns>
    public static string[] GetAllFilesByExtensionFitter(string directory, string fitter, SearchOption option)
    {
        string[] files = Directory.GetFiles(directory, "*.*", option).
        Where(s => !fitter.Contains(Path.GetExtension(s).ToLower())).ToArray();
        return files;
    }

    /// <summary>
    /// 根据拓展名获取文件
    /// </summary>
    /// <param name="directory"></param>
    /// <param name="pattern"></param>
    /// <param name="option"></param>
    /// <returns></returns>
    public static string[] GetAllFilesByExtension(string directory, string pattern, SearchOption option)
    {
        string[] files = Directory.GetFiles(directory, "*.*", option).
        Where(s => pattern.Contains(Path.GetExtension(s).ToLower())).ToArray();
        return files;
    }

    /// <summary>
    /// 获取AX缓存路径
    /// </summary>
    /// <returns></returns>
    public static string GetAXCacheAssetsName()
    {
        return Application.dataPath.Replace("Assets", "AXAssets");
    }
    /// <summary>
    /// 获取AX资源目录
    /// </summary>
    /// <returns></returns>
    public static string GetAXMainAssetName()
    {
        return Path.Combine(Application.dataPath, "AXAssetManager");
    }

    /// <summary>
    /// 浏览一个文件夹
    /// </summary>
    /// <param name="path"></param>
    public static void ExplorerDirectory(string path)
    {
        string fullPath = Path.GetFullPath(path);
        if (!Directory.Exists(fullPath))
        {
            Utility.LogError(fullPath + " 路径不存在！");
        }

        PlatformID platformId = Environment.OSVersion.Platform;
        Utility.Log("Current Platform is " + platformId);
        if (platformId == PlatformID.MacOSX || platformId == PlatformID.Unix)
        {
            Process p = new Process();
            p.StartInfo.FileName = "open";
            p.StartInfo.Arguments = Application.streamingAssetsPath;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.UseShellExecute = false;
            p.Start();
        }
        else if (platformId == PlatformID.Win32NT || platformId == PlatformID.Win32Windows)
        {
            Process.Start("explorer.exe", fullPath + "\\");
        }
    }

    //调用批处理程序，fileName：批处理路径，类似"D:\\New.bat";
    public static bool CallBatCode(string fileName, string args = "")
    {
        if (!File.Exists(fileName))
        {
            Utility.LogError(fileName + " 批处理文件不存在！");
            return false;
        }

        Process process = new Process();
        process.StartInfo.FileName = fileName;
        process.StartInfo.UseShellExecute = true;

        //这里相当于传参数 
        process.StartInfo.Arguments = args;
        bool isSuccess = process.Start();
        if (!isSuccess)
        {
            Utility.LogError("执行批处理失败：" + fileName);
        }

        //测试同步执行 
        process.WaitForExit();
        return isSuccess;
    }

    public static void Log(object o)
    {
        UnityEngine.Debug.Log(o);
    }

    public static void LogError(object o)
    {
        UnityEngine.Debug.LogError(o);
    }

    public static void LogWarning(object o)
    {
        UnityEngine.Debug.LogWarning(o);
    }

}
