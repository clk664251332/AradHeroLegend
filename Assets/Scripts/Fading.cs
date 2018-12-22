using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Fading : MonoSingleton<Fading> {

    public Texture2D fadeTexture;
    public float fadeSpeed=0.8f;

    private int drawDepth = -1000;  //显示在最上面
    //透明度，只有变成0才能看到画面
    private float alpha = 1.0f;
    private int fadeDir = 1;   //in=1 out=-1
    private Color color;
    private bool pingpong;

    private void OnGUI()
    {
        if (!pingpong)
            alpha -= fadeDir * fadeSpeed * Time.deltaTime;
        else
        {
            alpha -= fadeDir * fadeSpeed * Time.deltaTime;
            if (alpha >= 1)
            {
                fadeDir = 1;
                fadeSpeed = 0.3f;
            }
        }
        alpha = Mathf.Clamp01(alpha);

        //GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
        GUI.color = new Color(color.r, color.g, color.b, alpha);
        GUI.depth = drawDepth;
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeTexture);
    }

    public void BeginFade(Color _color,int direction,float speed)
    {
        alpha = 1.0f;
        color = _color;
        fadeDir = direction;
        fadeSpeed = speed;
        pingpong = false;
    }

    /// <summary>
    /// 逐渐变黑然后逐渐显示
    /// </summary>
    public void BeginFadePingPong(Color _color, float speed)
    {
        color = _color;
        fadeSpeed = speed;
        fadeDir = -1;//朝着变黑的方向
        pingpong = true;
    }
}
