/************************************************************************************
* Copyright(c) 2019 All Rights Reserved.
*创建人： lyp
*创建时间：2019-04-21 16:08:17
*版本：1.0
*设计意图：角色（小球）下落状态
*=====================================================================
*修改时间：
*修改人：
*修改原因：
*修改内容：
************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

//下落参数
public class CharacterFallParam : ParamBase
{
    public float fallLength;
}

public class CharacterFall : StateBase
{
    private float mFallLength;
    public CharacterFall(Character character) : base(character)
    {
        
    }

    public override void Init(ParamBase param)
    {
        CharacterFallParam fallParam = (CharacterFallParam)param;
        if (null == fallParam)
        {
            LogManager.Instance.LogError("下落参数异常");
            return;
        }
        mFallLength = fallParam.fallLength;
        base.Init(param);
    }

    public override void Start()
    {
        mIsStart = true;
    }

    public override void Run()
    {
        Vector3 pos = mCharacter.transform.localPosition;
        pos.y -= AppConst.CharacterMoveSpeed;
        mFallLength += AppConst.CharacterMoveSpeed;
        if (mFallLength > AppConst.FallLength2CameraMove) {
            Vector3 cameraPos = Camera.main.transform.localPosition;
            cameraPos.y -= AppConst.CharacterMoveSpeed;
            Camera.main.transform.localPosition = cameraPos;

            //判断是否改变层级
            float floorPos = AppConst.FloorStartYPos - AppConst.FloorSpace * mCharacter.mCurrentFloor;
            if (floorPos > pos.y) {
                mCharacter.mCurrentFloor++;
                mCharacter.mFallContinuityFloorCount++;
                CheckpointManager.Instance.mCurrentFloor = mCharacter.mCurrentFloor;
            }
        }
        mCharacter.transform.localPosition = pos;

        if (mCharacter.mFallContinuityFloorCount > AppConst.InvincibleTriggerFloor) {
            //播放无敌光效
        }
    }

    public override void End()
    {
        //清除无敌光效
        mIsEnd = true;
    }
}
