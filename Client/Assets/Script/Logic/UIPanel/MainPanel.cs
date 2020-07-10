/************************************************************************************
* Copyright(c) 2019 All Rights Reserved.
*创建人： lyp
*创建时间：2019-03-17
*版本：1.0
*设计意图：UI面板-首页
*=====================================================================
*修改时间：
*修改人：
*修改原因：
*修改内容：
************************************************************************************/
using UnityEngine;
using UnityEngine.UI;

//主界面行为
public enum MainOp {
    UpdateLv,      //更新等级
    UpdateIntegral,//更新积分
    Died,          //小球死亡更新界面
}

//主界面更新参数
public class MainUpdateParam : ParamBase {
    public MainOp opType;
    public int currentIntegral;
}

public class MainPanel : UIPanelBase
{
    private Text _txtCurrentLv;
    private Text _txtNextLv;
    private Text _txtCurrentIntegral;
    private Text _txtBestIngral;
    private GameObject _objNewGuide;
    private GameObject _objDied;

    //初始化界面
    public override void InitUI(GameObject obj)
    {       
        base.InitUI(obj);
        Transform _TextParent = obj.transform.Find("_TextParent");
        _txtCurrentLv = _TextParent.Find("_txtCurrentLv").GetComponent<Text>();
        _txtNextLv = _TextParent.Find("_txtNextLv").GetComponent<Text>();
        _txtCurrentIntegral = _TextParent.Find("_txtCurrentIntegral").GetComponent<Text>();
        _txtBestIngral = _TextParent.Find("_txtBestIngral").GetComponent<Text>();

        //新手页面
        _objNewGuide = obj.transform.Find("_objNewGuide").gameObject;

        //死亡页面       
        _objDied = obj.transform.Find("_objDied").gameObject;
        Button _txtRestart = _objDied.transform.Find("_txtRestart").GetComponent<Button>();
        _txtRestart.onClick.AddListener(OnRestart);
        Button _txtClickRestart = _objDied.transform.Find("_txtClickRestart").GetComponent<Button>();
        _txtClickRestart.onClick.AddListener(OnRestart);
    }

    //打开界面
    public override void ShowPanel(ParamBase param) {
        //判断是否打开新手引导
        int isHadShowGuide = PlayerPrefs.GetInt(AppConst.IsHadShownGuide, 0);
        _objNewGuide.SetActive(isHadShowGuide != 1);
        PlayerPrefs.SetInt(AppConst.IsHadShownGuide, 1);

        mRootObj.SetActive(true);
        CheckpointManager.Instance.Start();
        mRootObj.AddComponent<UIEventSystem>();
    }

    //更新界面
    public override void UpdatePanel(ParamBase param) {
        MainUpdateParam mainParam = (MainUpdateParam)param;
        switch (mainParam.opType)
        {
            case MainOp.UpdateLv:
                UpdateLv(mainParam);
                break;
            case MainOp.UpdateIntegral:
                _txtCurrentIntegral.text = mainParam.currentIntegral.ToString();
                break;
            case MainOp.Died:
                _objDied.SetActive(true);
                break;
            default:
                break;
        }
    }

    //关闭界面
    public override void ClosePanel() {
        base.ClosePanel();
    }

    public override void Update()
    {
        
    }

    public override void SetInfoByLanguage()
    {
        
    }

    //********************************************私有方法**********************************************
    private void OnRestart() {
        CheckpointManager.Instance.Start();
    }

    //更新关卡等级
    private void UpdateLv(MainUpdateParam mainParam) {
        _objDied.SetActive(false);
        _txtCurrentIntegral.text = mainParam.currentIntegral.ToString();
        int currentLv = PlayerPrefs.GetInt(AppConst.CurrentLv, 1);
        _txtCurrentLv.text = currentLv.ToString();
        int nextLv = currentLv + 1;
        _txtNextLv.text = nextLv.ToString();
        int bestIntegral = PlayerPrefs.GetInt(AppConst.BestIntegral, 1);
        _txtBestIngral.text = "最佳：" + bestIntegral.ToString();
    }
}
