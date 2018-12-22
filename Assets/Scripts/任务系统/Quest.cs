using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
///任务系统设计
///通过更改任务的状态来控制任务的接取，完成等功能

public enum QuestState { 不可接, 可接, 已接, 已完成, 终结 }
public enum QuestType {对话,打怪,收集物品,通关,到达目的地 }

[Serializable]
public class RewardItem
{
    public int itemId;
    public int amount;
}

[Serializable]
public class Quest
{

    public string title;
    public int id;
    [Multiline(2)]
    public string description;

    public QuestState state;
    public QuestType Type;

    public int NpcId;
    public int MonsterId;
    public int MonsterValue;
    public int ItemId;
    public int ItemValue;
    public int LevelId;
    public int TriggerId;

    public int StartNpcId;
    public int EndNpcId;
    ///当任务处于不可接或者终结的状态时,显示的是NPC的默认语句
    ///当任务处于其他状态时，显示的是NPC下面的语句，共3种情况
    public DialogSentence[] start_npc_dialog;//任务获取的对话
    public DialogSentence[] ing_npc_dialog;//任务进行中的对话
    public DialogSentence[] end_npc_dialog;//任务已完成的对话

    //当任务不与NPC发生互动时，角色自言自语的情况
    public DialogSentence[] tigger_dialog;

    public RewardItem[] rewardItems;
    public int rewardExp;
    public int rewardGold;

    public int nextQuestId; 
}
