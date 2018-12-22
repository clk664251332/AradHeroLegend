using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class QuestDataBase : ScriptableObject
{
    public Quest[] AllQuest { get { return m_allQuest; } }

    [SerializeField]
    private Quest[] m_allQuest;

    public Quest FindQuestById(int id)
    {
        for (int i = 0; i < m_allQuest.Length; i++)
        {
            if (m_allQuest[i].id == id)
                return m_allQuest[i];
        }
        Debug.Log("未找到该任务");
        return null;
    }
    ///// <summary>
    ///// 输入：NPC的ID 输出：该任务的进行对话
    ///// </summary>
    ///// <param name="npcId"></param>
    ///// <returns></returns>
    //public bool FindDialogByNpcId(int npcId,out Dialog[] dialog)
    //{
    //    for(int i = 0; i < m_allQuest.Length; i++)
    //    {
    //        if (m_allQuest[i].state == QuestState.已接)
    //        {
    //            if (m_allQuest[i].EndNpcId == npcId)
    //            {
    //                dialog = m_allQuest[i].end_npc_dialog;
    //                return true;
    //            }
    //            else if (m_allQuest[i].StartNpcId == npcId)
    //            {
    //                dialog = m_allQuest[i].ing_npc_dialog;
    //                return true;
    //            }
    //        }
    //        else if (m_allQuest[i].state == QuestState.可接)
    //        {
    //            if (m_allQuest[i].StartNpcId == npcId)
    //            {
    //                dialog = m_allQuest[i].start_npc_dialog;
    //                return true;
    //            }
    //        }
    //    }
    //    dialog = null;
    //    return false;
    //}
}
