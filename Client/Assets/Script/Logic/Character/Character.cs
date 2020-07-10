/************************************************************************************
* Copyright(c) 2019 All Rights Reserved.
*创建人： 
*创建时间：2019-04-21 17:02:47
*版本：1.0
*设计意图：
*=====================================================================
*修改时间：
*修改人：
*修改原因：
*修改内容：
************************************************************************************/ 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//角色光效
public enum CharacterEffectType {
    Hit,
    Fall,
    Invincible,
    Max,
}

public class Character : MonoBehaviour
{
    private StateManagerBase mStateManager;
    private Conf_Character mConfCharacter;
    private GameObject[] mObjEffects;
    private GameObject mObjFallEffect;
    private GameObject mObjInvincibleEffect;
    private bool mIsInvincible;             //是否无敌
    private bool mIsDied;                   //是否死亡
    public  int  mCurrentFloor;             //当前层数
    public  int  mFallContinuityFloorCount; //连续下落的层数

    public void Init(StateManagerBase stateManager, Conf_Character conf_Character)
    {
        mStateManager = stateManager;
        mConfCharacter = conf_Character;

        //加载碰撞光效
        mObjEffects = new GameObject[(int)CharacterEffectType.Max];
        string[] effectsPath = new string[(int)CharacterEffectType.Max] {
            AppConst.effectResPath + conf_Character.CollisionEffect,
            AppConst.effectResPath + conf_Character.DropEffect,
            AppConst.effectResPath + conf_Character.InvincibleEffect
        };
        ResourceManager.Instance.LoadResByPath<GameObject>(effectsPath, delegate (UnityEngine.Object[] objs)
        {
            if (objs.Length <= 0)
            {
                LogManager.Instance.LogError("加载光效失败：" + effectsPath);
                return;
            }
            for (int i = 0; i < objs.Length; i++)
            {
                if (null == objs[i])
                {
                    LogManager.Instance.LogError("加载光效失败：" + effectsPath[i]);
                    return;
                }
                mObjEffects[i] = objs[i] as GameObject;
                mObjEffects[i].SetActive(false);
                if (null == mObjEffects[i])
                {
                    LogManager.Instance.LogError("加载光效失败：" + effectsPath[i]);
                    return;
                }
                mObjEffects[i].transform.parent = transform;
                mObjEffects[i].transform.localPosition = Vector3.zero;
            }           
        });
    }

    //切换状态
    public void ChangeState(int state,ParamBase param)
    {
        if (state >= (int)CharacterStateEnum.Max)
        {
            LogManager.Instance.LogError("切换小球状态异常：" + state);
            return;
        }

        //判断是否播放无敌光效
        mIsInvincible = state == (int)CharacterStateEnum.Invincible;
        GameObject invincibleEffect = mObjEffects[(int)CharacterEffectType.Invincible];
        if (null != invincibleEffect)
        {
            invincibleEffect.SetActive(mIsInvincible);
        }

        bool isFall = (state == (int)CharacterStateEnum.Fall || state == (int)CharacterStateEnum.BounceUp);
        GameObject fallEffect = mObjEffects[(int)CharacterEffectType.Fall];
        if (null != fallEffect)
        {
            fallEffect.SetActive(isFall);
        }

        mStateManager.ChangeState(state, param);
    }

    //开始接触
    void OnTriggerEnter(Collider collider)
    {
        //判断是否已死亡
        if (mIsDied)
        {            
            return;
        }

        //碰撞光效
        GameObject hitEffect = mObjEffects[(int)CharacterEffectType.Hit];
        if (null == hitEffect) {
            LogManager.Instance.LogError("碰撞光效不存在");
            return;
        }
        hitEffect.transform.parent = transform;
        hitEffect.transform.localPosition = new Vector3(-0.001f, 0.001f, -0.0066f);
        hitEffect.transform.parent = collider.transform;
        hitEffect.SetActive(false);
        hitEffect.SetActive(true);

        //判断是否底部
        if (collider.name.Contains("bottom"))
        {
            ChangeState((int)CharacterStateEnum.Idle, null);
            int currentLv = PlayerPrefs.GetInt(AppConst.CurrentLv, 1);
            currentLv++;
            PlayerPrefs.SetInt(AppConst.CurrentLv, currentLv);
            CheckpointManager.Instance.Start();
            return;
        }

        //判断是否无敌
        if (mIsInvincible || mFallContinuityFloorCount >= AppConst.InvincibleTriggerFloor) {
            mFallContinuityFloorCount = 0;
            return;
        }
        mFallContinuityFloorCount = 0;

        //根据碰撞的踏板类型切换状态
        if (collider.name.Contains("safe"))
        {
            ChangeState((int)CharacterStateEnum.Collide,null);
        }
        else
        {
            mIsDied = true;
            hitEffect.SetActive(false);
            ChangeState((int)CharacterStateEnum.Died,null);

            //更新界面
            MainUpdateParam mainParam = new MainUpdateParam();
            mainParam.opType = MainOp.Died;
            UIManager.Instance.UpdatePanel(PanelEnum.Main, mainParam);
        }
    }
}
