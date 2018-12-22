using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDontDestroy : MonoBehaviour {

    private static UIDontDestroy m_Instance;
    public static UIDontDestroy Instance
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
