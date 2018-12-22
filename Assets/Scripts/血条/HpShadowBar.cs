using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpShadowBar : MonoSingleton<HpShadowBar> {

    //public HpBarController hpbar;

    public Image TopShadowBar;
    public Image BottomShadowBar;
    public Text BarCount;

    public float ratio;
    [HideInInspector]
    public float nextRatio;
    public Value<int> level;
    public float speed;

    Color red = new Color32(215, 0, 0, 255);
    Color darkRed = new Color32(172, 12, 0, 255);

    Color yellow = new Color32(248, 128, 0, 255);
    Color darkYellow = new Color32(187, 97, 0, 255);

    Color green = new Color32(144, 200, 0, 255);
    Color darkGreen = new Color32(121, 157, 33, 255);

    Color blue = new Color32(0, 120, 200, 255);
    Color darkBlue = new Color32(34, 108, 166, 255);

    Color purple = new Color32(136, 128, 248, 255);
    Color darkPurple = new Color32(125, 125, 200, 255);

    private void Awake()
    {
        level = new Value<int>(0);
        level.AddChangeListener(UpdateColor);
    }
    private void Start()
    {
        TopShadowBar.rectTransform.sizeDelta = HpBarController.Instance.TopBar.rectTransform.sizeDelta;
        BottomShadowBar.rectTransform.sizeDelta = HpBarController.Instance.BottomBar.rectTransform.sizeDelta;

        nextRatio = HpBarController.Instance.ratio;
        ratio = nextRatio;
        UpdateText();
    }

    public void ForceUpdate()
    {
        nextRatio = HpBarController.Instance.ratio;
        ratio = nextRatio;
    }

    private void Update()
    {
        //下一个比率
        //nextRatio = HpBarController.Instance.ratio;

        if (ratio > nextRatio)
        {
            ratio -= Time.deltaTime * speed;
        }else
        {
            ratio = nextRatio;
        }

        TopShadowBar.fillAmount = ratio - (int)ratio;
        //改变颜色
        level.Set((int)ratio % 5);

        if (ratio <= 0)
            HpBarController.Instance.HpBarUI.gameObject.gameObject.SetActive(false);
            //if (level == HpBarController.Instance.level)
            //{
            //    HpBarController.Instance.TopBar.enabled = true;
            //    HpBarController.Instance.BottomBar.enabled = true;
            //    BottomShadowBar.enabled = false;
            //}
            //else
            //{
            //    HpBarController.Instance.TopBar.enabled = false;
            //    HpBarController.Instance.BottomBar.enabled = false;
            //    BottomShadowBar.enabled = true;
            //}

    }

    //只有在阴影走到头时更新颜色
    public void UpdateColor()
    {
        //更改文本
        UpdateText();

        //更改颜色
        int CurHpLevel= (int)(HpBarController.Instance.ratio % 5);
        switch (CurHpLevel)
        {
            case 0://红
                if ((int)HpBarController.Instance.ratio == 0)
                {
                    HpBarController.Instance.TopBar.color = red;
                    HpBarController.Instance.BottomBar.color = new Color(0, 0, 0, 0);

                    TopShadowBar.color = darkRed;
                    BottomShadowBar.color = new Color(0, 0, 0, 0);
                }
                else
                {
                    HpBarController.Instance.TopBar.color = red;
                    HpBarController.Instance.BottomBar.color = purple;

                    TopShadowBar.color = darkRed;
                    BottomShadowBar.color = darkPurple;
                }
                break;
            case 1://黄
                HpBarController.Instance.TopBar.color = yellow;
                HpBarController.Instance.BottomBar.color = red;

                TopShadowBar.color = darkYellow;
                BottomShadowBar.color = darkRed;
                break;
            case 2://绿
                HpBarController.Instance.TopBar.color = green;
                HpBarController.Instance.BottomBar.color = yellow;

                TopShadowBar.color = darkGreen;
                BottomShadowBar.color = darkYellow;
                break;
            case 3://蓝
                HpBarController.Instance.TopBar.color = blue;
                HpBarController.Instance.BottomBar.color = green;

                TopShadowBar.color = darkBlue;
                BottomShadowBar.color = darkGreen;
                break;
            case 4://紫
                HpBarController.Instance.TopBar.color = purple;
                HpBarController.Instance.BottomBar.color = blue;

                TopShadowBar.color = darkPurple;
                BottomShadowBar.color = darkBlue;
                break;
        }

        //更改长度
        if (Mathf.Abs(level.Get() - HpBarController.Instance.level) <= 1)
        {
            HpBarController.Instance.TopBar.fillAmount = GetDotAmount(HpBarController.Instance.ratio);
            HpBarController.Instance.BottomBar.fillAmount = 1;
        }
        else
        {
            HpBarController.Instance.TopBar.fillAmount = 0;
            HpBarController.Instance.BottomBar.fillAmount = 0;
        }
    }

    void UpdateText()
    {
        //控制文字的显示
        if ((int)ratio == 0)
        {
            BarCount.enabled = false;
        }
        else
        {
            if (GetDotAmount(HpBarController.Instance.ratio) == 0)
                BarCount.text = "X" + ((int)ratio).ToString();
            else
                BarCount.text = "X" + ((int)ratio + 1).ToString();
            BarCount.enabled = true;
        }
    }
    /// <summary>
    /// 取浮点数的小数部分
    /// </summary>
    /// <param name="number"></param>
    /// <returns></returns>
    float GetDotAmount(float number)
    {
        return number - (int)number;
    }
}
