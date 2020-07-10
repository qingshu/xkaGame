/************************************************************************************
* Copyright(c) 2019 All Rights Reserved.
*创建人： lyp
*创建时间：2019-03-23 17:55:34
*版本：1.0
*设计意图：用unity接口统一为所有代码添加开头注释，就不用每个人自己去修改vs模板
************************************************************************************/
using System.IO;

public class NewScriptTemplete : UnityEditor.AssetModificationProcessor
{
    /// <summary>
    /// 在资源创建时调用
    /// </summary>
    /// <param name="path">自动传入资源路径</param>
    public static void OnWillCreateAsset(string path)
    {
        path = path.Replace(".meta", "");
        if (!path.EndsWith(".cs")) return;
        string allText = @"/************************************************************************************
* Copyright(c) $year$ All Rights Reserved.
*创建人： 
*创建时间：#CreateTime#
*版本：1.0
*设计意图：
*=====================================================================
*修改时间：
*修改人：
*修改原因：
*修改内容：
************************************************************************************/ ";
        allText += "\r\n";
        allText += File.ReadAllText(path);
        allText = allText.Replace("$year$", System.DateTime.Now.ToString("yyyy"));
        allText = allText.Replace("#CreateTime#", System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        File.WriteAllText(path, allText);
    }
}
