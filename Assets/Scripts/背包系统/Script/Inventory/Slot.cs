using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum SlotType
{
    背包,
    武器,上衣,腰带,下装,鞋子,项链,手镯,戒指,
    商店,
    技能
}
/// <summary>
/// 使得我们的物品有了显示的途径，并有了拖拽的行为，即可以可视化操作了
/// </summary>
public class Slot : MonoBehaviour
{
    public Message<Slot> Refreshed = new Message<Slot>();
    public delegate void PointerAction(PointerEventData data, Slot wrapper);
    public SlotType SlotType;

    public PointerAction PointerDown;
    public PointerAction PointerUp;
    public PointerAction PointerEnter;
    public PointerAction PointerExit;

    [SerializeField]
    private Image m_ItemIcon;
    [SerializeField]
    private Text m_StackDisplayer;

    public Image blueLight;
    public Image errorLight;

    public ItemHolder ItemHolder { get; private set; }
    public bool HasItem { get { return ItemHolder && ItemHolder.HasItem; } }
    public SavableItem CurrentItem { get { return ItemHolder ? ItemHolder.CurrentItem : null; } }


    protected void Awake()
    {
        ItemHolder = new ItemHolder();
        m_ItemIcon.enabled = false;
        m_StackDisplayer.enabled = false;
        ItemHolder.Updated.AddListener(On_ItemHolder_Updated);
    }

    private void On_ItemHolder_Updated(ItemHolder holder)
    {
        Refresh();
    }
    /// <summary>
    /// 更新UI的显示
    /// </summary>
    public void Refresh()
    {
        m_ItemIcon.enabled = HasItem;
        m_StackDisplayer.enabled = HasItem && CurrentItem.CurrentInStack > 1;

        if (m_ItemIcon.enabled)
            m_ItemIcon.sprite = CurrentItem.ItemData.Icon;

        if (m_StackDisplayer.enabled)
            m_StackDisplayer.text = string.Format("x{0}", CurrentItem.CurrentInStack);

        Refreshed.Send(this);
    }
    /// <summary>
    /// 清空格子
    /// </summary>
    public void Clear()
    {
        ItemHolder.ClearItem();
    }

    public Sprite GetImage()
    {
        return m_ItemIcon.sprite;
    }
    public void HideImage()
    {
        m_ItemIcon.enabled = false;
    }

    public void ShowImage()
    {
        m_ItemIcon.enabled = true;
    }
#region 获取属性数值的函数
    //获取各种属性数值的函数
    public int Get_StrengthValue()
    {
        if (CurrentItem == null)
            return 0;
        foreach (ItemAdditionalProperty temp in CurrentItem.ItemData.AdditionalPropertys)
        {
            if (temp.AdditionalProperty == AdditionalPropertyType.力量)
            {
                return temp.Value;
            }
        }
        return 0;
    }
    //获取各种属性数值的函数
    public int Get_EnduranceValue()
    {
        if (CurrentItem == null)
            return 0;
        foreach (ItemAdditionalProperty temp in CurrentItem.ItemData.AdditionalPropertys)
        {
            if (temp.AdditionalProperty == AdditionalPropertyType.体力)
            {
                return temp.Value;
            }
        }
        return 0;
    }
    //获取各种属性数值的函数
    public int Get_IntelligenceValue()
    {
        if (CurrentItem == null)
            return 0;
        foreach (ItemAdditionalProperty temp in CurrentItem.ItemData.AdditionalPropertys)
        {
            if (temp.AdditionalProperty == AdditionalPropertyType.智力)
            {
                return temp.Value;
            }
        }
        return 0;
    }
    //获取各种属性数值的函数
    public int Get_SpiritValue()
    {
        if (CurrentItem == null)
            return 0;
        foreach (ItemAdditionalProperty temp in CurrentItem.ItemData.AdditionalPropertys)
        {
            if (temp.AdditionalProperty == AdditionalPropertyType.精神)
            {
                return temp.Value;
            }
        }
        return 0;
    }
    //获取各种属性数值的函数
    public int Get_PhysicalAttackValue()
    {
        if (CurrentItem == null)
            return 0;
        foreach (ItemBaseProperty temp in CurrentItem.ItemData.BasePropertys)
        {
            if (temp.BaseProperty == BasePropertyType.物理攻击)
            {
                return temp.Value;
            }
        }
        return 0;
    }
    //获取各种属性数值的函数
    public int Get_PhysicalDefenceValue()
    {
        if (CurrentItem == null)
            return 0;
        foreach (ItemBaseProperty temp in CurrentItem.ItemData.BasePropertys)
        {
            if (temp.BaseProperty == BasePropertyType.物理防御)
            {
                return temp.Value;
            }
        }
        return 0;
    }
    //获取各种属性数值的函数
    public int Get_MagicAttackValue()
    {
        if (CurrentItem == null)
            return 0;
        foreach (ItemBaseProperty temp in CurrentItem.ItemData.BasePropertys)
        {
            if (temp.BaseProperty == BasePropertyType.魔法攻击)
            {
                return temp.Value;
            }
        }
        return 0;
    }
    //获取各种属性数值的函数
    public int Get_MagicDefenceValue()
    {
        if (CurrentItem == null)
            return 0;
        foreach (ItemBaseProperty temp in CurrentItem.ItemData.BasePropertys)
        {
            if (temp.BaseProperty == BasePropertyType.魔法防御)
            {
                return temp.Value;
            }
        }
        return 0;
    }
    #endregion
    public void ShowBlueLight()
    {
        blueLight.enabled = true;
        errorLight.enabled = false;
    }

    public void ShowRedLight()
    {
        blueLight.enabled = false;
        errorLight.enabled = true;
    }

    public void HideLight()
    {
        blueLight.enabled = false;
        errorLight.enabled = false;
    }
}
