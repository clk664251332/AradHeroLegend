using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDontDestory : MonoBehaviour {

    private static PlayerDontDestory m_Instance;
    public static PlayerDontDestory Instance
    {
        get
        {
            return m_Instance;
        }
    }

    private void Awake()
    {
        if (m_Instance == null)
        {
            m_Instance = this;
            DontDestroyOnLoad(transform.gameObject);
        }
        else
            Destroy(gameObject);
    }
}
