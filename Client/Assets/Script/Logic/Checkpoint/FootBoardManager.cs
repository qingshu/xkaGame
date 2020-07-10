/************************************************************************************
* Copyright(c) 2019 All Rights Reserved.
*创建人： lyp
*创建时间：2019-03-23 17:55:34
*版本：1.0
*设计意图：管理所有踏板的整个生命周期
*=====================================================================
*修改时间：
*修改人：
*修改原因：
*修改内容：
************************************************************************************/ 
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FootBoardManager
{
    private Dictionary<int, List<Conf_Floor>> dicFloorData;
    private List<GameObject> listFootBoard;

    // 初始化踏板
    public void Init(GameObject LogicObjOfScene,int currentLv)
    {
        Conf_Checkpoint checkpoint = DataManager.Instance.GetCheckpointDataById(currentLv);       
        Transform floorParent = LogicObjOfScene.transform.Find("RoteObj");
        List<string> floorPaths = new List<string>();
        listFootBoard = new List<GameObject>();

        //获取开始踏板配置
        Conf_Floor startFloor = DataManager.Instance.GetFloorDatasById(checkpoint.StartFloorId);
        floorPaths.Add(AppConst.floorResPath + startFloor.Model);

        //根据配表获取各个难度的踏板配置        
        for (int i = 0; i < checkpoint.Difficulty.Length; i++) {
            //根据类型生成对应数量的踏板
            int difficultCount = checkpoint.Difficulty[i];
            if (difficultCount <= 0) {
                continue;
            }

            //根据类型获取踏板
            List<Conf_Floor> listFloor = this.GetFloorDatasByType(i + 1);
            int count = listFloor.Count;
            for (int j = 0; j < difficultCount; j++) {
                int index = Random.Range(0, count);
                Conf_Floor floor = listFloor[index];
                floorPaths.Add(AppConst.floorResPath + floor.Model);
            }                    
        }

        //获取结束踏板配置
        Conf_Floor endFloor = DataManager.Instance.GetFloorDatasById(checkpoint.EndFloorId);
        floorPaths.Add(AppConst.floorResPath + endFloor.Model);

        //根据踏板配置生成所有踏板
        ResourceManager.Instance.LoadResByPath<GameObject>(floorPaths.ToArray(), delegate (UnityEngine.Object[] objs) {
            for (int j = 0; j < objs.Length; j++)
            {
                GameObject obj = objs[j] as GameObject;
                obj.transform.parent = floorParent;
                obj.transform.localScale = Vector3.one;
                if (j == 0)
                {
                    obj.transform.localEulerAngles = Vector3.zero;
                }
                else
                {
                    int angles = Random.Range(0, 360);
                    obj.transform.localEulerAngles = Vector3.up * angles;
                }              
                obj.transform.localPosition = Vector3.up * (AppConst.FloorStartYPos - AppConst.FloorSpace * j);
                listFootBoard.Add(obj);
            }
        });

        //根据踏板设置柱子长度
        Transform transPillar = LogicObjOfScene.transform.Find("LiZhu");
        if (null == transPillar)
        {
            LogManager.Instance.LogError("柱子不存在！！！");
            return;
        }
        transPillar.localScale = new Vector3(180, 180, AppConst.PillarLengthOfOneFloor * floorPaths.Count);
        transPillar.localPosition = Vector3.down * AppConst.PillarDownOffsetOfOneFloor * floorPaths.Count;
    }

    //隐藏踏板
    public void HideFloor(int hideFloor)
    {
        if (hideFloor >= listFootBoard.Count) {
            return;
        }
        GameObject obj = listFootBoard[hideFloor];
        obj.SetActive(false);
    }

    //根据踏板id获取踏板数据
    private List<Conf_Floor> GetFloorDatasByType(int type)
    {
        if (null != dicFloorData) {
            return dicFloorData[type];
        }

        //处理数据
        dicFloorData = new Dictionary<int, List<Conf_Floor>>();
        var floorDatas = DataManager.Instance.GetFloorDatas();
        foreach(var item in floorDatas)
        {
            Conf_Floor floor = item.Value;
            if (!dicFloorData.ContainsKey(floor.Type))
            {
                dicFloorData[floor.Type] = new List<Conf_Floor>();
            }
            dicFloorData[floor.Type].Add(floor);
        }
        return dicFloorData[type];
    }
}
