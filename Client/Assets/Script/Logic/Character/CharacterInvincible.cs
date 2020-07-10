/************************************************************************************
* Copyright(c) 2019 All Rights Reserved.
*创建人： lyp
*创建时间：2019-04-21 16:10:02
*版本：1.0
*设计意图：角色（小球）无敌状态
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

//无敌参数
public class CharacterInvincibleParam : ParamBase{
    public float fallLength;
    public int   canFallFloorCount;
}

public class CharacterInvincible : StateBase
{
    private float mFallLength;
    private int   mCanFallFloorCount;
    public CharacterInvincible(Character character) : base(character)
    {
        
    }

    public override void Init(ParamBase param)
    {       
        CharacterInvincibleParam invincibleParam = (CharacterInvincibleParam)param;
        if (null == invincibleParam)
        {
            LogManager.Instance.LogError("无敌参数异常");
            return;
        }
        mFallLength = invincibleParam.fallLength;
        mCanFallFloorCount = invincibleParam.canFallFloorCount;
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
        if (mFallLength > AppConst.FallLength2CameraMove)
        {
            Vector3 cameraPos = Camera.main.transform.localPosition;
            cameraPos.y -= AppConst.CharacterMoveSpeed;
            Camera.main.transform.localPosition = cameraPos;

            //判断是否改变层级
            float floorPos = AppConst.FloorStartYPos - AppConst.FloorSpace * mCharacter.mCurrentFloor;
            if (floorPos > pos.y)
            {
                mCharacter.mCurrentFloor++;
                mCanFallFloorCount--;
                mCharacter.mFallContinuityFloorCount++;
                CheckpointManager.Instance.mCurrentFloor = mCharacter.mCurrentFloor;
            }
        }
        mCharacter.transform.localPosition = pos;

        //判断无敌状态是否消失
        if (mCanFallFloorCount <= 0) {
            CharacterFallParam fallParam = new CharacterFallParam();
            fallParam.fallLength = mFallLength;
            mCharacter.ChangeState((int)CharacterStateEnum.Fall, fallParam);
        }
    }

    public override void End()
    {
        mIsEnd = true;
    }
}
