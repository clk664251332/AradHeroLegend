using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingText : MonoBehaviour {

    Animator anim;
    Text damageText;

	void OnEnable () {
        anim = GetComponent<Animator>();
        damageText = GetComponent<Text>();
        AnimatorClipInfo[] clipInfo = anim.GetCurrentAnimatorClipInfo(0);
        Destroy(transform.parent.gameObject, clipInfo[0].clip.length);
	}

	public void SetText(string text)
    {
        damageText.text = text;
    }

    public void SetColor(Color color)
    {
        damageText.color = color;
    }
}
