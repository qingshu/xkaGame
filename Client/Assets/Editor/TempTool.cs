/************************************************************************************
* Copyright(c) 2019 All Rights Reserved.
*创建人： 
*创建时间：2019-04-14 21:24:29
*版本：1.0
*设计意图：
*=====================================================================
*修改时间：
*修改人：
*修改原因：
*修改内容：
************************************************************************************/ 
using UnityEngine;
using UnityEditor;

public class TempTool : EditorWindow
{
    [MenuItem("Tools/Temp")]
    static void Excute()
    {
        Rect rect = new Rect(200, 200, 670, 500);
        TempTool createUICodeAutoWin = EditorWindow.GetWindowWithRect<TempTool>(rect);
        createUICodeAutoWin.Show();
    }

    void OnGUI()
    {
        GUILayout.Label("请鼠标选中Project中要处理的预设和预设们根目录，再点击确定");
        if (GUILayout.Button("执行"))
        {
            Object[] objSelect = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
            for (int i = 0; i < objSelect.Length; i++) {
                GameObject obj = objSelect[i] as GameObject;
                MeshCollider[] meshCollider = obj.GetComponentsInChildren<MeshCollider>();
                if (meshCollider.Length > 0) {
                    continue;
                }
                MeshRenderer[] meshRenders = obj.GetComponentsInChildren<MeshRenderer>();
                LogManager.Instance.Log("********************************************" + obj.name + "********************************************");
                for (int j = 0; j < meshRenders.Length; j++) {
                    MeshRenderer meshRender = meshRenders[j];
                    meshRender.gameObject.AddComponent<MeshCollider>();
                    LogManager.Instance.Log(meshRender.name);
                }             
            }
            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
        }
    }

    private void OnDisable()
    {
        
    }
}
