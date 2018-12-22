using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoSingleton<QuestManager>
{
    ///包含接取任务、放弃任务、完成任务的函数
    ///每当角色在与NPC进行对话时，便会查询NPC的任务中是否有特殊状态的任务，有的话便会触发相应的对话
    ///或者是触发特定的触发器，查询触发器是否有任务，有的话也进行触发
    public NpcDataBase npcDataBase;
    public QuestDataBase dataBase;
    private Quest[] m_allQuest;
    public Quest willShowDialogQuest;
    public Quest currentQuest;

    private void Awake()
    {
        m_allQuest = dataBase.AllQuest;
    }
    private void Start()
    {
        currentQuest = m_allQuest[0];
        QuestUIManager.Instance.UpdateUI(currentQuest);
    }
    /// <summary>
    /// 根据NPC与任务 来显示对话
    /// 输入：NPC的ID 输出：该任务的进行对话
    /// </summary>
    /// <param name="npcId"></param>
    /// <returns></returns>
    public bool FindDialogByNpcId(int npcId, out DialogSentence[] dialog)
    {
        for (int i = 0; i < m_allQuest.Length; i++)
        {
            if (m_allQuest[i].state == QuestState.已完成)
            {
                if (m_allQuest[i].EndNpcId == npcId)
                {
                    dialog = m_allQuest[i].end_npc_dialog;
                    willShowDialogQuest = m_allQuest[i];
                    //QuestUIManager.Instance.UpdateUI(currentQuest);
                    return true;
                }
            }
            else if (m_allQuest[i].state == QuestState.已接)
            {
                if (m_allQuest[i].StartNpcId == npcId)
                {
                    dialog = m_allQuest[i].ing_npc_dialog;
                    willShowDialogQuest = m_allQuest[i];
                    //QuestUIManager.Instance.UpdateUI(currentQuest);
                    return true;
                }
            }
            else if (m_allQuest[i].state == QuestState.可接)
            {
                if (m_allQuest[i].StartNpcId == npcId)
                {
                    dialog = m_allQuest[i].start_npc_dialog;
                    willShowDialogQuest = m_allQuest[i];
                    //QuestUIManager.Instance.UpdateUI(currentQuest);
                    return true;
                }
            }
        }
        dialog = null;
        return false;
    }
    //对话任务完成检测,只在与NPC对话的时候检测
    public void DialogQuestCheck(int npcId)
    {
        for (int i = 0; i < m_allQuest.Length; i++)
        {
            if (m_allQuest[i].Type == QuestType.对话)
            {
                if (m_allQuest[i].NpcId == npcId)
                {
                    CompleteQuest(m_allQuest[i].id);
                }
            }
        }
    }
    //收集物品任务检测，设置监听器，每当物品增加删除时更新
    //副本通关检测
    public void DungeonQuestCheck(int dungeonId)
    {
        for (int i = 0; i < m_allQuest.Length; i++)
        {
            if (m_allQuest[i].Type == QuestType.通关)
            {
                if (m_allQuest[i].LevelId == dungeonId)
                {
                    CompleteQuest(m_allQuest[i].id);
                    Debug.Log("完成副本任务");
                }
            }
        }
    }


    public void AcceptQuest(int id)
    {
        var quest = dataBase.FindQuestById(id);
        if (quest != null)
        {
            if (quest.state == QuestState.可接)
            {
                quest.state = QuestState.已接;
                Debug.Log("接受任务：" + quest.title);
                Fading.Instance.BeginFade(Color.white, 1, 1);
                NPC resultNpc;
                NpcInfo npcInfo;
                if (CurMapNpcManager.Instance.GetNpcByID(quest.StartNpcId, out resultNpc))
                {
                    resultNpc.ShowNothingSign();
                }
                if (npcDataBase.FindNpcById(quest.StartNpcId, out npcInfo))
                {
                    npcInfo.questStateType = NPCQuestStateType.无;
                }

                if (quest.Type == QuestType.对话)
                {
                    if (CurMapNpcManager.Instance.GetNpcByID(quest.NpcId, out resultNpc))
                    {
                        //该NPC在另一个地图上怎么修改？
                        resultNpc.ShowCompleteQuestSign();
                    }
                    if (npcDataBase.FindNpcById(quest.NpcId, out npcInfo))
                    {
                        npcInfo.questStateType = NPCQuestStateType.感叹号;
                    }
                }
                    
            }
        }
        currentQuest = quest;
        QuestUIManager.Instance.UpdateUI(currentQuest);
    }

    public void CompleteQuest(int id)
    {
        var quest = dataBase.FindQuestById(id);
        if (quest != null)
        {
            if (quest.state == QuestState.已接)
            {
                quest.state = QuestState.已完成;
                Debug.Log("完成任务：" + quest.title);
                Fading.Instance.BeginFade(Color.yellow, 1, 1);
                NPC resultNpc;
                NpcInfo npcInfo;
                if (CurMapNpcManager.Instance.GetNpcByID(quest.EndNpcId, out resultNpc))
                {
                    resultNpc.ShowCompleteQuestSign();
                }
                if (npcDataBase.FindNpcById(quest.EndNpcId, out npcInfo))
                {
                    npcInfo.questStateType = NPCQuestStateType.感叹号;
                }
            }
        }
    }
    //领取奖励
    public void FinishQuest(int id)
    {
        var quest = dataBase.FindQuestById(id);
        if (quest != null)
        {
            if (quest.state == QuestState.已完成)
            {
                quest.state = QuestState.终结;
                Debug.Log("获得奖励！");
                //改变任务提示
                NPC _resultNpc;
                NpcInfo npcInfo;
                if (CurMapNpcManager.Instance.GetNpcByID(quest.EndNpcId, out _resultNpc))
                {
                    _resultNpc.ShowNothingSign();
                }
                if (npcDataBase.FindNpcById(quest.EndNpcId, out npcInfo))
                {
                    npcInfo.questStateType = NPCQuestStateType.无;
                }
                //获得奖励
                PlayerStateInfo.Instance.CurrentExp.Set(PlayerStateInfo.Instance.CurrentExp.Get() + quest.rewardExp);
                PlayerStateInfo.Instance.Gold.Set(PlayerStateInfo.Instance.Gold.Get() + quest.rewardGold);
                foreach(var rewardItem in quest.rewardItems)
                {
                    int added;
                    ItemData item = InventoryController.Instance.GetItemById(rewardItem.itemId);
                    InventoryController.Instance.InventoryContainer.TryAddItem(item, rewardItem.amount, out added);
                }

                var nextQuest = dataBase.FindQuestById(quest.nextQuestId);
                if (nextQuest != null)
                {
                    if (nextQuest.state == QuestState.不可接)
                    {
                        nextQuest.state = QuestState.可接;
                        NPC resultNpc;
                        if (CurMapNpcManager.Instance.GetNpcByID(nextQuest.StartNpcId, out resultNpc))
                        {
                            resultNpc.ShowGetMainQuestSign();
                        }
                        if (npcDataBase.FindNpcById(nextQuest.StartNpcId, out npcInfo))
                        {
                            npcInfo.questStateType = NPCQuestStateType.金色书本;
                        }
                    }
                }
                else
                    Debug.LogWarning("下一个任务为空,请设置任务！");
                
            }
        }
        currentQuest = quest;
        QuestUIManager.Instance.UpdateUI(currentQuest);
    }

    public void GiveUplQuest(int id)
    {
        var quest = dataBase.FindQuestById(id);
        if (quest != null)
        {
            if (quest.state == QuestState.已接)
                quest.state = QuestState.可接;
        }
    }
    /// <summary>
    /// 更新NPC头像的任务标记
    /// </summary>
    public void UpdateNpcQuestSign()
    {
        Debug.Log("更新一次");
        for (int i = 0; i < m_allQuest.Length; i++)
        {
            //if (m_allQuest[i].state == QuestState.终结 || m_allQuest[i].state == QuestState.不可接)
            //{
            //    NPC resultNpc;
            //    if (AllNpcManager.Instance.GetNpcByID(m_allQuest[i].StartNpcId, out resultNpc))
            //    {
            //        resultNpc.ShowNothingSign();
                    
            //    }
            //}
            if(m_allQuest[i].state == QuestState.已接)
            {
                if (m_allQuest[i].Type == QuestType.对话)
                {
                    //Debug.Log("有角色需要显示感叹号");
                    Debug.Log(m_allQuest[i].NpcId + "号NPC需要显示感叹号");
                    NPC resultNpc;
                    if (CurMapNpcManager.Instance.GetNpcByID(m_allQuest[i].NpcId, out resultNpc))
                    {
                        Debug.Log("已经找到了");
                        resultNpc.ShowCompleteQuestSign();
                    }
                }
            }
            else if (m_allQuest[i].state == QuestState.已完成)
            {
                NPC resultNpc;
                if (CurMapNpcManager.Instance.GetNpcByID(m_allQuest[i].EndNpcId, out resultNpc))
                {
                    resultNpc.ShowCompleteQuestSign();
                }
            }
            else if (m_allQuest[i].state == QuestState.可接)
            {
                //根据NPC ID指定NPC显示相应的图标
                //AllNpcManager.Instance.GetNpcByID(m_allQuest[i].StartNpcId).ShowGetMainQuestSign();
                NPC resultNpc;
                if (CurMapNpcManager.Instance.GetNpcByID(m_allQuest[i].StartNpcId, out resultNpc))
                {
                    resultNpc.ShowGetMainQuestSign();
                }
            }
        }
    }
}
