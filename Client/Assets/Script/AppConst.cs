/************************************************************************************
* Copyright(c) 2019 All Rights Reserved.
*创建人： lyp
*创建时间：2019-04-17 00:01:55
*版本：1.0
*设计意图：定义一些游戏常量
*=====================================================================
*修改时间：
*修改人：
*修改原因：
*修改内容：
************************************************************************************/
using UnityEngine;

public class AppConst
{
    //**************************************常量数据**********************************************
    public static readonly float   FloorStartYPos               = 16;    //踏板y轴起始位置
    public static readonly Vector3 CharacterStartPos            = new Vector3(0.3f, 17.25f, -3.84f);
    public static readonly Vector3 CharacterStartAngles         = new Vector3(-90, -180, 0);
    public static readonly int     CharacterScale               = 60;    //小球缩放
    public static readonly int     FloorSpace                   = 6;     //踏板间隔
    public static readonly int     PillarLengthOfOneFloor       = 350;   //一个踏板的柱子长度
    public static readonly float   PillarDownOffsetOfOneFloor   = 2.1f;  //一个踏板的柱子向下偏移长度
    public static readonly float   CharacterMoveSpeed           = 0.2f;  //角色移动速度
    public static readonly int     MaxBounceUpLength            = 4;     //弹起的最大高度
    public static readonly float   FallLength2CameraMove        = MaxBounceUpLength + CharacterMoveSpeed + 0.01f; //引起相机移动的下落距离
    public static readonly float   DragRate                     = 10f;   //滑动系数，数字越大滑动越不灵敏
    public static readonly int     InvincibleTriggerFloor       = 3;     //触发无敌的经过层数
    //**************************************常量名称**********************************************
    public static readonly string IsHadShownGuide    = "IsHadShownGuide";       //是否已显示新手引导
    public static readonly string CurrentLv          = "CurrentLv";             //当前关卡
    public static readonly string BestIntegral       = "BestIntegral";          //最佳积分
    public static readonly string CurrentCharacterId = "CurrentCharacterId";    //当前角色id

    //**************************************资源路径**********************************************
    // excel表格路径
    public static readonly string excelsFolderPath = Application.dataPath + "/data/";

    // excel工具存放序列化文件路径
    public static readonly string excelAssetsSavePath = "Assets/Resources/DataAssets/";

    //表格序列号资源根路径
    public static readonly string excelResPath = "DataAssets/ExcelData";

    //ui面板资源根路径
    public static readonly string uiPanelResPath = "UI/Prefab/";

    //踏板模型资源根路径
    public static readonly string floorResPath = "Models/Prefab/Floor/";

    //小球（角色）模型资源根路径
    public static readonly string characterResPath = "Models/Prefab/Character/";

    //光效资源根路径
    public static readonly string effectResPath = "Effect/Prefab/";

    //音效资源根路径
    public static readonly string soundResPath  = "Sound/Prefab/";
}
