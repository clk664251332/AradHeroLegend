using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 管理副本中门的开启与关闭
/// 如果地图的怪物都死了，开启大门
/// </summary>
public class LevelDoor : MonoBehaviour {

    public SpriteRenderer fence;
    public SpriteRenderer flash;
    public GameObject colider;

    private void Start()
    {
        fence.enabled = true;
        flash.enabled = false;
        colider.SetActive(false);
    }

    public void OpenDoor()
    {
        fence.enabled = false;
        StartCoroutine("FlashDoor");
        colider.SetActive(true);
    }

    IEnumerator FlashDoor()
    {
        flash.enabled = true;
        float a = 0;
        int direction = 1;
        while (true)
        {
            a += Time.deltaTime * direction*1.5f;
            if (a >= 1)
            {
                a = 1;
                direction = -1;
            }
            if (a <= 0)
            {
                a = 0;
                direction = 1;
            }
            flash.color = new Color(1, 1, 1, a);
            yield return null;
        }
    }
}
