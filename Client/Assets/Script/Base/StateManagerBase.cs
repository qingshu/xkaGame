/************************************************************************************
* Copyright(c) 2019 All Rights Reserved.
*创建人： lyp
*创建时间：2019-03-23 19:40:35
*版本：1.0
*设计意图：设计意图：管理所有状态的整个生命周期，包括状态的开始、运行到结束
*=====================================================================
*修改时间：
*修改人：
*修改原因：
*修改内容：
************************************************************************************/

public abstract class StateManagerBase
{
    protected bool        mIsInit;            //是否已初始化
    protected StateBase[] mStates;            //所有状态
    protected int         mLastState;         //上一状态
    protected int         mCurrentState;      //当前状态

    //初始化所有状态
    public abstract void Init();

    //每一帧都调用的逻辑
    public void Update()
    {
        if (!mIsInit) {
            return;
        }

        //如果上一状态没结束，则先结束上一状态
        if (mLastState != mCurrentState)
        {
            if (!mStates[mLastState].IsEnd())
            {
                mStates[mLastState].End();
                return;
            }
            mLastState = mCurrentState;
        }

        //如果当前状态还没开始，则先开始
        if (!mStates[mCurrentState].IsStart())
        {
            mStates[mCurrentState].Start();
            return;
        }

        //运行状态
        mStates[mCurrentState].Run();
    }

    //切换状态
    public virtual void ChangeState(int state, ParamBase param)
    {
        //子类要先判断是否越界！
        mCurrentState = state;
        mStates[mCurrentState].Init(param);
    }
}