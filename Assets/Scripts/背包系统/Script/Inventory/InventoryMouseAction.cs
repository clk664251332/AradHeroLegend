using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
//用来处理背包UI物品的各种操作

public class InventoryMouseAction : MonoBehaviour {

    public Transform InventoryTrans;
    public Transform BottomSlotsTrans;
    public RectTransform PickedItemTrans;
    private Image PickedItemImage;
    bool picked;
    private SavableItem pickedItem;
    private Slot currentSlot;

    Slot[] slots;
    Slot[] bottomSlots;

    private void Awake()
    {
        slots = InventoryTrans.GetComponentsInChildren<Slot>();
        bottomSlots= BottomSlotsTrans.GetComponentsInChildren<Slot>();

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].PointerEnter += OnEnterSlot;
            slots[i].PointerExit += OnExitSlot;
            slots[i].PointerDown += OnDownSlot;
            slots[i].PointerUp += OnUpSlot;
        }
        for (int i = 0; i < bottomSlots.Length; i++)
        {
            bottomSlots[i].PointerEnter += OnEnterSlot;
            bottomSlots[i].PointerExit += OnExitSlot;
            bottomSlots[i].PointerDown += OnDownSlot;
            bottomSlots[i].PointerUp += OnUpSlot;
        }

    }

    void Start () {
        PickedItemImage = PickedItemTrans.GetComponent<Image>();
        picked = false;       
    }

    private void Update()
    {
        if (picked)
        {
            PickedItemTrans.position = Input.mousePosition;
        }
        else
        {
            PickedItemTrans.gameObject.SetActive(false);
            if(pickedItem!=null)
                pickedItem = null;
        }
    }
    //鼠标进入事件
    void OnEnterSlot(PointerEventData eventData, Slot slot)
    {
        if(pickedItem!=null)
        {
           //如果进入的是装备格子
           if(slot.SlotType != SlotType.商店 && slot.SlotType != SlotType.技能 && slot.SlotType != SlotType.背包)
            {
                if (pickedItem.ItemData.MainType.ToString()== slot.SlotType.ToString())
                {
                    //显示蓝色
                    slot.ShowBlueLight();
                }else
                {
                    //显示红色
                    slot.ShowRedLight();
                }
            }//其他格子正常显示蓝色
            else
            {
                slot.ShowBlueLight();
            }
        }
        else
        {
            slot.ShowBlueLight();
        }
        currentSlot = slot;
    }
    //鼠标退出事件
    void OnExitSlot(PointerEventData eventData, Slot slot)
    {
        slot.HideLight();
    }
    //鼠标点击事件
    void OnDownSlot(PointerEventData eventData, Slot slot)
    {
        if (slot.CurrentItem == null)
            return;
        //左键点击事件
        if(eventData.button== PointerEventData.InputButton.Left)
        {
            PickedItemTrans.gameObject.SetActive(true);
            PickedItemImage.sprite = slot.GetImage();
            pickedItem = slot.ItemHolder.CurrentItem;
            picked = true;

            currentSlot = slot;//储存格子的引用
            slot.HideImage();
        }
        //右键点击事件
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            //如果点击的是背包里的格子
            if(slot.SlotType==SlotType.背包)
            {
                //等级判断
                if (PlayerStateInfo.Instance.Level.Get()<slot.CurrentItem.ItemData.NeedLevel)
                {
                    Debug.Log("等级不够");
                    return;
                }
                if (slot.CurrentItem.MainType == ItemMainType.消耗品)//消耗品
                {
                    //使用消耗品，并显示特效
                    //应用属性
                    foreach(var property in slot.CurrentItem.BasePropertys)
                    {
                        if (property.BaseProperty == BasePropertyType.生命值)
                        {
                            if (PlayerStateInfo.Instance.HP.Get() == PlayerStateInfo.Instance.MaxHp.Get())
                                return;
                            PlayerStateInfo.Instance.HP.Set(PlayerStateInfo.Instance.HP.Get() + property.Value);
                            //显示加血特效
                            PlayerContorller.Instance.ShowHpEffect();
                        }
                        else if (property.BaseProperty == BasePropertyType.魔法值)
                        {
                            if (PlayerStateInfo.Instance.MP.Get() == PlayerStateInfo.Instance.MaxMp.Get())
                                return;
                            PlayerStateInfo.Instance.MP.Set(PlayerStateInfo.Instance.MP.Get() + property.Value);
                            //显示加蓝特效
                            PlayerContorller.Instance.ShowMpEffect();
                        }
                    }
                    slot.ItemHolder.RemoveFromStack(1);
                }
                else if (slot.CurrentItem.MainType != ItemMainType.材料)//装备
                {
                    //装备使用逻辑在EquipmentContainer中已有
                    InventoryController.Instance.EquipmentContainer.TryAddEquipment(slot.ItemHolder);
                }
            }//如果点击的是装备栏里的格子
            else if(slot.SlotType != SlotType.商店&& slot.SlotType != SlotType.技能)
            {
                //卸掉装备
                int added;
                InventoryController.Instance.InventoryContainer.TryAddItem(slot.CurrentItem.ItemData, 1, out added);
                slot.ItemHolder.RemoveFromStack(1);
            }
        }
    }

    //抬起鼠标事件
    void OnUpSlot(PointerEventData eventData, Slot slot)
    {
        if(eventData.button == PointerEventData.InputButton.Right)
            return;
        //落在背包上
        if (currentSlot.SlotType == SlotType.背包 && picked)
        {
            if (currentSlot.CurrentItem == null)
            {
                currentSlot.ItemHolder.SetItem(pickedItem);//空格子添加物品
                slot.ItemHolder.ClearItem();//旧格子清除物品
                //picked = false;
            }
            else if(currentSlot == slot)
            {
                slot.ShowImage();
            }
            else
            {
                slot.ItemHolder.ExchangeItem(currentSlot.ItemHolder);
                //picked = false;
            }
           
        }
        //落在装备格子上
        else if(currentSlot.SlotType != SlotType.商店&& currentSlot.SlotType != SlotType.技能 && picked)
        {
            if (!InventoryController.Instance.EquipmentContainer.TryAddEquipment(slot.ItemHolder))
            {
                slot.ShowImage();
            }
        }
        picked = false;
    }

}
