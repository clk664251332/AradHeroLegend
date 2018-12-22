using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 普通地图通往副本的门
/// </summary>
public class DungeonDoor : MonoBehaviour {

    public string dungeonName;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (dungeonName == "洛兰")
                DungeonManager.Instance.ShowLorienSelect();
        }
    }
}
