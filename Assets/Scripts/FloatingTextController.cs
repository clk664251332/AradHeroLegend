using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingTextController : MonoSingleton<FloatingTextController>
{
    public GameObject DamageTextPrefab;
    public GameObject PlayerBeHitTextPrefab;
    public GameObject CriticalTextPrefab;

    public Transform canvasTrans;

    public void CreatDamageText(string text, Vector2 position)
    {
        //创建物体
        GameObject instance = Instantiate(DamageTextPrefab);
        //设置位置
        instance.transform.SetParent(canvasTrans, false);
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(position);
        instance.transform.position = screenPosition;
        //设置文字
        instance.GetComponentInChildren<FloatingText>().SetText(text);
    }

    public void CreatBeHitDamageText(string text, Vector2 position)
    {
        //创建物体
        GameObject instance = Instantiate(PlayerBeHitTextPrefab);
        //设置位置
        instance.transform.SetParent(canvasTrans, false);
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(position);
        instance.transform.position = screenPosition;
        //设置文字
        instance.GetComponentInChildren<FloatingText>().SetText(text);
        instance.GetComponentInChildren<FloatingText>().SetColor(Color.red);
    }

    public void CreatCriticalDamageText(string text, Vector2 position)
    {
        //创建物体
        GameObject instance = Instantiate(CriticalTextPrefab);
        //设置位置
        instance.transform.SetParent(canvasTrans, false);
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(position);
        instance.transform.position = screenPosition;
        //设置文字
        instance.GetComponentInChildren<FloatingText>().SetText(text);
    }
}