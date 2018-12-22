using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmountSelectController : MonoBehaviour {

    public Text AmountText;
    int amount = 1;

    private void Start()
    {
        if (gameObject.activeSelf)
            gameObject.SetActive(false);
    }
    private void Update()
    {
        AmountText.text = amount.ToString();
    }

    //确认按钮
    public void OnClickOK()
    {
        gameObject.SetActive(false);
        if (ShopController.Instance.CurSlot != null)
        {
            if(PlayerStateInfo.Instance.Gold.Get() - ShopController.Instance.CurSlot.CurrentItem.ItemData.Price * amount<0)
            {
                Debug.Log("金钱不够");
            }
            else
            {
                int added;
                InventoryController.Instance.InventoryContainer.TryAddItem(
                    ShopController.Instance.CurSlot.CurrentItem.ItemData, amount, out added);
                PlayerStateInfo.Instance.Gold.Set(PlayerStateInfo.Instance.Gold.Get() - ShopController.Instance.CurSlot.CurrentItem.ItemData.Price * amount);
                ShopController.Instance.CurSlot = null;
            }
        }
        amount = 1;
    }
    //取消按钮
    public void OnClickCancel()
    {
        ShopController.Instance.CurSlot = null;
        gameObject.SetActive(false);
        amount = 1;
    }
    //+1按钮
    public void AddOne()
    {
        amount++;
    }
    //-1按钮
    public void MinusOne()
    {
        amount--;
        if (amount < 1)
            amount = 1;
    }
    //+10按钮
    public void AddTen()
    {
        amount += 10;
    }
    //-10按钮
    public void MinusTen()
    {
        amount -= 10;
        if (amount < 1)
            amount = 1;
    }
}
