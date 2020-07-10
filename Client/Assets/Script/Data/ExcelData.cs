/************************************************************************************
* Copyright(c) 2019 All Rights Reserved.
*创建人： 
*创建时间：2019-04-09 18:02:55
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

public class ExcelData :ScriptableObject
{
    public Conf_Checkpoint[] checkPointDatas;
    public Conf_Floor[] floorDatas;
    public Conf_Character[] characterDatas;
}

//关卡
[System.Serializable]
public class Conf_Checkpoint
{
    public int ID;            //id
    public int Integral;      //一个踏板获得的分数
    public int StartFloorId;  //开始踏板id
    public int[] Difficulty;  //难度
    public int EndFloorId;    //结束踏板id
}

//踏板
[System.Serializable]
public class Conf_Floor
{
    public int ID;             //id
    public int Type;           //难度
    public string Model;       //模型
}

//角色
[System.Serializable]
public class Conf_Character
{
    public int ID;
    public string Model;
    public int Speed;
    public string BounceUpEffect;
    public string DropEffect;
    public string CollisionEffect;
    public string InvincibleEffect;
    public string DeadEffect;
    public string CollisionSound;
}
