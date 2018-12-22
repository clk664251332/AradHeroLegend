using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Text;

public class ShopToolTip : MonoBehaviour {

    public Transform ShopSlotsTrans;

    public Text title;
    public Text type;
    public Text quality;
    public Text level;
    public Text price;
    public Text baseProperty;
    public Text additionalProperty;
    public Text specialProperty;
    public Text description;

    private Image image;
    private RectTransform rectTrans;
    private RectTransform textRectTrans;

    ShopSlot[] shopSlots;

    ShopSlot tempSlot;

    void Start()
    {
        rectTrans = GetComponent<RectTransform>();
        shopSlots = ShopSlotsTrans.GetComponentsInChildren<ShopSlot>();

        for (int i = 0; i < shopSlots.Length; i++)
        {
            shopSlots[i].PointerEnter += OnEnterSlot;
            shopSlots[i].PointerExit += OnExitSlot;
            //shopSlots[i].PointerDown += OnDowntSlot;
            //shopSlots[i].PointerUp += OnUpSlot;
        }
    }

    //再次显示的时候，进行加载文字,并更改坐标
    private void OnEnable()
    {

    }
    private void Update()
    {
        if (tempSlot == null)
            transform.position = new Vector2(80, -500);
        else
        {
            Canvas.ForceUpdateCanvases();
            SetPotion(tempSlot);
            Canvas.ForceUpdateCanvases();
        }
    }
    //回调函数
    void OnEnterSlot(PointerEventData eventData, ShopSlot slot)
    {
        if (slot.CurrentItem == null)
            return;
        tempSlot = slot;
        LoadText(slot);
    }
    //回调函数
    void OnExitSlot(PointerEventData eventData, ShopSlot slot)
    {
        tempSlot = null;
        //transform.position = new Vector2(80, -500);
    }
    //点击格子对ToolTip的影响
    void OnDowntSlot(PointerEventData eventData, ShopSlot slot)
    {
        //if (slot.CurrentItem == null)
        //    return;
        ////如果按的是左键直接隐藏掉。 
        //if (eventData.button == PointerEventData.InputButton.Left)
        //    //gameObject.SetActive(!gameObject.activeSelf);
        //    gameObject.SetActive(false);
        //else
        //{
        //    tempSlot = slot;
        //    LoadText(slot);
        //    //gameObject.SetActive(!gameObject.activeSelf);
        //}
    }
    //回调函数
    void OnUpSlot(PointerEventData eventData, ShopSlot slot)
    {
        //if (eventData.button == PointerEventData.InputButton.Left)
        //    //gameObject.SetActive(!gameObject.activeSelf);
        //    gameObject.SetActive(true);
        //if (tempSlot != null)
        //    LoadText(tempSlot);
    }

    //当格子变空的时候
    void OnSlotEmpty()
    {
        tempSlot = null;
    }

    void LoadText(ShopSlot slot)
    {
        title.text = slot.CurrentItem.Name;
        type.text = slot.CurrentItem.MainType + "-" + slot.CurrentItem.SubType;
        quality.text = slot.CurrentItem.Quality.ToString();
        if (PlayerStateInfo.Instance.Level.Get() < slot.CurrentItem.ItemData.NeedLevel)
            level.color = Color.red;
        else
            level.color = Color.white;
        level.text = "需要等级：" + slot.CurrentItem.ItemData.NeedLevel;

        if (PlayerStateInfo.Instance.Gold.Get() < slot.CurrentItem.ItemData.Price)
            price.color = Color.red;
        else
            price.color = Color.white;
        price.text = slot.CurrentItem.ItemData.Price.ToString() + "金币";
        //基础属性
        if (GetBaseProperty(slot) == null)
            baseProperty.text = "";
        else
        {
            baseProperty.text = GetBaseProperty(slot);
        }
        //附加属性
        if (GetAdditionalProperty(slot) == null)
            additionalProperty.text = "";
        else
        {
            additionalProperty.text = GetAdditionalProperty(slot);
        }
        //特殊属性
        if (GetSpecialProperty(slot) == null)
            specialProperty.text = "";
        else
        {
            specialProperty.text = GetSpecialProperty(slot);
        }

        description.text = slot.CurrentItem.ItemData.Descriptions;
        // Canvas.ForceUpdateCanvases();
    }

    void SetPotion(ShopSlot slot)
    {
        if (Input.mousePosition.x <= 400)
            transform.position = new Vector2(slot.transform.position.x + 69 + rectTrans.rect.width / 2,
              Mathf.Clamp(slot.transform.position.y, rectTrans.rect.height / 2, 600 - rectTrans.rect.height / 2));
        else
            transform.position = new Vector2(slot.transform.position.x - 69 - rectTrans.rect.width / 2,
                Mathf.Clamp(slot.transform.position.y, rectTrans.rect.height / 2, 600 - rectTrans.rect.height / 2));
        //动态设置高度
        //rectTrans.sizeDelta = new Vector2(rectTrans.sizeDelta.x, textRectTrans.sizeDelta.y + 10);
    }

    //获得基础属性
    private string GetBaseProperty(ShopSlot slot)
    {
        if (slot.CurrentItem.BasePropertys.Count == 0)
            return null;
        StringBuilder description = new StringBuilder();
        foreach (var i in slot.CurrentItem.BasePropertys)
        {
            description.Append(i.BaseProperty.ToString() + "+" + i.Value + "\n");
        }
        return description.ToString();
    }
    //获得附加属性
    private string GetAdditionalProperty(ShopSlot slot)
    {
        if (slot.CurrentItem.AdditionalPropertys.Count == 0)
            return null;
        StringBuilder description = new StringBuilder();
        foreach (var i in slot.CurrentItem.AdditionalPropertys)
        {
            description.Append(i.AdditionalProperty.ToString() + "+" + i.Value + "\n");
        }
        return description.ToString();
    }
    //获得特殊属性
    private string GetSpecialProperty(ShopSlot slot)
    {
        if (slot.CurrentItem.SpecialPropertys.Count == 0)
            return null;
        StringBuilder description = new StringBuilder();
        foreach (var i in slot.CurrentItem.SpecialPropertys)
        {
            description.Append(i.SpecialProperty.ToString() + "+" + i.Value + "\n");
        }
        return description.ToString();
    }
}
