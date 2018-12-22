using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class ClampCamera
{
    public Clamp ClampX;
    public float SetY;
}

[Serializable]
public class Clamp
{
    public float min;
    public float max;
}


/// <summary>
/// 对地图进行初始化，设置地图标题，限制范围等，并且调用屏幕渐变功能
/// </summary>
public class MapInitial : MonoBehaviour {

    public Texture MidGround;
    public Texture BackGround;

    public string mapTitle;
    public ClampCamera clampCamera;
    //Fading fade;
   

    private void Awake()
    {
        //Debug.Log("清理之前地图的NPC集合");
        CurMapNpcManager.Instance.AllNpc.Clear();
    }
    private void OnEnable()
    {
        //if (BackGroundController.instance == null)
            //Debug.Log("empty");
        SceneManager.sceneLoaded += OnSceneLoaded;
        //m_MonsterTrans = new List<Transform>();
       // Debug.Log("切换场景OnEnable");
    }
    private void Start()
    {
        //QuestManager.Instance.UpdateNpcQuestSign();
        //Debug.Log("切换场景Start");
        if (MidGround == null || BackGround == null)
            return;
        BackGroundController.instance.ChangeGround(MidGround, BackGround);
        
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (mapTitle == "" || mapTitle == " ")
        {
            mapTitle = "未设置";
        }
        //Fading.Instance.BeginFade(Color.black, 1, 0.8f);
        MapTitleUI.Instance.SetMapTitle(mapTitle);
        CameraFollow.Instance.SetClampInfo(clampCamera);
        //QuestManager.Instance.UpdateNpcQuestSign();

    }
}
