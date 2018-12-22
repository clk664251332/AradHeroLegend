using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddItemController : MonoBehaviour {

    public InputField ItemID;
    public InputField Amount;
    public InputField ExpAdd;

    public GameObject AddItemUI;
    public GameObject Monster;

    private void Start()
    {
        AddItemUI.SetActive(false);    
    }

    public void AddItem()
    {
        if(ItemID.text!=""& Amount.text != "")
        {
            int added;
            ItemData item = InventoryController.Instance.GetItemById(int.Parse(ItemID.text));
            InventoryController.Instance.InventoryContainer.TryAddItem(item, int.Parse(Amount.text), out added);
        }
        if(ExpAdd.text != "")
        {
            PlayerStateInfo.Instance.AddExp(int.Parse(ExpAdd.text));
        }
    }

    public void AddMonster()
    {
        Vector3 playerPosition = PlayerContorller.Instance.transform.position;
        Vector3 newPosition = new Vector3(playerPosition.x + Random.Range(60, 150), playerPosition.y + Random.Range(20, 50), playerPosition.z);
        Instantiate(Monster, newPosition, Quaternion.identity);
    }

    public void ShowAddItemUI()
    {
        if (AddItemUI.activeSelf == false)
        {
            AddItemUI.SetActive(true);
            //AddItemUI.transform.position = new Vector2(1000, 200);
            GetComponent<RectTransform>().SetAsLastSibling();
        }
        else
            AddItemUI.SetActive(false);
    }

}
