/************************************************************************************
* Copyright(c) 2019 All Rights Reserved.
*创建人： lyp
*创建时间：2019-03-17
*版本：1.0
*设计意图：负责管理关卡
*1、开始关卡，加载场景、生成踏板、小球
*2、开始游戏
*=====================================================================
*修改时间：
*修改人：
*修改原因：
*修改内容：
************************************************************************************/
using UnityEngine;

public class CheckpointManager:SingleInstance<CheckpointManager>
{
    private bool             mIsInit;            //是否已初始化
    private FootBoardManager mFootBoardManager;  //踏板管理器
    private CharacterManager mBallManager;       //球管理器
    private int              mCurrentLv;         //当前关卡等级
    public  int              mCurrentFloor;      //当前层数
    public  int              mLastFloor;         //上一层级
    public int               mCurrentIntegral;   //当前积分

    public void Start()
    {
        mIsInit = false;
        mCurrentFloor = 0;
        mLastFloor = 0;
        int currentLv = PlayerPrefs.GetInt(AppConst.CurrentLv, 1);

        //判断是否要更新最高积分
        int bestIntegral = PlayerPrefs.GetInt(AppConst.BestIntegral, 0);
        if (mCurrentIntegral > bestIntegral)
        {
            PlayerPrefs.SetInt(AppConst.BestIntegral, mCurrentIntegral);
        }
        if (currentLv == mCurrentLv)
        {
            mCurrentIntegral = 0;
        }
        else
        {
            mCurrentLv = currentLv;
        }
        

        //初始化主界面关卡等级
        MainUpdateParam mainParam = new MainUpdateParam();
        mainParam.opType = MainOp.UpdateLv;
        mainParam.currentIntegral = mCurrentIntegral;
        UIManager.Instance.UpdatePanel(PanelEnum.Main, mainParam);

        //加载场景
        ResourceManager.Instance.LoadSceneAsync("Checkpoint", delegate ()
        {
            //获取场景中"logic"对象
            GameObject logic = GameObject.Find("logic");
            if (null == logic)
            {
                LogManager.Instance.LogError("场景中不存在logic物体！！！");
                return;
            }
           
            //生成踏板
            mFootBoardManager = new FootBoardManager();
            mFootBoardManager.Init(logic, mCurrentLv);

            //初始化小球
            mBallManager = new CharacterManager();
            mBallManager.Init();

            mIsInit = true;
            LogManager.Instance.Log("初始化关卡管理器完成");
        });       
    }

    public void Update()
    {
        if (!mIsInit) {
            return;
        }

        //更新小球状态
        mBallManager.Update();

        //更新踏板状态
        if (mCurrentFloor != mLastFloor) {
            mFootBoardManager.HideFloor(mLastFloor);
            mLastFloor = mCurrentFloor;
            mCurrentIntegral += mCurrentLv;
            MainUpdateParam mainParam = new MainUpdateParam();
            mainParam.opType = MainOp.UpdateIntegral;
            mainParam.currentIntegral = mCurrentIntegral;
            UIManager.Instance.UpdatePanel(PanelEnum.Main, mainParam);
        }      
    }
}
