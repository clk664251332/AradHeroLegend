using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonMapInitial : MonoBehaviour {

    private List<MonsterController> m_MonsterList;
    public LevelDoor[] levelDoors;
    public bool firstLevel;

    void OnEnable () {
        m_MonsterList = new List<MonsterController>();
        //如果是第一个关卡，则初始化小地图的信息并显示小地图
        if (firstLevel)
        {
            MiniMapManager.Instance.SetMiniMapInfo(GetComponent<MapInitial>().mapTitle);
            MiniMapManager.Instance.ShowMiniMapUI();
        }
        MiniMapManager.Instance.OnEnterMapUpdate();
    }

    private void Start()
    {
        //如果该房间已经被清理过，销毁所有怪物
        if (DungeonManager.Instance.GetHaveClearLevel().Contains(SceneManager.GetActiveScene().buildIndex))
        {
            //隐藏所有怪物
            foreach (var monster in m_MonsterList)
           {
                monster.gameObject.SetActive(false);
           }
            //打开所有门
            if (levelDoors.Length != 0)
            {
                for (int i = 0; i < levelDoors.Length; i++)
                    levelDoors[i].OpenDoor();
            }
        }
    }

    public void AddMonster(MonsterController monster)
    {
        m_MonsterList.Add(monster);
    }

    public void RemoveMonster(MonsterController monster)
    {
        m_MonsterList.Remove(monster);
        if (m_MonsterList.Count == 0)
        {
            if (levelDoors.Length != 0)
            {
                for (int i = 0; i < levelDoors.Length; i++)
                    levelDoors[i].OpenDoor();
            }
            //已经清理完的地图，添加进副本管理
            DungeonManager.Instance.AddClearedLevel(SceneManager.GetActiveScene().buildIndex);
            MiniMapManager.Instance.OnClearMonsterUpdate();
        }
    }
    /// <summary>
    /// 当Boss死亡后清除该关卡其余的怪物
    /// </summary>
    public void ClrearAllMonsterWhenBossDie()
    {
        foreach (var monster in m_MonsterList)
        {
            if (monster.monsterInfo.isBoss)
                continue;
            monster.gameObject.SetActive(false);
        }
    }
}
