using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MiniMapSlot : MonoBehaviour {

    public Image ContentImage;
    public Image QuestionMarkImage;
    public Image Player;
    public Image BossImage;
    [HideInInspector]
    public int slotIndex;
    [HideInInspector]
    public int sceneIndex;
    [HideInInspector]
    public bool bossLevel;
    //是否是角色所在格子
    //是否被清理过

    public void SetContentImage(Sprite sprite)
    {
        if (sprite == null)
        {
            ContentImage.sprite = null;
            ContentImage.enabled = false;
        }
        else
            ContentImage.sprite = sprite;
    }

    public void ShowContent()
    {
        if (ContentImage.sprite == null)
            ContentImage.enabled = false;
        else
            ContentImage.enabled = true;
        ContentImage.color = Color.white;

        QuestionMarkImage.enabled = false;
    }
    //已经探索过的，如果闪烁则是已经探索过但可以去的地方
    public void ShowContentDark()
    {
        if (ContentImage.sprite == null)
            ContentImage.enabled = false;
        else
            ContentImage.enabled = true;
        ContentImage.color = new Color32(255, 255, 255, 140);

        QuestionMarkImage.enabled = false;
    }
    //问号必须闪烁（表示为下一个可去的地点，且没有探索过）
    public void ShowQuestionMark()
    {
        if (ContentImage.sprite == null)
            return;
        ContentImage.enabled = false;
        QuestionMarkImage.enabled = true;
        //BossImage.enabled = false;
    }


    /// <summary>
    /// 更新小地图，在每次 切换关卡地图 时调用
    /// </summary>
    public void OnEnterMapUpdate()
    {
        StopAllCoroutines();
        //该格子的场景编号与当前场景编号相同
        if (sceneIndex == SceneManager.GetActiveScene().buildIndex)
        {
            Player.enabled = true;
            ShowContent();
            //储存该格子的格子编号
            MiniMapManager.Instance.currentSlotIndex = slotIndex;
        }
        //对其余所有格子进行操作
        else
        {
            //如果被探索过
            if (DungeonManager.Instance.GetHaveClearLevel().Contains(sceneIndex))
            {
                Player.enabled = false;
                //显示暗色
                ShowContentDark();
            }
            //未被探索过
            else
            {
                //显示黑色
                Player.enabled = false;
                ContentImage.enabled = false;
                QuestionMarkImage.enabled = false;
                BossImage.enabled = false;
            }
        }
        //如果进的是一个已经清理完的空关卡
        if (DungeonManager.Instance.GetHaveClearLevel().Contains(SceneManager.GetActiveScene().buildIndex))
        {
            MiniMapManager.Instance.OnClearMonsterUpdate();
        }

        //是否是Boss关卡
        if (bossLevel)
            BossImage.enabled = true;
        else
            BossImage.enabled = false;
    }

    /// <summary>
    /// 清理完怪物设置，周围的格子才能调用的函数
    /// </summary>
    public void SetSurroundingSlot()
    {
        if (SceneManager.GetActiveScene().buildIndex == sceneIndex)
            return;
        if(bossLevel)
            return;
        //如果被探索过了
        if (DungeonManager.Instance.GetHaveClearLevel().Contains(sceneIndex))
        {
            ShowContent();
            StartCoroutine("FlashContent");
        }
        else
        {
            ShowQuestionMark();
            StartCoroutine("FlashQuestionMark");
        }
    }

    IEnumerator FlashContent()
    {
        while (true)
        {
            ContentImage.color = Color.white;
            yield return new WaitForSeconds(0.5f);
            ContentImage.color = new Color32(255, 255, 255, 140);
            yield return new WaitForSeconds(0.5f);
            yield return null;
        }
    }

    IEnumerator FlashQuestionMark()
    {
        while (true)
        {
            QuestionMarkImage.color = Color.white;
            yield return new WaitForSeconds(0.5f);
            QuestionMarkImage.color = new Color32(255, 255, 255, 140);
            yield return new WaitForSeconds(0.5f);
            yield return null;
        }
    }
}
