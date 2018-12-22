using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapSwitchManager : MonoSingleton<MapSwitchManager> {

    public Transform MapSwitchUI;
    public Image BackGround;
    public Image Text;

    public void StartMapSwitch(Sprite back,Sprite text)
    {
        MapSwitchUI.gameObject.SetActive(true);
        Text.gameObject.SetActive(true);
        Text.color = Color.white;

        BackGround.sprite = back;
        Text.sprite = text;
        StartCoroutine("FadingEffect");
    }

    IEnumerator FadingEffect()
    {
        yield return new WaitForSeconds(1.5f);
        //屏幕黑色渐变
        Fading.Instance.BeginFadePingPong(Color.black, 0.6f);
        yield return new WaitForSeconds(0.8f);
        //隐藏过图UI
        MapSwitchUI.gameObject.SetActive(false);
        Text.color = new Color(1, 1, 1, 0);
        //背景文字的显示与隐藏渐变
        float a = 0;
        while (a < 1)
        {
            a += Time.deltaTime;
            Text.color = new Color(1, 1, 1, a);
            yield return null;
        }
        yield return new WaitForSeconds(1.5f);
        while (a > 0)
        {
            a -= Time.deltaTime;
            Text.color = new Color(1, 1, 1, a);
            yield return null;
        }
        Text.gameObject.SetActive(false);
    }
}
