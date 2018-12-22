using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShopItemContainer : MonoBehaviour
{

    //public string Name { get { return _Name; } }
    //格子列表
    public List<ShopSlot> Slots { get; private set; }

    //[SerializeField]
    //private string _Name = "";

    //private List<ItemHolder> m_ItemHolders;
    //private List<Slot> m_Slots;

    public void Init()
    {
        //Debug.Log("我还没被激活，调用");
        if (Slots == null)
        {
            Slots = new List<ShopSlot>();
            GetComponentsInChildren<ShopSlot>(Slots);//获取格子列表
        }
        //Debug.Log(Slots.Count);
        for (int i = 0; i < Slots.Count; i++)
        {
            Slots[i].Init();
        }
    }
    public void ClearAll()
    {
        for (int i = 0; i < Slots.Count; i++)
        {
            Slots[i].Clear();
        }
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
    public void TryAddItem(ItemData itemData)
    {
       //遍历所有的格子
       for (int i = 0; i < Slots.Count; i++)
       {
            if (Slots[i].ItemHolder.AddItemInShop(itemData))
                return;
       }
    }
}
