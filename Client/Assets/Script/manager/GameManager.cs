using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/************************************************************************************
* Copyright(c) 2019 All Rights Reserved.
*创建人： lyp
*创建时间：2019-03-13
*版本：1.0
*设计意图：游戏管理器
*=====================================================================
*修改时间：
*修改人：
*修改原因：
*修改内容：
************************************************************************************/

public class GameManager:SingleInstance<GameManager>
{
    public void Update() {
        CheckpointManager.Instance.Update();
    }
}
