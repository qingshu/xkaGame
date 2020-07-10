using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/************************************************************************************
* Copyright(c) 2019 All Rights Reserved.
*创建人： lyp
*创建时间：2019-03-13
*版本：1.0
*设计意图：日志管理器,方便对日志进行统一地管理
*=====================================================================
*修改时间：
*修改人：
*修改原因：
*修改内容：
************************************************************************************/
public class LogManager : SingleInstance<LogManager>
{
    public void Log(object log) {
        Debug.Log(string.Format("{0:G} ", System.DateTime.Now) + log);
    }

    public void LogWarning(object log)
    {
        Debug.LogWarning(string.Format("{0:G} ", System.DateTime.Now) + log);
    }

    public void LogError(object log)
    {
        Debug.LogError(string.Format("{0:G} ", System.DateTime.Now) + log);
    }
}
