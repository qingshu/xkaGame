/************************************************************************************
* Copyright(c) 2019 All Rights Reserved.
*创建人： lyp
*创建时间：2019-04-21 16:08:17
*版本：1.0
*设计意图：角色（小球）碰撞状态
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

public class CharacterCollide : StateBase
{
    public CharacterCollide(Character character) : base(character)
    {
        
    }

    public override void Start()
    {
        mIsStart = true;
    }

    public override void Run()
    {
        mCharacter.ChangeState((int)CharacterStateEnum.BounceUp,null);
    }

    public override void End()
    {
        mIsEnd = true;
    }
}
