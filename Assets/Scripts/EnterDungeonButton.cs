using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EnterDungeonButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public int sceneIndex;
    public int dungeonId;
    public Vector2 toPosition;
    public GameObject SceneCut;
    public Image BlueLight;
    Button button;

    private void Start()
    {
        button = GetComponentInChildren<Button>();
        BlueLight.enabled = false;
    }
    //进入相应副本
    public void Enter_Dungeon()
    {
        //记录该副本的ID
        DungeonManager.Instance.CurDungeonId = dungeonId;
        //记录进入副本之前场景的序号
        DungeonManager.Instance.beforeDungeonSceneIndex = SceneManager.GetActiveScene().buildIndex;
        //记录进入副本之前的坐标
        DungeonManager.Instance.beforeDungeonPosition = PlayerContorller.Instance.transform.position;
        //记录进入副本之前的朝向
        DungeonManager.Instance.beforeDungeonplayerFacingRight = PlayerContorller.Instance.IsFacingRight();
        StartCoroutine(EnterDungeon());
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(button.interactable)
            BlueLight.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        BlueLight.enabled = false;
    }

    IEnumerator EnterDungeon()
    {
        //渐变显示加载背景，并持续两秒
        Fading.Instance.BeginFade(Color.black, 1, 0.8f);
        SceneCut.SetActive(true);
        yield return new WaitForSeconds(2);
        Fading.Instance.BeginFade(Color.black, -1, 0.8f);
        //渐变显示场景
        SceneManager.LoadScene(sceneIndex);
        SceneCut.SetActive(false);
        PlayerContorller.Instance.transform.position = toPosition;
        PlayerContorller.Instance.SetFacingLeft();

       DungeonManager.Instance.HideLorienSelect();
        Fading.Instance.BeginFade(Color.black, 1, 0.8f);
    }
}
