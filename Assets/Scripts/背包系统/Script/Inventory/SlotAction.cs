using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class SlotAction : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerDownHandler,IPointerUpHandler
{
    Slot slot;
    //public Image Light;
    //public Image errorLight;
    private void Awake()
    {
        slot = GetComponent<Slot>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //注意在ToolTip类里面写上方法，再监听PointerEnter委托 
        if (slot.PointerEnter != null)
            slot.PointerEnter(eventData, slot);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (slot.PointerExit != null)
            slot.PointerExit(eventData, slot);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (slot.PointerDown != null)
            slot.PointerDown(eventData, slot);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (slot.PointerUp != null)
            slot.PointerUp(eventData, slot);
    }

    
}
