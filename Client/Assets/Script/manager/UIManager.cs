/************************************************************************************
* Copyright(c) 2019 All Rights Reserved.
*创建人： lyp
*创建时间：2019-03-13
*版本：1.0
*设计意图：ui管理器
* 1、负责打开ui界面
* 2、负责更新ui界面
* 3、负责关闭ui界面
*=====================================================================
*修改时间：
*修改人：
*修改原因：
*修改内容：
************************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PanelEnum
{
    Main,      //主界面
    Setting,   //设置界面
    Shop,      //商店界面
    Skin,      //皮肤界面
}

public class UIManager:SingleInstance<UIManager>
{
    private Transform mPanelParent;
    private Transform mPopPanelParent;
    private Dictionary<PanelEnum, UIPanelBase> dicAllUIPanel;    //所有界面
    private Stack<PanelEnum>             stPanelList;      //打开的界面枚举堆栈
    protected override void Init() {
        GameObject canvas = GameObject.Find("Canvas");
        if (null == canvas)
        {
            LogManager.Instance.LogError("不存在ui画布！！！");
            return;
        }
        mPanelParent = canvas.transform.Find("UICamera/Panel");
        mPopPanelParent = canvas.transform.Find("UICamera/PopPanel");


        //所有界面
        dicAllUIPanel = new Dictionary<PanelEnum, UIPanelBase>();

        //初始化主界面
        UIPanelBase mainPanel = new MainPanel();
        mainPanel.SetPanleConfig(new UIPanelConfig(PanelEnum.Main,false, true, AppConst.uiPanelResPath + "MainPanel"));
        dicAllUIPanel[PanelEnum.Main] = mainPanel;

        stPanelList = new Stack<PanelEnum>();     
        LogManager.Instance.Log("ui管理器初始化完毕");
    }

    //打开界面
    public void ShowPanel(PanelEnum panelEnum, ParamBase param) {
        //获取界面数据
        UIPanelBase panel = dicAllUIPanel[panelEnum];
        if (null == panel) {
            LogManager.Instance.LogError("不存在界面，请检查是否在UIManager.Init中初始化！：" + panelEnum);
            return;
        }

        //判断是否已加载
        if (!panel.IsInit()) {
            //获取界面配置
            UIPanelConfig panelConfig = panel.GetPanelConfig();
            if (null == panelConfig) {
                LogManager.Instance.LogError("不存在界面配置，请检查是否在UIManager.Init中初始化！：" + panelEnum);
                return;
            }

            //加载面板
            ResourceManager.Instance.LoadResByPath<GameObject>(new string[1] { panelConfig.ResPath }, delegate (UnityEngine.Object[] objs)
            {
                if (objs.Length <= 0) {
                    LogManager.Instance.LogError("加载面板失败：" + panel);
                    return;
                }
                GameObject objPanel = objs[0] as GameObject;
                if (null == objPanel)
                {
                    LogManager.Instance.LogError("加载面板失败：" + panel);
                    return;
                }
                if (panelConfig.IsPanel)
                {
                    objPanel.transform.parent = mPanelParent;
                }
                else
                {
                    objPanel.transform.parent = mPopPanelParent;
                }
                objPanel.transform.localScale = Vector3.one;
                RootPanel rootPanel = objPanel.GetComponent<RootPanel>();
                if (null == rootPanel)
                {
                    rootPanel = objPanel.AddComponent<RootPanel>();
                }
                rootPanel.SetUIHandle(panel);
                SwitchPanel(panel, param);
            });
            return;
        }
        SwitchPanel(panel, param);
    }

   
    //更新界面
    public void UpdatePanel(PanelEnum panelEnum, ParamBase param) {
        //获取界面数据
        UIPanelBase panel = dicAllUIPanel[panelEnum];
        if (null == panel)
        {
            LogManager.Instance.LogError("不存在界面，请检查是否在UIManager.Init中初始化！：" + panelEnum);
            return;
        }
        panel.UpdatePanel(param);
    }

    //关闭当前界面
    public void ClosePanel()
    {
        //如果是poppanel,则直接关闭

        //如果是面板，则要一直找到上一个面板打开,并关闭之间的所有poppanel
    }

    //******************************************************私有方法******************************************************
    //切换页面
    private void SwitchPanel(UIPanelBase panel, ParamBase param)
    {
        //获取界面配置
        UIPanelConfig panelConfig = panel.GetPanelConfig();
        if (null == panelConfig)
        {
            LogManager.Instance.LogError("不存在界面配置，请检查是否在UIManager.Init中初始化");
            return;
        }

        //如果打开的是面板则先关闭上一个界面
        if (!panelConfig.IsPanel)
        {
            while (stPanelList.Count > 0)
            {
                //获取界面并关闭
                PanelEnum lastPanelEnum = stPanelList.Pop();
                UIPanelBase lastPanel = dicAllUIPanel[lastPanelEnum];
                if (null == lastPanel)
                {
                    LogManager.Instance.LogError("界面不存在，请检查是否在UIManager.Init中初始化:" + lastPanelEnum);
                    continue;
                }

                //关闭界面
                lastPanel.ClosePanel();

                //判断这个界面是否为panel,如果不是则继续关闭
                UIPanelConfig lastPanelConfig = lastPanel.GetPanelConfig();
                if (null == lastPanelConfig)
                {
                    LogManager.Instance.LogError("不存在界面配置，请检查是否在UIManager.Init中初始化:" + lastPanelEnum);
                    continue;
                }
                if (lastPanelConfig.IsPanel)
                {
                    break;
                }
            }
        }


        //打开界面
        panel.ShowPanel(param);
        stPanelList.Push(panelConfig.PanelEn);
    }

}
