using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopSlotAction : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    ShopSlot slot;
    public Image Light;
    private void Awake()
    {
        slot = GetComponent<ShopSlot>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (slot.PointerClick != null && slot.CurrentItem != null)
            slot.PointerClick(eventData, slot);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (slot.PointerEnter != null && slot.CurrentItem != null)
            slot.PointerEnter(eventData, slot);

        Light.enabled = slot.HasItem;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (slot.PointerExit != null)
            slot.PointerExit(eventData, slot);

        Light.enabled = false;
    }
}
