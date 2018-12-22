using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurMapNpcManager : MonoSingleton<CurMapNpcManager> {

    public List<NPC> AllNpc;

    private void Awake()
    {
        AllNpc = new List<NPC>();
    }

    public bool GetNpcByID(int id,out NPC _npc)
    {
        // _npc=null;
        if (AllNpc.Count != 0)
        {
            foreach (var npc in AllNpc)
            {
                if (npc.id == id)
                {
                    //Debug.Log("找到了ID为" + id + "的NPC");
                    _npc = npc;
                    return true;
                }
            }
        }
        _npc = null;
        return false;
    }
}
