/************************************************************************************
* Copyright(c) 2019 All Rights Reserved.
*创建人： 
*创建时间：2019-04-14 02:01:40
*版本：1.0
*设计意图：
*=====================================================================
*修改时间：
*修改人：
*修改原因：
*修改内容：
************************************************************************************/ 
using System.IO;
using UnityEditor;
using UnityEngine;

public class CreateExcelDataAsset : Editor
{
    [MenuItem("Tools/_CreateExcelDataAsset")]
    public static void _CreateExcelDataAsset()
    {
        //判断路径是否存在
        if (!Directory.Exists(AppConst.excelsFolderPath))
        {
            LogManager.Instance.LogError("表格路径不存在，请检查！");
            return;
        }

        ExcelData excelData = ScriptableObject.CreateInstance<ExcelData>();
        excelData.checkPointDatas = ExcelTool.CreateArrayWithExcel<Conf_Checkpoint>(Path.Combine(AppConst.excelsFolderPath,"Checkpoint.xlsx"));
        excelData.floorDatas = ExcelTool.CreateArrayWithExcel<Conf_Floor>(Path.Combine(AppConst.excelsFolderPath, "Floor.xlsx"));
        excelData.characterDatas = ExcelTool.CreateArrayWithExcel<Conf_Character>(Path.Combine(AppConst.excelsFolderPath, "Character.xlsx"));

        //确保文件夹存在
        if (!Directory.Exists(AppConst.excelAssetsSavePath))
        {
            Directory.CreateDirectory(AppConst.excelAssetsSavePath);
        }

        //asset文件的路径 要以"Assets/..."开始，否则CreateAsset会报错
        string assetPath = string.Format("{0}ExcelData.asset", AppConst.excelAssetsSavePath);
        AssetDatabase.CreateAsset(excelData, assetPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
