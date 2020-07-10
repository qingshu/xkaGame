/************************************************************************************
* Copyright(c) 2019 All Rights Reserved.
*创建人： lyp
*创建时间：2019-03-24 14:50:34
*版本：1.0
*设计意图：所有UI界面挂载的脚本
*=====================================================================
*修改时间：
*修改人：
*修改原因：
*修改内容：
************************************************************************************/ 
using UnityEngine;
using System.Collections;

public class RootPanel : MonoBehaviour
{
    private UIPanelBase mUIHandle;

    public void SetUIHandle(UIPanelBase handle) {
        handle.InitUI(gameObject);
        mUIHandle = handle;
    }

    // Update is called once per frame
    void Update()
    {
        if (null == mUIHandle) {
            return;
        }
        mUIHandle.Update();
    }
}
