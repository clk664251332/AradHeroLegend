using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
/// <summary>
/// 使得我们的物品有了显示的途径，并有了拖拽的行为，即可以可视化操作了
/// </summary>
public class ShopSlot : MonoBehaviour
{
    public Message<ShopSlot> Refreshed = new Message<ShopSlot>();
    public delegate void PointerAction(PointerEventData data, ShopSlot wrapper);

    public PointerAction PointerClick;
    public PointerAction PointerDown;
    public PointerAction PointerUp;
    public PointerAction PointerEnter;
    public PointerAction PointerExit;

    [SerializeField]
    private Image m_ItemIcon;

    [SerializeField]
    private Image m_goldIcon;

    [SerializeField]
    private Text m_ItemName;

    [SerializeField]
    private Text m_ItemPrice;

    public ItemHolder ItemHolder { get; private set; }
    public bool HasItem { get { return ItemHolder && ItemHolder.HasItem; } }
    public SavableItem CurrentItem { get { return ItemHolder ? ItemHolder.CurrentItem : null; } }


    public void Init()
    {
        ItemHolder = new ItemHolder();
        m_ItemIcon.enabled = false;
        m_goldIcon.enabled = false;
        m_ItemName.enabled = false;
        m_ItemPrice.enabled = false;
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
        m_goldIcon.enabled = HasItem;
        m_ItemName.enabled = HasItem;
        m_ItemPrice.enabled = HasItem;

        if (m_ItemIcon.enabled)
            m_ItemIcon.sprite = CurrentItem.ItemData.Icon;

        if (m_ItemName.enabled)
            m_ItemName.text = string.Format("{0}", CurrentItem.ItemData.Name);

        if (m_ItemPrice.enabled)
        {
            m_ItemPrice.text = string.Format("{0}", CurrentItem.ItemData.Price);
            if (CurrentItem.ItemData.Price > PlayerStateInfo.Instance.Gold.Get())
                m_ItemPrice.color = Color.red;
            else
                m_ItemPrice.color = Color.white;
        }
       
        Refreshed.Send(this);
    }
    /// <summary>
    /// 清空格子
    /// </summary>
    public void Clear()
    {
        ItemHolder.ClearItem();
    }
}
