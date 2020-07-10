/************************************************************************************
* Copyright(c) 2019 All Rights Reserved.
*创建人： 
*创建时间：2019-04-21 18:16:48
*版本：1.0
*设计意图：
*=====================================================================
*修改时间：
*修改人：
*修改原因：
*修改内容：
************************************************************************************/ 
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//UI事件系统
public class UIEventSystem : MonoBehaviour, IDragHandler
{
    private Transform roteObj;
    void Awake()
    {
        Image image = GetComponent<Image>();
        if (null == image)
        {
            image = gameObject.AddComponent<Image>();
        }
        image.color = new Color(255, 255, 255, 0);
        RectTransform rectTrans = GetComponent<RectTransform>();
        rectTrans.sizeDelta = new Vector2(10000, 10000);       
    }

    //拖拽
    public void OnDrag(PointerEventData eventData)
    {
        if (null == roteObj) {
            GameObject logic = GameObject.Find("logic");
            if (null == logic)
            {
                return;
            }
            roteObj = logic.transform.Find("RoteObj");
            return;
        }
        Vector3 angle = roteObj.localEulerAngles;
        float deta = (eventData.delta.x / AppConst.DragRate);
        angle.y -= deta;
        roteObj.localEulerAngles = angle;
    }
}
