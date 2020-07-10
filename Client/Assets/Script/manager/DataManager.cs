/************************************************************************************
* Copyright(c) 2019 All Rights Reserved.
*创建人： lyp
*创建时间：2019-04-14 12:04:19
*版本：1.0
*设计意图：策划表格数据管理器
*=====================================================================
*修改时间：
*修改人：
*修改原因：
*修改内容：
************************************************************************************/ 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : SingleInstance<DataManager>
{
    private Dictionary<int, Conf_Checkpoint> dicCheckpointData;
    private Dictionary<int, Conf_Floor> dicFloorData;
    private Dictionary<int, Conf_Character> dicCharacterData;

    protected override void Init() {
        ExcelData excelData = Resources.Load<ExcelData>(AppConst.excelResPath);
        if (null == excelData) {
            LogManager.Instance.LogError("加载表格数据失败！！！" + AppConst.excelResPath);
            return;
        }

        //处理关卡数据
        dicCheckpointData = new Dictionary<int, Conf_Checkpoint>();
        for (int i = 0; i < excelData.checkPointDatas.Length; i++) {
            Conf_Checkpoint checkPoint = excelData.checkPointDatas[i];
            dicCheckpointData[checkPoint.ID] = checkPoint;
        }

        //处理踏板数据
        dicFloorData = new Dictionary<int, Conf_Floor>();
        for (int i = 0; i < excelData.floorDatas.Length; i++)
        {
            Conf_Floor floor = excelData.floorDatas[i];
            dicFloorData[floor.ID] = floor;
        }

        //处理角色数据
        dicCharacterData = new Dictionary<int, Conf_Character>();
        for (int i = 0; i < excelData.characterDatas.Length; i++)
        {
            Conf_Character character = excelData.characterDatas[i];
            dicCharacterData[character.ID] = character;
        }

        //释放资源
        Resources.UnloadAsset(excelData);
        LogManager.Instance.Log("数据管理器初始化完毕");
    }

    //获取关卡数据
    public Dictionary<int, Conf_Checkpoint> GetCheckpointDatas() {
        return dicCheckpointData;
    }

    //根据关卡id获取关卡数据
    public Conf_Checkpoint GetCheckpointDataById(int id)
    {
        return dicCheckpointData[id];
    }

    //获取踏板数据
    public Dictionary<int, Conf_Floor> GetFloorDatas()
    {
        return dicFloorData;
    }

    //根据踏板id获取踏板数据
    public Conf_Floor GetFloorDatasById(int id)
    {
        return dicFloorData[id];
    }

    //获取角色数据
    public Dictionary<int, Conf_Character> GetCharacterDatas()
    {
        return dicCharacterData;
    }

    //根据角色id获取角色数据
    public Conf_Character GetCharacterDataById(int id)
    {
        return dicCharacterData[id];
    }
}
