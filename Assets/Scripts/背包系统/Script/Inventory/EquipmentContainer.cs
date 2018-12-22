using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 管理装备众多格子的类
/// </summary>
public class EquipmentContainer : MonoBehaviour
{
    //格子列表
    public List<Slot> Slots { get; private set; }

    public void Awake()
    {
        if (Slots == null)
        {
            Slots = new List<Slot>();
            GetComponentsInChildren<Slot>(Slots);//获取格子列表
        }
    }
    //内部自带判断，所以参数可以是一切格子的ItemHolder
    public bool TryAddEquipment(ItemHolder itemHolder)
    {
        if (PlayerStateInfo.Instance.Level.Get() < itemHolder.CurrentItem.ItemData.NeedLevel)
        {
            Debug.Log("等级不够");
            return false;
        }
        if (itemHolder.CurrentItem.MainType == ItemMainType.武器)
        {
            Slot slot = GetSlot(ItemMainType.武器);
            if (slot.CurrentItem == null)
            {
                slot.ItemHolder.SetItem(itemHolder.CurrentItem);
                itemHolder.RemoveFromStack(1);
            }
            else
                slot.ItemHolder.ExchangeItem(itemHolder);
            return true;
        }
        else if (itemHolder.CurrentItem.MainType == ItemMainType.上衣)
        {
            Slot slot = GetSlot(ItemMainType.上衣);
            if (slot.CurrentItem == null)
            {
                slot.ItemHolder.SetItem(itemHolder.CurrentItem);
                itemHolder.RemoveFromStack(1);
            }
            else
                slot.ItemHolder.ExchangeItem(itemHolder);
            return true;
        }
        else if (itemHolder.CurrentItem.MainType == ItemMainType.下装)
        {
            Slot slot = GetSlot(ItemMainType.下装);
            if (slot.CurrentItem == null)
            {
                slot.ItemHolder.SetItem(itemHolder.CurrentItem);
                itemHolder.RemoveFromStack(1);
            }
            else
                slot.ItemHolder.ExchangeItem(itemHolder);
            return true;
        }
        else if (itemHolder.CurrentItem.MainType == ItemMainType.腰带)
        {
            Slot slot = GetSlot(ItemMainType.腰带);
            if (slot.CurrentItem == null)
            {
                slot.ItemHolder.SetItem(itemHolder.CurrentItem);
                itemHolder.RemoveFromStack(1);
            }
            else
                slot.ItemHolder.ExchangeItem(itemHolder);
            return true;
        }
        else if (itemHolder.CurrentItem.MainType == ItemMainType.鞋子)
        {
            Slot slot = GetSlot(ItemMainType.鞋子);
            if (slot.CurrentItem == null)
            {
                slot.ItemHolder.SetItem(itemHolder.CurrentItem);
                itemHolder.RemoveFromStack(1);
            }
            else
                slot.ItemHolder.ExchangeItem(itemHolder);
            return true;
        }
        else if (itemHolder.CurrentItem.SubType == ItemSubType.戒指)
        {
            Slot slot = GetSlot(ItemSubType.戒指);
            if (slot.CurrentItem == null)
            {
                slot.ItemHolder.SetItem(itemHolder.CurrentItem);
                itemHolder.RemoveFromStack(1);
            }
            else
                slot.ItemHolder.ExchangeItem(itemHolder);
            return true;
        }
        else if (itemHolder.CurrentItem.SubType == ItemSubType.手镯)
        {
            Slot slot = GetSlot(ItemSubType.手镯);
            if (slot.CurrentItem == null)
            {
                slot.ItemHolder.SetItem(itemHolder.CurrentItem);
                itemHolder.RemoveFromStack(1);
            }
            else
                slot.ItemHolder.ExchangeItem(itemHolder);
            return true;
        }
        else if (itemHolder.CurrentItem.SubType == ItemSubType.项链)
        {
            Slot slot = GetSlot(ItemSubType.项链);
            if (slot.CurrentItem == null)
            {
                slot.ItemHolder.SetItem(itemHolder.CurrentItem);
                itemHolder.RemoveFromStack(1);
            }
            else
                slot.ItemHolder.ExchangeItem(itemHolder);
            return true;
        }
        else//不是装备物品
        {
            return false;
        }
    }
    //获得特定类型的格子
    Slot GetSlot(ItemMainType type)
    {
        for (int i = 0; i < Slots.Count; i++)
        {
            if (Slots[i].SlotType.ToString() == type.ToString())
                return Slots[i];
        }
        return null;
    }

    Slot GetSlot(ItemSubType type)
    {
        for (int i = 0; i < Slots.Count; i++)
        {
            if (Slots[i].SlotType.ToString() == type.ToString())
                return Slots[i];
        }
        return null;
    }
}
