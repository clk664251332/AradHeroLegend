using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private static CameraFollow instance = null;
    public static CameraFollow Instance
    {
        get
        {
            return instance;
        }
    }

    //相机最大X坐标                           
    private float maxX=1600f;
    //最小
    private float minX=0;
    //坐标限制
    private ClampCamera clamp;

    private Transform m_Player; 


    private void Awake()
    {
        Application.targetFrameRate = 60;
        m_Player = GameObject.FindGameObjectWithTag("Player").transform;
        //m_Player = PlayerContorller.Instance.transform;

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(transform.gameObject);
        }
        else
            Destroy(gameObject);
    }

    private void LateUpdate()
    {
        TrackPlayer();
    }

    public void SetClampInfo(ClampCamera info)
    {
        clamp = info;
    }

    private void TrackPlayer()
    {
        //获取玩家位置
        float X = m_Player.position.x;
        //根据玩家位置限制相机的坐标
        X = Mathf.Clamp(X, clamp.ClampX.min, clamp.ClampX.max);
        //更新相机坐标
        transform.position = new Vector3(X, clamp.SetY, transform.position.z);
    }

}

