using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BottomUI : MonoBehaviour {

    public Image HpImage;
    public Image MpImage;
    public Image LvImage;
    public Text HpText;
    public Text MpText;
    public Text LvText;

    private void Start()
    {
        PlayerStateInfo.Instance.HP.AddChangeListener(onHPChange);
        PlayerStateInfo.Instance.MP.AddChangeListener(onMPChange);
        PlayerStateInfo.Instance.CurrentExp.AddChangeListener(onExpChange);
        PlayerStateInfo.Instance.Level.AddChangeListener(onLvChange);

        float HpFillAmount= (float)Math.Round((double)PlayerStateInfo.Instance.HP.Get() / PlayerStateInfo.Instance.MaxHp.Get(), 3);
        float MpFillAmount = (float)Math.Round((double)PlayerStateInfo.Instance.MP.Get() / PlayerStateInfo.Instance.MaxMp.Get(), 3);
        HpImage.fillAmount = HpFillAmount;
        MpImage.fillAmount = MpFillAmount;

        HpText.text = (HpFillAmount * 100).ToString()+"%";
        MpText.text= (MpFillAmount * 100).ToString()+"%";

        LvImage.fillAmount = (float)Math.Round((double)PlayerStateInfo.Instance.CurrentExp.Get() / PlayerStateInfo.Instance.NeedExp.Get(), 3);
    }

    private void onHPChange()
    {
        float value = (float)Math.Round((double)PlayerStateInfo.Instance.HP.Get() / PlayerStateInfo.Instance.MaxHp.Get(), 3);
        //Debug.Log(Math.Round((double)PlayerStateInfo.Instance.HP.Get() / PlayerStateInfo.Instance.MaxHp.Get(), 3));
        HpImage.fillAmount = value;
        HpText.text = (value*100).ToString()+"%";
    }
    private void onMPChange()
    {
        float value = (float)Math.Round((double)PlayerStateInfo.Instance.MP.Get() / PlayerStateInfo.Instance.MaxMp.Get(), 3);
        MpImage.fillAmount = value;
        MpText.text = (value * 100).ToString() + "%";
    }

    private void onExpChange()
    {
        LvImage.fillAmount = (float)Math.Round((double)PlayerStateInfo.Instance.CurrentExp.Get() / PlayerStateInfo.Instance.NeedExp.Get(), 3);
    }

    private void onLvChange()
    {
        LvText.text = PlayerStateInfo.Instance.Level.Get().ToString();
    }
}
