using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

///控制各个大副本的显示与隐藏
///保存当前清理了哪些关卡
public class DungeonManager : MonoSingleton<DungeonManager> {

    public Image SelectText;
    public Transform Lorien;
    public List<int> m_HaveClearLevel;
    [HideInInspector]
    public Vector2 beforeDungeonPosition;
    [HideInInspector]
    public int beforeDungeonSceneIndex;
    [HideInInspector]
    public bool beforeDungeonplayerFacingRight;
    [HideInInspector]
    public int CurDungeonId;

    private void Start()
    {
        m_HaveClearLevel = new List<int>();
    }
    //显示格兰之森的副本选择
    public void ShowLorienSelect()
    {
        //Fading.Instance.BeginFade(Color.black, 1, 0.8f);
        Lorien.gameObject.SetActive(true);
        SelectText.gameObject.SetActive(true);
        SelectText.color = new Color(1, 1, 1, 0);
        Lorien.Find("小副本选择").gameObject.SetActive(false);
        PlayerContorller.Instance.canMove = false;
        StartCoroutine("FadingSelectText");
    }

    public void HideLorienSelect()
    {
        Lorien.gameObject.SetActive(false);
        PlayerContorller.Instance.canMove = true;
    }

    IEnumerator FadingSelectText()
    {
        float a = 0;
        while (a < 1)
        {
            a += Time.deltaTime;
            SelectText.color = new Color(1, 1, 1, a);
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        Lorien.Find("小副本选择").gameObject.SetActive(true);
        SelectText.gameObject.SetActive(false);
        yield return null;
    }

    public void AddClearedLevel(int levelId)
    {
        m_HaveClearLevel.Add(levelId);
    }

    public void ResetClearedLevel()
    {
        m_HaveClearLevel.Clear();
    }

    public List<int> GetHaveClearLevel()
    {
        return m_HaveClearLevel;
    }

    public IEnumerator MoveToTown()
    {
        Debug.Log("10秒后自动返回城镇");
        //检验是否有副本任务完成
        QuestManager.Instance.DungeonQuestCheck(CurDungeonId);
        CurDungeonId = 0;
        yield return new WaitForSeconds(10);
        Fading.Instance.BeginFade(Color.black, 1, 0.8f);
        ResetClearedLevel();
        var pos = beforeDungeonPosition;
        PlayerContorller.Instance.transform.position = new Vector2(pos.x - 100, pos.y);
        if (beforeDungeonplayerFacingRight)
            PlayerContorller.Instance.SetFacingLeft();
        else
            PlayerContorller.Instance.SetFacingRight();

        SceneManager.LoadScene(beforeDungeonSceneIndex);
    }
}
