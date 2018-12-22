using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// 管理背包众多格子的类（添加物品功能，整理功能）
/// </summary>
public class InventoryContainer : MonoBehaviour {

    //格子列表
    public List<Slot> Slots { get; private set; }

    public void Start()
    {
        if (Slots == null)
        {
            Slots = new List<Slot>();
            GetComponentsInChildren<Slot>(Slots);//获取格子列表
        }
        #region 给自己的格子打上地点标签用来区别
        //if (_Name == "背包")
        //{
        //    for (int i = 0; i < Slots.Count; i++)
        //    {
        //        Slots[i].SlotType = SlotType.背包;
        //    }
        //}
        //else if (_Name == "武器")
        //{
        //    for (int i = 0; i < Slots.Count; i++)
        //    {
        //        Slots[i].SlotType = SlotType.武器;
        //    }
        //}
        //else if (_Name == "上衣")
        //{
        //    for (int i = 0; i < Slots.Count; i++)
        //    {
        //        Slots[i].SlotType = SlotType.上衣;
        //    }
        //}
        //else if (_Name == "腰带")
        //{
        //    for (int i = 0; i < Slots.Count; i++)
        //    {
        //        Slots[i].SlotType = SlotType.腰带;
        //    }
        //}
        //else if (_Name == "下装")
        //{
        //    for (int i = 0; i < Slots.Count; i++)
        //    {
        //        Slots[i].SlotType = SlotType.下装;
        //    }
        //}
        //else if (_Name == "鞋子")
        //{
        //    for (int i = 0; i < Slots.Count; i++)
        //    {
        //        Slots[i].SlotType = SlotType.鞋子;
        //    }
        //}
        //else if (_Name == "项链")
        //{
        //    for (int i = 0; i < Slots.Count; i++)
        //    {
        //        Slots[i].SlotType = SlotType.项链;
        //    }
        //}
        //else if (_Name == "手镯")
        //{
        //    for (int i = 0; i < Slots.Count; i++)
        //    {
        //        Slots[i].SlotType = SlotType.手镯;
        //    }
        //}
        //else if (_Name == "戒指")
        //{
        //    for (int i = 0; i < Slots.Count; i++)
        //    {
        //        Slots[i].SlotType = SlotType.戒指;
        //    }
        //}
        //else if (_Name == "商店")
        //{
        //    for (int i = 0; i < Slots.Count; i++)
        //    {
        //        Slots[i].SlotType = SlotType.商店;
        //    }
        //}
        //else if (_Name == "技能")
        //{
        //    for (int i = 0; i < Slots.Count; i++)
        //    {
        //        Slots[i].SlotType = SlotType.技能;
        //    }
        //}
        #endregion
    }

    public bool HasItem(SavableItem item)
    {
        for (int i = 0; i < Slots.Count; i++)
            if (item == Slots[i].CurrentItem)
                return true;

        return false;
    }
    /// <summary>
    /// 尝试在这个格子集合中添加物品
    /// </summary>
    /// <param name="itemData"></param>
    /// <param name="amount"></param>
    /// <param name="added"></param>
    /// <returns></returns>
   public bool TryAddItem(ItemData itemData, int amount, out int added)
   {
       added = 0;
        //InventoryUtils.Instance.AddItem(itemData, amount, Slots, out added);
        //遍历所有的格子
        for (int i = 0; i < Slots.Count; i++)
        {
            int addedNow;
            Slots[i].ItemHolder.TryAddItem(itemData, amount, out addedNow);//amount=999,最多99
            added += addedNow;//99
            amount -= addedNow;//900
            // We added all the items, we can stop now.
            if (amount == 0)
                return added > 0;
        }
        Debug.Log(added);
        return added > 0;    
   }
}
