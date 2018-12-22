using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public enum NPCQuestStateType
{
    无, 金色书本,感叹号
}
//管理NPC的类
[Serializable]
public class NpcInfo
{
    public string name;
    public int id;
    public Sprite portrait;
    public DialogSentence[] saying;
    public NPCQuestStateType questStateType;

    public NpcInfo(int id, Sprite portrait, string name, DialogSentence[] saying)
    {
        this.id = id;
        this.portrait = portrait;
        this.name = name;
        this.saying = saying;
    }

    public int GetId()
    {
        return id; 
    }

    public Sprite GetPortrait()
    {
        return portrait;
    }

    public string GetName()
    {
        return name;
    }

    public DialogSentence[] GetSaying()
    {
        return saying;
    }

    public override string ToString()
    {
        string result = name+saying[0];
        return result;
    }

}

public class NPC : MonoBehaviour {

    SpriteRenderer sr;
    Material material_default;
    public Material material_ouline;

    public NpcDataBase dataBase;
    public int id;
    public int[] sellId;
    //public Sprite image;
    //public string name;
    //public string[] content;
    public GameObject Menu;
    public Button dialogButton;
    public Button shopButton;

    public Image QuestSign;
    public Sprite GetMainQuest;
    public Sprite GetSubQuest;
    public Sprite CompleteQuest;

    bool isShowMenu = false;
    bool isOnNpc = false;
    bool isOnMenu = false;
    //NpcInfo npcInfo;
    //Quest tempQuest;
    //该NPC的信息
    NpcInfo npcInfo;

    void Start () {
        //npcInfo = new NpcInfo(id, image, name, content);
        //dialogButton.onClick.AddListener(OnClickDialogButton);
        //shopButton.onClick.AddListener(OnClickShopButton);
        //Debug.Log("添加新的NPC到集合");
        CurMapNpcManager.Instance.AllNpc.Add(this);

        sr = GetComponent<SpriteRenderer>();
        material_default = sr.material;
        DialogManager.Instance.dialogEnd.AddListener(EndDialog);
        //AllNpcManager.Instance.AllNpc.Add(this);
        UpdateSign();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (isShowMenu&&!isOnNpc&&!isOnMenu)
            {
                Menu.SetActive(false);
                isShowMenu = false;
            }
        }
    }

    public  void MouseDown()
    {
        if (CursorManager.Instance.state != CursorState.Dialog)
            return;
        CursorManager.Instance.SetNormal();
        NpcMouseDownEvent();
    }
    public void MouseEnter()
    {
        sr.material = material_ouline;
        isOnNpc = true;
        if (CursorManager.Instance.state == CursorState.Normal)
            CursorManager.Instance.SetDialog();
    }
    public void MouseExit()
    {
        sr.material = material_default;
        isOnNpc = false;
        if (CursorManager.Instance.state == CursorState.Dialog)
            CursorManager.Instance.SetNormal();
    }
    /// <summary>
    /// NPC点击事件
    /// </summary>
    public void NpcMouseDownEvent()
    {
        //if (EventSystem.current.IsPointerOverGameObject())
        //{
        //    //Debug.Log("被UI覆盖，无法点击");
        //    return;
        //}
        if (!isShowMenu&& !DialogManager.Instance.DialogBox.activeSelf)
        {
            Menu.SetActive(true);
            dialogButton.onClick.AddListener(OnClickDialogButton);
            shopButton.onClick.AddListener(OnClickShopButton);
            //Menu.transform.position = Input.mousePosition;
            isShowMenu = true;
        }
        //Menu.transform.position = Input.mousePosition;
    }
    /// <summary>
    /// 开始对话，先获得该NPC的信息，
    /// 
    /// 在任务数据库中查找【已接任务中】是否有endNpcId与该Npc的Id一致的
    /// 如果有，显示该任务的【完成对话】，没有显示NPC的默认语句
    /// 在查找是否有startNpcId与该Npc的Id一致的，有的话显示该任务的【进行对话】
    /// 
    /// 在任务数据库中查找【可接任务中】是否有startNpcId与该Npc的Id一致的
    /// 如果有，显示该任务的【获取对话】，没有显示NPC的默认语句
    /// </summary>
    public void StartDialog()
    {
        //更新NPC的任务提示
        //QuestManager.Instance.UpdateNpcQuestSign();
        //NpcInfo npcInfo;
        //if(!dataBase.FindNpcById(id,out npcInfo))
        //{
        //    Debug.Log("未找到该NPC！");
        //}
        //对话之前检查是否能 完成对话任务
        QuestManager.Instance.DialogQuestCheck(id);
        //查找任务数据库来决定显示什么对话
        DialogSentence[] tempDialog;
        if (QuestManager.Instance.FindDialogByNpcId(npcInfo.id, out tempDialog))
        {
            //DialogController.Instance.ShowDialog(tempDialog);
            DialogManager.Instance.StartDialog(tempDialog);
            tempDialog = null;
        }
        else
            // DialogController.Instance.ShowDialog(npcInfo);
            DialogManager.Instance.StartDialog(npcInfo.saying);

        HideMenu();
    }
    //对话结束
    void EndDialog()
    {
        //更新NPC的任务提示
        //QuestManager.Instance.UpdateNpcQuestSign();
        //根据任务的性质改变任务的状态
        if (QuestManager.Instance.willShowDialogQuest != null)
        {
            int id = QuestManager.Instance.willShowDialogQuest.id;
            //if (QuestManager.Instance.currentQuest.state == QuestState.可接)
            //    QuestManager.Instance.currentQuest.state = QuestState.已接;
            ////else if (tempQuest.state == QuestState.已接收)
            ////tempQuest.state = QuestState.已完成;
            //else if (QuestManager.Instance.currentQuest.state == QuestState.已完成)
            //{
            //    QuestManager.Instance.currentQuest.state = QuestState.终结;
            //    Debug.Log("任务完成，获得奖励！");
            //}
            if (QuestManager.Instance.willShowDialogQuest.state == QuestState.可接)
                QuestManager.Instance.AcceptQuest(id);
            else if (QuestManager.Instance.willShowDialogQuest.state == QuestState.已完成)
                QuestManager.Instance.FinishQuest(id);

            QuestManager.Instance.willShowDialogQuest = null;
        }
        //DialogController.Instance.StopAllCoroutines();
        //Debug.Log("关闭所有协程");
    }

    public void IsOnNpc()
    {
        isOnNpc = true;
    }

    public void IsNotOnNpc()
    {
        isOnNpc = false;
    }

    public void IsOnMenu()
    {
        isOnMenu = true;
    }

    public void IsNotOnMenu()
    {
        isOnMenu = false;
    }
    public void HideMenu()
    {
        Menu.SetActive(false);
        isShowMenu = false;
        isOnNpc = false;
        isOnMenu = false;
    }

    private void OnClickDialogButton()
    {
        //Debug.Log("点击对话按钮");
        StartDialog();
    }

    private void OnClickShopButton()
    {
        //Debug.Log("点击商店按钮");
        ShopController.Instance.ShowShopUI(sellId);
        HideMenu();
    }

    public void ShowCompleteQuestSign()
    {
        //Debug.Log("已经显示了感叹号");
        QuestSign.enabled = true;
        QuestSign.sprite = CompleteQuest;
    }

    public void ShowGetMainQuestSign()
    {
        //Debug.Log("已经显示了金色书");
        QuestSign.enabled = true;
        QuestSign.sprite = GetMainQuest;
    }

    public void ShowNothingSign()
    {
        //Debug.Log("隐藏不显示");
        QuestSign.enabled = false;
    }

    public void UpdateSign()
    {
        if (!dataBase.FindNpcById(id, out npcInfo))
        {
            Debug.Log("未找到该NPC！");
        }
        else
        {
            if (npcInfo.questStateType == NPCQuestStateType.无)
            {
                ShowNothingSign();
            }
            else if (npcInfo.questStateType == NPCQuestStateType.感叹号)
            {
                ShowCompleteQuestSign();
            }
            else if (npcInfo.questStateType == NPCQuestStateType.金色书本)
            {
                ShowGetMainQuestSign();
            }
        }
    }
}
