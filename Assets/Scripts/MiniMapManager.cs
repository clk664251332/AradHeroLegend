using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapManager : MonoSingleton<MiniMapManager> {

    public Transform MiniMapUI;
    public DungeonMiniMapDataBase MinimapDatabase;
    private DungeonMimiMapSlotInfo[] currentMiniMapSlotInfo;
    private MiniMapSlot[] miniMapSlots;
    [HideInInspector]
    public int currentSlotIndex;

    private void Start()
    {
        miniMapSlots = GetComponentsInChildren<MiniMapSlot>();
        for(int i=0;i< miniMapSlots.Length; i++)
        {
            miniMapSlots[i].slotIndex = i;
        }
        MiniMapUI.gameObject.SetActive(false);
    }
    /// <summary>
    /// 初始化小地图
    /// </summary>
    /// <param name="dungeonName"></param>
    public void SetMiniMapInfo(string dungeonName)
    {
        //获取设置信息
        currentMiniMapSlotInfo = MinimapDatabase.GetMimiMapInfoByName(dungeonName).MiniMapSlots;
        //初始化图像
        for(int i=0;i< miniMapSlots.Length; i++)
        {
            miniMapSlots[i].ContentImage.sprite = currentMiniMapSlotInfo[i].MiniMapSprite;
            miniMapSlots[i].sceneIndex = currentMiniMapSlotInfo[i].SceneIndex;
            miniMapSlots[i].bossLevel = currentMiniMapSlotInfo[i].Boss;
        }
    }

    public void OnEnterMapUpdate()
    {
        for (int i = 0; i < miniMapSlots.Length; i++)
        {
            miniMapSlots[i].OnEnterMapUpdate();
        }
    }

    public void OnClearMonsterUpdate()
    {
        //对周围的格子进行更新
        List<int> SurroundingNumber=GetSurroundingNumber(currentSlotIndex);
        //Debug.Log("当前格子序号"+currentSlotIndex);

        foreach(var index in SurroundingNumber)
        {
            //Debug.Log("周围格子序号" + index);
            miniMapSlots[index].SetSurroundingSlot();
        }
    }

    public void ShowMiniMapUI()
    {
        MiniMapUI.gameObject.SetActive(true);
    }

    public void HideMiniMapUI()
    {
        MiniMapUI.gameObject.SetActive(false);
    }

    /// <summary>
    /// 获得该格子周围格子的序号
    /// </summary>
    /// <returns></returns>
    public List<int> GetSurroundingNumber(int slotIndex)
    {
        List<int> SurroundingNumber = new List<int>();

        int[,] array = new int[3, 5]
        {
            {0,1,2,3,4 },{5,6,7,8,9},{10,11,12,13,14}
        };

        int row = 0;
        int column = 0;

        for (int r = 0; r < 3; r++)
        {
            for (int c = 0; c < 5; c++)
            {
                if (array[r, c] == slotIndex)
                {
                    row = r;
                    column = c;
                    break;
                }
            }
        }

        //上
        if (row - 1 >= 0)
        {
            SurroundingNumber.Add(array[row - 1, column]);
        }
        //下
        if (row + 1 <= 2)
        {
            SurroundingNumber.Add(array[row + 1, column]);
        }
        //左
        if (column - 1 >= 0)
        {
            SurroundingNumber.Add(array[row, column - 1]);
        }
        //右
        if (column + 1 <= 4)
        {
            SurroundingNumber.Add(array[row, column + 1]);
        }

        return SurroundingNumber;
    }
}
