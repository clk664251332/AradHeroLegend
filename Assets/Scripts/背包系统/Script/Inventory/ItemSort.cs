using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSort : MonoBehaviour {

    //private Slot[] slots;
    public Button sortButton;
    private List<SavableItem> items=new List<SavableItem>();

    void Start () {
        //slots = GetComponentsInChildren<Slot>();
        sortButton.onClick.AddListener(Sort);
	}
	
    private void Sort()
    {
        Slot[] temp= GetComponentsInChildren<Slot>();
        //Debug.Log(temp.Length);
        //把背包所有的物品聚集在列表内,并Clear背包所有东西
        foreach (Slot s in temp)
        {
            if (s.HasItem)
            {
                //Debug.Log("1");
                items.Add(s.CurrentItem);
                s.Clear();
            }
        }
        //排序
        items.Sort((x, y) => {
            int re1= x.ItemData.MainType.CompareTo(y.ItemData.MainType);
            if (re1 == 0)
            {
                int re2 = x.ItemData.Quality.CompareTo(y.ItemData.Quality);
                if(re2==0)
                    return x.ItemData.Id.CompareTo(y.ItemData.Id);
                return re2;
            }
            return re1;
            //return x.ItemData.MainType.CompareTo(y.ItemData.MainType);
        });
        //把所有物品再给放回背包
        foreach(SavableItem item in items)
        {
            int added;
            InventoryController.Instance.InventoryContainer.TryAddItem(item.ItemData, item.CurrentInStack, out added);
        }
        //清空列表
        items.Clear();
    }
}
