using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour {

    public int sceneIndex;
    public Vector2 toPosition;

    public bool SceneSwitchEffect;
    public Sprite SceneCutBackGround;
    public Sprite SceneCutText;
    public int NeedLevel;

    Transform player;

	void Start () {
		player= GameObject.FindGameObjectWithTag("Player").transform;
    }
	
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(PlayerStateInfo.Instance.Level.Get()<NeedLevel)
            {
                Debug.Log("等级不够");
                return;
            }
            if (SceneSwitchEffect)
                MapSwitchManager.Instance.StartMapSwitch(SceneCutBackGround, SceneCutText);
            else
                Fading.Instance.BeginFade(Color.black, 1, 0.8f);
            SceneManager.LoadScene(sceneIndex);
            player.position = toPosition;
        }    
    }
}
