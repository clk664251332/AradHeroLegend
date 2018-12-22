using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ItemCategory
{
    public string Name { get { return m_Name; } }

    public ItemData[] Items { get { return m_Items; } }

    [SerializeField]
    private string m_Name;

    [SerializeField]
    private ItemData[] m_Items;
}
public enum ItemMainType
{
    消耗品,材料,武器, 上衣,腰带,下装,鞋子,首饰
}
public enum ItemSubType
{
    药品,材料,
    光剑, 太刀, 短剑, 巨剑, 钝器,
    轻甲,重甲,板甲,皮甲,布甲,
    项链,手镯,戒指  
}
public enum ItemQuality
{
    普通,强化,稀有,罕见,史诗
}
/// <summary>
/// 仅仅代表物品的数据
/// </summary>
[Serializable]
public class ItemData
{
    public String Name;
    public int Id;
    //public String Name;
    public Sprite Icon;
    public String Descriptions;
    public int Price;
    public int NeedLevel=1;
    public int StackSize;
    public ItemQuality Quality;
    public ItemMainType MainType;
    public ItemSubType SubType;
    public List<ItemBaseProperty> BasePropertys;
    public List<ItemAdditionalProperty> AdditionalPropertys;
    public List<ItemSpecialProperty> SpecialPropertys;
    //public ItemProperty[] Propertys;
}
/// <summary>
/// 在虚无的数据的基础上添加元素构成了一个真正意义上有数量的物品
/// 是一个物品的最小单位，物品的交换，设置可以在上面一层进行了
/// </summary>
[Serializable]
public class SavableItem
{
    /// <summary>
    /// 添加数量变化的消息
    /// </summary>
    public Message StackChanged = new Message();
    public ItemData ItemData { get; private set; }

    //一些访问物品数据的接口
    public int Id { get { return ItemData.Id; } }
    public string Name { get { return ItemData.Name; } }
    public ItemQuality Quality { get { return ItemData.Quality; } }
    public ItemMainType MainType { get { return ItemData.MainType; } }
    public ItemSubType SubType { get { return ItemData.SubType; } }

    public List<ItemBaseProperty> BasePropertys { get { return ItemData.BasePropertys; } }
    public List<ItemAdditionalProperty> AdditionalPropertys { get { return ItemData.AdditionalPropertys; } }
    public List<ItemSpecialProperty> SpecialPropertys { get { return ItemData.SpecialPropertys; } }

    public int CurrentInStack { get { return m_CurrentInStack; } set { m_CurrentInStack = value; StackChanged.Send(); } }
    //public bool HasData { get { return ItemData != null; } }

    [SerializeField]
    private int m_CurrentInStack;

    public static implicit operator bool(SavableItem item) { return item != null; }
    //构造函数
    public SavableItem(ItemData data, int currentInStack = 1)
    {
        CurrentInStack = Mathf.Clamp(currentInStack, 1, data.StackSize);
        ItemData = data;
    }

    public void SetData(ItemData data)
    {
        ItemData = data;
    }
}

/// <summary>
/// 这是一个无形的，但是可以更换包含的物品的类，物品第有了与别的物品交换的行为。
/// </summary>
[Serializable]
public class ItemHolder
{
    /// <summary>
    /// 刷新的消息
    /// </summary>
    public Message<ItemHolder> Updated = new Message<ItemHolder>();
    //用完通知描述物品面板清空
    public Message setNull = new Message(); 

    public bool HasItem { get { return CurrentItem != null; } }
    public SavableItem CurrentItem;//{ get; private set; }
    public string CurrentPosition;

    //private SavableItem m_currentItem;
    public static implicit operator bool(ItemHolder holder)
    {
        return holder != null;
    }

    /// <summary>
    /// 添加物品,一般是ItemHolder还没有物体的时候，即CurrentItem为空
    /// </summary>
    /// <param name="itemData"></param>
    /// <param name="amount"></param>
    /// <param name="added"></param>
    /// <returns></returns>
    public bool TryAddItem(ItemData itemData, int amount, out int added)
    {
        added = 0;
        //如果已经有物体，还不是相同物体，直接跳过
        if (HasItem && itemData.Id != CurrentItem.Id)
            return false;
        //没有物体
        if (!HasItem)
        {
            CurrentItem = new SavableItem(itemData, 1);
            CurrentItem.CurrentInStack = 0;
            CurrentItem.StackChanged.AddListener(On_StackChanged);
        }
        int oldValue = CurrentItem.CurrentInStack;
        int surplus = amount + oldValue - itemData.StackSize;
        int currentInStack = oldValue;

        if (surplus <= 0)
            currentInStack += amount;
        else
            currentInStack = itemData.StackSize;

        CurrentItem.CurrentInStack = currentInStack;
        added = currentInStack - oldValue;

        Updated.Send(this);
        //如果添加了返回true
        return added > 0;
    }
    /// <summary>
    /// 设置物品，一般是ItemHolder已经有物体的时候
    /// </summary>
    /// <param name="item"></param>
    public void SetItem(SavableItem item)
    {
        if (CurrentItem)
        {
            //CurrentItem.PropertyChanged.RemoveListener(On_PropertyChanged);
            CurrentItem.StackChanged.RemoveListener(On_StackChanged);
        }

        CurrentItem = item;

        if (CurrentItem)
        {
            //CurrentItem.PropertyChanged.AddListener(On_PropertyChanged);
            CurrentItem.StackChanged.AddListener(On_StackChanged);
        }

        Updated.Send(this);
    }

    public bool AddItemInShop(ItemData data)
    {
        if (HasItem && data.Id != CurrentItem.Id)
            return false;

        if (!HasItem)
        {
            CurrentItem = new SavableItem(data, 1);
            Updated.Send(this);
            return true;
        }
        return false;
        
    }
    /// <summary>
    /// 
    /// </summary>
    public void RemoveFromStack(int amount, out int removed)
    {
        removed = 0;

        if (!HasItem)
            return;

        if (amount >= CurrentItem.CurrentInStack)
        {
            removed = CurrentItem.CurrentInStack;
            SetItem(null);
            setNull.Send();
            return;
        }

        int oldStack = CurrentItem.CurrentInStack;
        CurrentItem.CurrentInStack = Mathf.Max(CurrentItem.CurrentInStack - amount, 0);
        removed = oldStack - CurrentItem.CurrentInStack;

        Updated.Send(this);
    }

    /// <summary>
    /// 
    /// </summary>
    public void RemoveFromStack(int amount)
    {
        if (!HasItem)
            return;
        if (amount >= CurrentItem.CurrentInStack)
        {
            SetItem(null);
            setNull.Send();
            return;
        }
        int oldStack = CurrentItem.CurrentInStack;
        CurrentItem.CurrentInStack = Mathf.Max(CurrentItem.CurrentInStack - amount, 0);

        Updated.Send(this);
    }
    /// <summary>
    /// 交换物品
    /// </summary>
    /// <param name="item"></param>
    public void ExchangeItem(ItemHolder itemholder)
    {
        SavableItem temp;
        temp = CurrentItem;
        CurrentItem = itemholder.CurrentItem;
        itemholder.CurrentItem = temp;

        Updated.Send(this);
        itemholder.Updated.Send(itemholder);
    }

    /// <summary>
    /// 清空物体
    /// </summary>
    public void ClearItem()
    {
        SetItem(null);
        Updated.Send(this);
    }
    
    private void On_StackChanged()
    {
        Updated.Send(this);
    }
}