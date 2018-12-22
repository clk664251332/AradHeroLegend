using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDieFlash : MonoBehaviour {

	
    public void DestoryFlash()
    {
        Destroy(gameObject);
    }

    public void SetTimeScale1()
    {
        Time.timeScale = 0.05f;
    }

    public void SetTimeScale2()
    {
        Time.timeScale = 0.3f;
    }

    public void ResetTimeScale()
    {
        Time.timeScale = 1;
    }
}
