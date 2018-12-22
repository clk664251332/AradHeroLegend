using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance
    {
        get
        {
            if (m_Instance == null)
                m_Instance = FindObjectOfType<T>();

            return m_Instance;
        }
    }

    protected static T m_Instance;

}

