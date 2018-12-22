using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardItemSlot : MonoBehaviour {

    public Image itemImage;
    public Text itemAmountText;

    private void Start()
    {
        itemImage.enabled = false;
        itemAmountText.enabled = false;
    }
    public void SetItemImage(Sprite icon)
    {
        itemImage.enabled = true;
        itemImage.sprite = icon;
    }

    public void SetItemAmount(int amount)
    {
        itemAmountText.enabled = true;
        itemAmountText.text = "X"+amount.ToString();
    }

    public void ResetSlot()
    {
        itemImage.enabled = false;
        itemAmountText.enabled = false;
    }

}
