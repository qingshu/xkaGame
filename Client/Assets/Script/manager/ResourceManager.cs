/************************************************************************************
* Copyright(c) 2019 All Rights Reserved.
*创建人： lyp
*创建时间：2019-03-13
*版本：1.0
*设计意图：资源管理器
*=====================================================================
*修改时间：
*修改人：
*修改原因：
*修改内容：
************************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResourceManager : MonoBehaviour
{

    private static ResourceManager m_instance;
    public static ResourceManager Instance
    {
        get
        {
            return m_instance;
        }
    }

    private void Awake()
    {
        m_instance = this;
    }

    //加载资源
    public void LoadResByPath<T>(string[] paths,Action<UnityEngine.Object[]> callBackFunc) where T:UnityEngine.Object{
        if (null == callBackFunc) {
            LogManager.Instance.LogError("资源加载没有回调函数：" + paths);
            return;
        }

        //先去重
        Dictionary<string, UnityEngine.Object> dicPath = new Dictionary<string, UnityEngine.Object>();
        for (int i = 0; i < paths.Length; i++)
        {
            string path = paths[i];
            dicPath[path] = null;
        }

        //加载资源
        string[] keys = new string[dicPath.Count];
        dicPath.Keys.CopyTo(keys, 0);
        for(int i = 0;i < keys.Length;i++)
        {
            string key = keys[i];
            UnityEngine.Object obj = Resources.Load<T>(key);
            if (null == obj)
            {
                LogManager.Instance.LogError("加载资源失败：" + paths);
                return;
            }
            dicPath[key] = obj;
        }

        //克隆资源
        UnityEngine.Object[] objs = new UnityEngine.Object[paths.Length];
        for (int i = 0; i < paths.Length; i++)
        {
            string path = paths[i];
            UnityEngine.Object obj = dicPath[path];
            UnityEngine.Object prefab = GameObject.Instantiate(obj);
            objs[i] = prefab;
        }
        
        callBackFunc(objs);
    }

    //加载场景  LoadSceneAsync
    public void LoadSceneAsync(string scene, Action callBackFunc)
    {
        if (null == callBackFunc)
        {
            LogManager.Instance.LogError("场景加载没有回调函数：" + scene);
            return;
        }
        StartCoroutine(LoadSceneAsync_Cor(scene, callBackFunc)); 
    }

    IEnumerator LoadSceneAsync_Cor(string scene, Action callBackFunc)
    {
        var aync = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Single);
        yield return aync;
        callBackFunc();
    }
}
