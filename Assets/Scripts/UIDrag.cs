using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIDrag : MonoBehaviour,IPointerDownHandler,IDragHandler {

    Vector3 startPos;
    Vector3 uiPos;
    Vector3 offset;
    RectTransform RectTrans;

    public void OnPointerDown(PointerEventData eventData)
    {
        startPos = Input.mousePosition;
        RectTrans = transform.parent.parent.GetComponent<RectTransform>();
        //uiPos = transform.parent.GetComponent<RectTransform>().position;
        uiPos = RectTrans.position;
        offset = startPos - uiPos;
        //transform.parent.GetComponent<RectTransform>().SetAsLastSibling();
        RectTrans.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        //transform.parent.GetComponent<RectTransform>().position = Input.mousePosition - offset;
        RectTrans.position = Input.mousePosition - offset;
    }
}
