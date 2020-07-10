/************************************************************************************
* Copyright(c) 2019 All Rights Reserved.
*创建人： lyp
*创建时间：2019-03-17
*版本：1.0
*设计意图：状态基类
*=====================================================================
*修改时间：
*修改人：
*修改原因：
*修改内容：
************************************************************************************/
using UnityEngine;
public abstract class StateBase
{
    protected bool mIsStart;        //是否开始
    protected bool mIsEnd;          //是否结束
    protected Character mCharacter; //角色

    public StateBase(Character character) {
        mIsStart = false;
        mIsEnd   = false;
        mCharacter = character;
    }

    //重置状态数据(构造函数和切换状态的时候调用)
    public virtual void Init(ParamBase param) {
        mIsStart = false;
        mIsEnd = false;
    }

    //是否开始
    public bool IsStart()
    {
        return mIsStart;
    }

    //开始
    public abstract void Start();

    //运行中
    public abstract void Run();

    //是否结束
    public bool IsEnd()
    {
        return mIsEnd;
    }

    //结束
    public abstract void End();
}
