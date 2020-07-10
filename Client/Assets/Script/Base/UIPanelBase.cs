/************************************************************************************
* Copyright(c) 2019 All Rights Reserved.
*创建人： lyp
*创建时间：2019-03-17
*版本：1.0
*设计意图：ui界面基类，提供打开、更新、关闭界面等操作
*=====================================================================
*修改时间：
*修改人：
*修改原因：
*修改内容：
************************************************************************************/

using UnityEngine;

//界面配置
public class UIPanelConfig
{
    public bool   IsDestroyWhenClose;
    public bool   IsPanel;
    public string ResPath;
    public PanelEnum PanelEn;

    public UIPanelConfig(PanelEnum panelEnum , bool isDestroyWhenClose,bool isPanel,string resPath) {
        PanelEn = panelEnum;        
        IsPanel = isPanel;
        ResPath = resPath;
        IsDestroyWhenClose = isDestroyWhenClose;
    }
}

//界面基类
public abstract class UIPanelBase
{
    protected GameObject    mRootObj;
    protected UIPanelConfig mUIPanelConfig;

    //初始化ui
    public virtual void InitUI(GameObject obj) {
        mRootObj = obj;
    }

    //设置配置
    public void SetPanleConfig(UIPanelConfig panelConfig) {
        mUIPanelConfig = panelConfig;
    }

    //获取配置
    public UIPanelConfig GetPanelConfig() {
        return mUIPanelConfig;
    }

    //打开界面
    public abstract void ShowPanel(ParamBase param);

    //更新界面
    public abstract void UpdatePanel(ParamBase param);

    //每一帧的更新
    public abstract void Update();

    //关闭界面
    public virtual void ClosePanel() {
        if (mUIPanelConfig.IsDestroyWhenClose) {
            GameObject.Destroy(mRootObj);
            mRootObj = null;
        }
    }

    //根据语言设置信息
    public abstract void SetInfoByLanguage();

    //是否初始化过界面
    public bool IsInit() {
        return null != mRootObj;
    }
}
