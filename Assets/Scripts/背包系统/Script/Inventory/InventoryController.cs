using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class InventoryController: MonoSingleton<InventoryController> {

    public ItemDataBase Database { get { return m_ItemDatabase; } }
    [SerializeField]
    private ItemDataBase m_ItemDatabase;

    public InventoryContainer InventoryContainer;
    public EquipmentContainer EquipmentContainer;

    private Slot[] m_slots;

    public GameObject InventoryUI;

    private void Awake()
    {
        m_slots = GetComponentsInChildren<Slot>();
    }
    private void Start()
    {        
        InventoryUI.SetActive(false);
    }

    public Slot[] GetAllSlots()
    {
        return m_slots;
    }

    public ItemData GetItemById(int itemID)
    {
        ItemData itemData;
        if (m_ItemDatabase.FindItemById(itemID, out itemData))
            return itemData;
        else
        {
            Debug.LogWarning("未找到该物品");
            return null;
        }
    }

    //显示背包界面
    public void ShowInventoryUI()
    {
        if (InventoryUI.activeSelf == false)
        {
            InventoryUI.SetActive(true);
           // InventoryUI.transform.position = new Vector2(1827, 172);
            GetComponent<RectTransform>().SetAsLastSibling();
        }
        else
            InventoryUI.SetActive(false);
    }
}
