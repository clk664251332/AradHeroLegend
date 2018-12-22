using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DungeonMimiMapInfo
{

    public string DungeonName;
    public DungeonMimiMapSlotInfo[] MiniMapSlots;
}

[Serializable]
public class DungeonMimiMapSlotInfo
{
    public int SceneIndex;
    public Sprite MiniMapSprite;
    public bool Boss;
}

[CreateAssetMenu]
public class DungeonMiniMapDataBase : ScriptableObject
{
    public DungeonMimiMapInfo[] mimiMapInfos;

    public DungeonMimiMapInfo GetMimiMapInfoByName(string name)
    {
        for(int i=0;i< mimiMapInfos.Length; i++)
        {
            if (mimiMapInfos[i].DungeonName == name)
            {
                return mimiMapInfos[i];
            }
        }
        Debug.LogWarning("未找到信息");
        return null;
    }
}


