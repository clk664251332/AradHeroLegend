using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//控制商店界面的类，调用时传递物品ID的数组，然后遍历添加每个物品ID到容器里面
public class ShopController : MonoSingleton<ShopController>
{
    public ItemDataBase Database { get { return m_ItemDatabase; } }
    [SerializeField]
    private ItemDataBase m_ItemDatabase;

    public GameObject SelectAmount;
    public GameObject ShopUI;
    public Button previousButton;
    public Button nextButton;
    //public Button buyButton;
    public Text pageText;

    [SerializeField]
    private ShopItemContainer m_ShopCollection;

    ShopSlot[] shopSlots;
    /// <summary>
    /// 当前页面索引
    /// </summary>
    private int m_PageIndex = 1;

    /// <summary>
    /// 总页数
    /// </summary>
    private int m_PageCount = 0;

    /// <summary>
    /// 元素总个数
    /// </summary>
    private int m_ItemsCount = 0;
    /// <summary>
    /// 元素列表
    /// </summary>
    private List<int> m_ItemsList;
    /// <summary>
    /// 是否正在购买
    /// </summary>
    public bool isBuying;
    /// <summary>
    /// 当前点击的格子
    /// </summary>
    public ShopSlot CurSlot;

    private void Awake()
    {
        m_ShopCollection.Init();
        m_ItemsList = new List<int>();
    }

    private void Start()
    {
        shopSlots = GetComponentsInChildren<ShopSlot>();

        for (int i = 0; i < shopSlots.Length; i++)
        {
            //shopSlots[i].PointerEnter += OnEnterSlot;
            //shopSlots[i].PointerExit += OnExitSlot;
            //shopSlots[i].PointerDown += OnDowntSlot;
            //shopSlots[i].PointerUp += OnUpSlot;
            shopSlots[i].PointerClick += OnClickSlot;
        }
        ShopUI.SetActive(false);
    }
    //显示商店界面，要首先进行初始化
    public void ShowShopUI(int[] sellId)
    {
        if (ShopUI.activeSelf == true)
            return;
        if (sellId == null || sellId.Length == 0)
            return;
        //计算元素总个数
        m_ItemsCount = sellId.Length;
        //计算总页数
        m_PageCount = (m_ItemsCount % 10) == 0 ? m_ItemsCount / 10 : (m_ItemsCount / 10) + 1;
        
        //添加元素
        for (int i = 0; i < sellId.Length; i++)
        {
            m_ItemsList.Add(sellId[i]);
        }
        //显示
        ShopUI.SetActive(true);
        //ShopUI.transform.position = new Vector2(-200, 70);

        previousButton.onClick.AddListener(OnPrevious);
        nextButton.onClick.AddListener(OnNext);

        UpdateUI();
    }

    //显示页面
    void ShowPage(int index)
    {
        if (index < 0 || index > m_ItemsCount)
            return;
        if (m_ItemsCount<=0)
            return;

            for (int i = 0; i < m_ItemsList.Count-(index-1)*10; i++)
            {
                ItemData itemData;
                if (m_ItemDatabase.FindItemById(m_ItemsList[i+(index-1)*10], out itemData))
                {
                    m_ShopCollection.TryAddItem(itemData);
                    //Debug.Log(itemData.Name);
                }
            }
    }

    void UpdateUI()
    {
        ClearPage();
        ShowPage(m_PageIndex);
        pageText.text= string.Format("{0}/{1}", m_PageIndex.ToString(), m_PageCount.ToString());

        if (m_PageIndex == 1)
            previousButton.interactable = false;
        else
            previousButton.interactable = true;
        if (m_PageIndex==m_PageCount)
           nextButton.interactable = false;
        else
            nextButton.interactable = true;
    }

    public void OnPrevious()
    {
        if (m_PageCount <= 0)
            return;
        //第一页时禁止向前翻页
        if (m_PageIndex <= 1)
            return;
        m_PageIndex -= 1;
        if (m_PageIndex < 1)
            m_PageIndex = 1;
        UpdateUI();
    }

    public void OnNext()
    {
        if (m_PageCount <= 0)
            return;
        //最后一页禁止向后翻页
        if (m_PageIndex >= m_PageCount)
            return;

        m_PageIndex += 1;
        if (m_PageIndex >= m_PageCount)
            m_PageIndex = m_PageCount;
        UpdateUI();
    }

    void ClearPage()
    {
        m_ShopCollection.ClearAll();
    }

    public void CloseUI()
    {
        //ClearPage();
        m_ItemsList.Clear();
        m_PageIndex = 1;
        ShopUI.SetActive(false);
    }

    public void BuyItem()
    {
        CursorManager.Instance.SetBuy();
        isBuying = true;
    }

    void OnClickSlot(PointerEventData eventData, ShopSlot slot)
    {
        if (isBuying)
        {
            CurSlot = slot;
            isBuying = false;
            CursorManager.Instance.SetNormal();
            //显示数量选择框
            SelectAmount.SetActive(true);
            SelectAmount.transform.position = Input.mousePosition;
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (isBuying)
            {
                isBuying = false;
                CursorManager.Instance.SetNormal();
            }
        }
    }
}
