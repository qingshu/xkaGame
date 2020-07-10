/************************************************************************************
* Copyright(c) 2019 All Rights Reserved.
*创建人： lyp
*创建时间：2019-03-13
*版本：1.0
*设计意图：泛型单例
*=====================================================================
*修改时间：
*修改人：
*修改原因：
*修改内容：
************************************************************************************/

public class SingleInstance<T> where T:class, new()
{
    private static T m_instance = default;
    public static T Instance {
        get {
            if (null == m_instance) {
                m_instance = new T();
                (m_instance as SingleInstance<T>).Init();
            }          
            return m_instance;
        }
    }

    public static void DestroyInstance() {
        if (null == m_instance) {
            return;
        }
        (m_instance as SingleInstance<T>).Dispose();
        m_instance = null;
    }

    protected virtual void  Init() { }
    protected virtual void  Dispose() { }
}
