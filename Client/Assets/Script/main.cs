using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//创建时间：2019 03 13
//作者：lyp
//设计意图：游戏入口，通过调用gamemanager来进行整个游戏

public class main : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject canvas = GameObject.Find("Canvas");
        if (null == canvas) {
            LogManager.Instance.LogError("不存在ui画布！！！");
            return;
        }
        DontDestroyOnLoad(canvas);
        GameObject eventSys = GameObject.Find("EventSystem");
        if (null == eventSys)
        {
            LogManager.Instance.LogError("不存在事件监听器！！！");
            return;
        }
        DontDestroyOnLoad(eventSys);
        LogManager.Instance.Log("当前语言环境：" + Application.systemLanguage.ToString());
        gameObject.AddComponent<ResourceManager>();

        //打开主界面
        UIManager.Instance.ShowPanel(PanelEnum.Main, null);
    }

    // Update is called once per frame
    void Update()
    {
        GameManager.Instance.Update();
    }
}
