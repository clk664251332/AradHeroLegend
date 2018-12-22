using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashArea : MonoBehaviour {

    RectTransform rectTrans;
    Image image;
    int id;

    public void StartFlash(Vector2 position,Vector2 size,int _id)
    {
        rectTrans = GetComponent<RectTransform>();
        image = rectTrans.GetComponent<Image>();
        id = _id;
        HpBarController.Instance.DontDestoryFlashById += CheckToDestory;

        rectTrans.sizeDelta = size;
        rectTrans.localPosition = position;
        StartCoroutine("Flash");
    }

    IEnumerator Flash()
    {
        float a = 0.72f;
        while (a > 0)
        {
            a -= Time.deltaTime * 1.25f;
            //a -= Time.deltaTime * 0.25f;
            image.color = new Color(1, 1, 1, a);
            //yield return new WaitForEndOfFrame();
            yield return null;
        }
        HpShadowBar.Instance.nextRatio = HpBarController.Instance.ratio;
        //HpBarController.Instance.queue.Dequeue();
        HpBarController.Instance.DontDestoryFlashById -= CheckToDestory;
        Destroy(gameObject);
        yield return null;
    }

    public void CheckToDestory(int canShowId)
    {
        if (id != canShowId)
        {
            HpBarController.Instance.DontDestoryFlashById -= CheckToDestory;
            Destroy(gameObject);
        }
    }
}
