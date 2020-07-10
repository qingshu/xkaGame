/************************************************************************************
* Copyright(c) 2019 All Rights Reserved.
*创建人： lyp
*创建时间：2019-03-17
*版本：1.0
*设计意图：负责管理跳跳球的所有生命周期
*=====================================================================
*修改时间：
*修改人：
*修改原因：
*修改内容：
************************************************************************************/

using UnityEngine;

public enum CharacterStateEnum {
    Idle,        //待机  
    Born,        //出生
    Fall,        //下落
    Collide,     //碰撞
    BounceUp,    //弹起
    Invincible,  //无敌
    Died,        //死亡
    Max,
}

public class CharacterManager : StateManagerBase
{
    public override void Init()
    {
        //获取场景中"logic"对象
        GameObject logic = GameObject.Find("logic");
        if (null == logic)
        {
            LogManager.Instance.LogError("场景中不存在logic物体！！！");
            return;
        }

        //初始化角色  
        int characterId = PlayerPrefs.GetInt(AppConst.CurrentCharacterId, 1);
        Conf_Character conf_Character = DataManager.Instance.GetCharacterDataById(characterId);
        ResourceManager.Instance.LoadResByPath<GameObject>(new string[1] { AppConst.characterResPath + conf_Character.Model }, delegate (UnityEngine.Object[] objs)
        {
            if (objs.Length <= 0)
            {
                LogManager.Instance.LogError("加载小球失败：" + conf_Character.Model);
                return;
            }
            GameObject objCharacter = objs[0] as GameObject;
            if (null == objCharacter)
            {
                LogManager.Instance.LogError("加载小球失败：" + conf_Character.Model);
                return;
            }
            objCharacter.transform.parent = logic.transform;
            objCharacter.transform.localScale = Vector3.one * AppConst.CharacterScale;
            objCharacter.transform.localEulerAngles = AppConst.CharacterStartAngles;
            objCharacter.transform.localPosition = AppConst.CharacterStartPos;
            Character character = objCharacter.AddComponent<Character>();
            character.Init(this, conf_Character);

            //初始化角色所有状态
            mStates = new StateBase[(int)CharacterStateEnum.Max];
            mStates[(int)CharacterStateEnum.Idle] = new CharacterIdle(character);
            mStates[(int)CharacterStateEnum.Born] = new CharacterBorn(character);
            mStates[(int)CharacterStateEnum.Fall] = new CharacterFall(character);
            mStates[(int)CharacterStateEnum.Collide] = new CharacterCollide(character);
            mStates[(int)CharacterStateEnum.BounceUp] = new CharacterBounceUp(character);
            mStates[(int)CharacterStateEnum.Invincible] = new CharacterInvincible(character);
            mStates[(int)CharacterStateEnum.Died] = new CharacterDied(character);
            mCurrentState = (int)CharacterStateEnum.Born;
            mLastState = (int)CharacterStateEnum.Born;
            mIsInit = true;

        });
    }

    public override void ChangeState(int state,ParamBase param)
    {
        if (state >= (int)CharacterStateEnum.Max) {
            LogManager.Instance.LogError("切换小球状态异常：" + state);
            return;
        }
        base.ChangeState(state, param);
    }
}
