/************************************************************************************
* Copyright(c) 2019 All Rights Reserved.
*创建人： lyp
*创建时间：2019-03-11 17:55:34
*版本：1.0
*设计意图：统一处理资源，设置格式等
************************************************************************************/

using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Linq;


public class AssetProcessor : AssetPostprocessor
{
    /// <summary>
    /// 资源被导入、删除、移动完成之后调用
    /// </summary>
    /// <param name="importedAssets"></param> 
    /// <param name="deletedAssets"></param>
    /// <param name="movedAssets"></param>
    /// <param name="movedFromAssetPaths"></param>
    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        // 检查资源的格式
        CheckResoucesFormat(importedAssets);

        // 处理UI图集
        HandleUITexture(importedAssets);

        // 处理资源的AB包
        //HandleAB(importedAssets);
    }

    /// <summary>
    /// 检查资源的格式
    /// </summary>
    /// <param name="importedAssets"></param>
    static void CheckResoucesFormat(string[] importedAssets)
    {
        importedAssets = importedAssets.Where(s => s.Contains("Resource")).ToArray();
        var BundleConfig = new Dictionary<string, HashSet<string>>();
        var ErrorString = "";

        //遍历文件
        foreach (string file in importedAssets)
        {
            //检查是否存在dds
            if (file.EndsWith(".dds") || file.EndsWith(".DDS"))
            {
                ErrorString += "DDS:" + file + "\n";
            }
            string fileName = Utility.GetUnityAssetPath(file);
            AssetImporter importer = AssetImporter.GetAtPath(fileName);
            if (importer == null)
            {
                continue;
            }

            if (importer.assetBundleName.Equals("") && importer.assetBundleVariant.Equals(""))
            {
                continue;
            }
            //检查assetbundle设置是否错误
            if (importer.assetBundleName.Equals(""))
            {
                ErrorString += "配置错误:" + file + "\n";

                continue;
            }
            if (importer.assetBundleVariant.Equals(""))
            {
                ErrorString += "配置错误:" + file + "\n";
                continue;
            }
            //检查同一个assetbundle是否有重复
            if (BundleConfig.ContainsKey(importer.assetBundleName))
            {
                HashSet<string> set = BundleConfig[importer.assetBundleName];
                if (set.Contains(file))
                {
                    ErrorString += "文件重复:" + file + "\n";
                    continue;
                }
                else
                {
                    set.Add(file);
                }
            }
            else
            {
                HashSet<string> set = new HashSet<string>();
                set.Add(file);
                BundleConfig.Add(importer.assetBundleName, set);
            }
        }
        if (!string.IsNullOrEmpty(ErrorString))
        {
            EditorUtility.DisplayDialog("检查文件出错", "错误内容，项目里不能出现dds文件，赶紧找美术删掉\n" + ErrorString, "确定");
        }
    }

    /// <summary>
    ///  处理UI图集
    /// </summary>
    static void HandleUITexture(string[] importedAssets)
    {
        // 设置图集Tag
        importedAssets = importedAssets.Where(s => s.Contains("LuaFramework/UI/Texture")).ToArray();
        foreach (var file in importedAssets)
        {
            FileInfo fileInfo = new FileInfo(file);

            TextureImporter importer = AssetImporter.GetAtPath(file) as TextureImporter;
            if (importer == null)
            {
                Utility.LogError("SetDirectoryUIAlts:importer is null, path = " + file);
                return;
            }
            importer.textureType = TextureImporterType.Sprite;
            importer.spritePackingTag = fileInfo.Directory.Name;

            BuildTarget target = EditorUserBuildSettings.activeBuildTarget;

            if (target == BuildTarget.Android)
            {
                importer.textureType = TextureImporterType.Default;
                importer.textureFormat = TextureImporterFormat.ETC_RGB4;            
                importer.SetPlatformTextureSettings("Android", 1024, importer.textureFormat, (int)UnityEditor.TextureCompressionQuality.Fast, true);
                importer.SetAllowsAlphaSplitting(true);
            }
            else if (target == BuildTarget.iOS)
            {
                importer.textureFormat = TextureImporterFormat.ARGB16;
                importer.SetPlatformTextureSettings("iPhone", 1024, TextureImporterFormat.ARGB16, (int)UnityEditor.TextureCompressionQuality.Normal, false);
            }
            else
            {
                importer.textureType = TextureImporterType.Sprite;
                importer.textureFormat = TextureImporterFormat.AutomaticCompressed;
            }

            importer.spriteImportMode = SpriteImportMode.Polygon;
            importer.mipmapEnabled = false;
            importer.maxTextureSize = 1024;
        }
    }

    /// <summary>
    /// 处理资源的AB包
    /// </summary>
    static void HandleAB(string[] importedAssets)
    {
        // 处理Texture的AB包
        HandleTextureBundle(importedAssets);

        // 打包角色
        HandleRoleBundle(importedAssets);

        // 处理场景预设打包
        HandleScenesBundle(importedAssets);

        // 处理UI打包
        HandleUIBundle(importedAssets);

        // 处理声音
        HandleSoundBundle(importedAssets);

        // 处理光效预设打包
        HandleEffectBundle(importedAssets);

    }

    #region 处理各个资源的AB包名
    /// <summary>
    /// 处理Texture的AB包
    /// </summary>
    static void HandleTextureBundle(string[] importedAssets)
    {
        string patter = "*.png*.jpg";
        string[] textures = importedAssets.Where(s => s.Contains("LuaFramework/UI/Texture/") && patter.Contains(Utility.GetFileExtension(s).ToLower())).ToArray();
        foreach (var file in textures)
        {
            FileInfo fileInfo = new FileInfo(file);
            string abName = "texture/" + fileInfo.Directory.Name;
            SetABName(file, abName);
        }
    }

    /// <summary>
    /// 打包角色
    /// </summary>
    static void HandleRoleBundle(string[] importedAssets)
    {
        string patter = "*.prefab";
        string[] files = importedAssets.Where(s => s.Contains("LuaFramework/Role/Perfab/") && patter.Contains(Utility.GetFileExtension(s).ToLower())).ToArray();
        foreach (var file in files)
        {
            string fileName = Utility.GetFileNameWithoutExtension(file);
            string abName = "role/" + fileName;
            SetABName(file, abName);
        }
    }

    /// <summary>
    /// 处理场景预设打包
    /// </summary>
    static void HandleScenesBundle(string[] importedAssets)
    {
        string patter = "*.unity";
        string[] files = importedAssets.Where(s => s.Contains("LuaFramework/Scenes/Scene/") && patter.Contains(Utility.GetFileExtension(s).ToLower())).ToArray();
        foreach (var file in files)
        {
            string fileName = Utility.GetFileNameWithoutExtension(file);
            string abName = "scenes/" + fileName;
            SetABName(file, abName);
        }
    }

    /// <summary>
    /// 处理UI打包
    /// </summary>
    static void HandleUIBundle(string[] importedAssets)
    {
        string patter = "*.prefab";
        string[] files = importedAssets.Where(s => s.Contains("LuaFramework/UI/Perfab/") && patter.Contains(Utility.GetFileExtension(s).ToLower())).ToArray();
        foreach (var file in files)
        {
            string fileName = Utility.GetFileNameWithoutExtension(file);
            string abName = "ui/" + fileName;
            SetABName(file, abName);
        }
    }

    /// <summary>
    /// 处理声音打包
    /// </summary>
    static void HandleSoundBundle(string[] importedAssets)
    {
        string patter = "*.mp3";
        string[] files = importedAssets.Where(s => s.Contains("LuaFramework/Sound/") && patter.Contains(Utility.GetFileExtension(s).ToLower())).ToArray();
        foreach (var file in files)
        {
            SetABName(file, "sound");
        }
    }

    /// <summary>
    /// 处理光效预设打包
    /// </summary>
    static void HandleEffectBundle(string[] importedAssets)
    {
        string patter = "*.prefab";
        string[] files = importedAssets.Where(s => s.Contains("LuaFramework/Effect/UI/") && patter.Contains(Utility.GetFileExtension(s).ToLower())).ToArray();
        foreach (var file in files)
        {
            SetABName(file, "effect");
        }

        string[] files2 = importedAssets.Where(s => s.Contains("LuaFramework/Effect2/") && patter.Contains(Utility.GetFileExtension(s).ToLower())).ToArray();
        foreach (var file in files2)
        {
            SetABName(file, "effect2");
        }
    }
    #endregion

    /// <summary>
    ///  设置图片的tag
    /// </summary>
    /// <param name="assetPath"></param>
    static void SetUITextureTag(string assetPath)
    {
        
    }

    /// <summary>
    /// 设置AB包名
    /// </summary>
    static void SetABName(string assetPath, string abName, string abVariant = null)
    {
        string path = Utility.GetUnityAssetPath(assetPath);
        AssetImporter importer = AssetImporter.GetAtPath(path);
        if (importer == null)
        {
            Utility.LogError("SetABName:importer is null, path = " + path);
            return;
        }
        if (abVariant == null)
        {
            abVariant = "unity3d";
        }
        importer.assetBundleName = abName;
        importer.assetBundleVariant = abVariant;
    }

}

