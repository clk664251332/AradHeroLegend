using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestUIManager : MonoSingleton<QuestUIManager> {

    public GameObject QuestUI;
    public Text QuestTitle;
    public Text QuestDescription;
    public GameObject IngSign;
    public GameObject CompleteSign;
    public Text GoldText;
    public Text ExpText;

    RewardItemSlot[] rewardItemSlots;

    private void Start()
    {
        rewardItemSlots = GetComponentsInChildren<RewardItemSlot>();
        QuestUI.SetActive(false);
    }

    public void ShowQuestUI()
    {
        if (QuestUI.activeSelf == false)
        {
            QuestUI.SetActive(true);
            GetComponent<RectTransform>().SetAsLastSibling();
        }
        else
            QuestUI.SetActive(false);
    }

    public void UpdateUI(Quest quest)
    {
        QuestTitle.text = quest.title;
        QuestDescription.text = quest.description;
        if (quest.state == QuestState.已接)
        {
            IngSign.SetActive(true);
            CompleteSign.SetActive(false);
        }
        else if (quest.state == QuestState.已完成|| quest.state == QuestState.终结)
        {
            IngSign.SetActive(false);
            CompleteSign.SetActive(true);
        }
        //隐藏全部格子
        for (int i = 0; i < rewardItemSlots.Length; i++)
        {
            rewardItemSlots[i].ResetSlot();
        }
        //更新奖励物品格子显示
        var rewardItems = quest.rewardItems;
        for (int i = 0; i < rewardItems.Length; i++)
        {
            var itemData = InventoryController.Instance.GetItemById(rewardItems[i].itemId);
            rewardItemSlots[i].SetItemImage(itemData.Icon);
            rewardItemSlots[i].SetItemAmount(rewardItems[i].amount);
        }
        //更新金币经验显示
        GoldText.text = quest.rewardGold.ToString();
        ExpText.text = quest.rewardExp.ToString();
    }
}

