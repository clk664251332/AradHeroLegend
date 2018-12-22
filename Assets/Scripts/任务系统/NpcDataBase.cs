using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class NpcDataBase : ScriptableObject
{
    public NpcInfo[] AllNpc { get { return m_allNpc; } }

    [SerializeField]
    private NpcInfo[] m_allNpc;

    public bool FindNpcById(int id, out NpcInfo npcInfo)
    {
        for (int i = 0; i < m_allNpc.Length; i++)
        {
            if (m_allNpc[i].id == id)
            {
                npcInfo = (m_allNpc[i]);
                return true;
            }
        }
        npcInfo = null;
        return false;
    }

    /// <summary>
    /// 
    /// </summary>
    public bool FindNpcByName(string name, out NpcInfo npcInfo)
    {
        for (int i = 0; i < m_allNpc.Length; i++)
        {
            if (m_allNpc[i].name == name)
            {
                npcInfo = (m_allNpc[i]);
                return true;
            }
        }
        npcInfo = null;
        return false;
    }

}

