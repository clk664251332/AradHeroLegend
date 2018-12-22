using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class DialogSentence
{
    public string name;
    public Sprite portrait;
    [TextArea(3, 10)]
    public string sentence;
}

public class DialogManager : MonoSingleton<DialogManager> {
    //临时储存要显示的所有句子
    private Queue<DialogSentence> Sentences;
    public GameObject DialogBox;
    //获取UI控件;
    public Image Image;
    public Text Name;
    public Text Content;
    public float showSpeed;
    public Message dialogEnd;

    bool sentenceEnd;
    DialogSentence sentence;

    private void Awake()
    {
        Sentences = new Queue<DialogSentence>();
        dialogEnd = new Message();
    }

    private void Update()
    {
    }

    public void StartDialog(DialogSentence[] dialogs)
    {
        Sentences.Clear();
        foreach (var temp in dialogs)
        {
            Sentences.Enqueue(temp);//入队
        }
        DialogBox.SetActive(true);
        DisplayNextSentence();
    }
    /// <summary>
    /// 显示句子
    /// </summary>
    public void DisplayNextSentence()
    {
        if (Sentences.Count == 0)
        {
            EndDialog();
            return;
        }
        StopAllCoroutines();

        sentenceEnd = false;
        sentence = Sentences.Dequeue();//出队列
        Image.sprite = sentence.portrait;
        Name.text = sentence.name;
        //Debug.Log("显示下一句");
        StartCoroutine(TypeSentence(sentence.sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        Content.text = "";
        foreach(char letter in sentence.ToCharArray())
        {
            Content.text += letter;
            yield return new WaitForSeconds(showSpeed);
           // yield return null;
        }
        //Debug.Log("显示完一句");
        sentenceEnd = true;
    }

    void EndDialog()
    {
        //Debug.Log("对话结束");
        sentence = null;
        DialogBox.SetActive(false);
        dialogEnd.Send();
    }

    public void OnNextButton()
    {
        if (!sentenceEnd)
        {
            StopAllCoroutines();
            sentenceEnd = true;
            Content.text = sentence.sentence;
            return;
        }
        else
            DisplayNextSentence();
    }
}
