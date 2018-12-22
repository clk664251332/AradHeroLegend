using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public  class HpBarController : MonoSingleton<HpBarController> {

    public Transform HpBarUI;
    public Transform FlashTrans;
    public GameObject FlashArea;
    //public int damage;
    public Image TopBar;
    //public Image TopBarShadow;
    public Image BottomBar;
    public Text MonsterName;
    public Image MonsterPortrait;
    public Image MonsterPortraitBox;
    public Image HpBarBox;
    public Sprite commonPortraitBoxSprite;
    public Sprite bossPortraitBoxSprite;
    public Sprite commonHpBarBoxSprite;
    public Sprite bossHpBarBoxSprite;

    public int BarWidth;//血条长度
    private float m_BarHeight;
    public int AllHp;//总血量
    //public int curHp;//当前血量
    //public Value<int> curHp=new Value<int>(0);//当前血量
    public Value<int> curHp;
    public int OneBarHp;//一层血量

    public float ratio;
    public int level;

    //记录当前怪物的id
    private int lastMonsterId;
    //
    public delegate void DestoryFlashDelegate(int id);
    public DestoryFlashDelegate DontDestoryFlashById;

    private const float commonHpbarHeight = 10.52f;
    private const float BossHpbarHeight = 24f;
    private const float commonMonsterNameY = 13.33f;//-6.67f;
    private const float BossNameY = 5.5f;//-14.5f;


    bool init=true;
    private void Start()
    {
        //queue = new Queue<Vector2>();
        TopBar.rectTransform.sizeDelta = new Vector2(BarWidth, TopBar.rectTransform.sizeDelta.y);
        BottomBar.rectTransform.sizeDelta = new Vector2(BarWidth, BottomBar.rectTransform.sizeDelta.y);
        m_BarHeight = TopBar.rectTransform.sizeDelta.y;
        //curHp.AddChangeListener(OnHpChange);
        //curHp.Set(AllHp);
        HpShadowBar.Instance.UpdateColor();
        //OnHpChange();
        init = false;
        //InitColor();
        HpBarUI.gameObject.SetActive(false);
    }

    public void OnHpChange(object obj, EventArgs eventArgs)
    {
        GameObject curMonsterObj = (GameObject)obj;
        int curMonsterInstanceId = curMonsterObj.GetInstanceID();

        HpBarUI.gameObject.SetActive(true);
        MonsterEventArgs monsterInfo = (MonsterEventArgs)eventArgs;
        //之前血量的比率
        float lastRatio = (float)Math.Round((double)monsterInfo.curHp.GetLastValue() / monsterInfo.OneBarHp, 3);
        //之后血量的比率
        ratio = (float)Math.Round((double)monsterInfo.curHp.Get() / monsterInfo.OneBarHp, 3);
        if (ratio < 0)
            ratio = 0;
        //
        level = (int)ratio % 5;
        //
       

        #region 闪烁逻辑
        //计算出闪烁区域的宽度
        if (Mathf.Abs(lastRatio - ratio) <= 1 )
        {
            //同一条血条
            if ((int)lastRatio == (int)ratio)
            {
                float flashWidth = (lastRatio - ratio) * monsterInfo.BarWidth;
                float positionX = GetDotAmount(lastRatio) * monsterInfo.BarWidth;

                //经过检测之后的宽度与位置
                //float flashWidth = (GetDotAmount(lastRatio) - CheckFlashQueue(GetDotAmount(ratio))) * BarWidth;
                ////float positionX = GetDotAmount(CheckFlashQueue(ratio)) * BarWidth;
                //float positionX =CheckFlashQueue(GetDotAmount(ratio)) * BarWidth;

                //创建闪烁区域
                if (!init)//&& positionX!= BarWidth)
                {
                    GameObject instance = Instantiate(FlashArea, FlashTrans);
                    if (monsterInfo.isBoss)
                        instance.GetComponent<FlashArea>().StartFlash(new Vector2(positionX, 0), new Vector2(flashWidth, BossHpbarHeight), curMonsterInstanceId);
                    else
                        instance.GetComponent<FlashArea>().StartFlash(new Vector2(positionX, 0), new Vector2(flashWidth, commonHpbarHeight), curMonsterInstanceId);
                    //将GetDotAmount(ratio)以及GetDotAmount(lastRatio)插入队列
                    //queue.Enqueue(new Vector2(GetDotAmount(ratio), GetDotAmount(lastRatio)));
                    //Debug.Log(0);
                }
            }
            else
            {
                float flashWidth1 = GetDotAmount(lastRatio) * monsterInfo.BarWidth;
                float flashWidth2 = (1 - GetDotAmount(ratio)) * monsterInfo.BarWidth;
                float positionX1 = GetDotAmount(lastRatio) * monsterInfo.BarWidth;

                //float flashWidth1 = GetDotAmount(lastRatio) * BarWidth;
                //float flashWidth2 = (1 - CheckFlashQueue(GetDotAmount(ratio))) * BarWidth;
                //float positionX2 = CheckFlashQueue(GetDotAmount(ratio)) * BarWidth;

                GameObject instance1 = Instantiate(FlashArea, FlashTrans);
                if (monsterInfo.isBoss)
                    instance1.GetComponent<FlashArea>().StartFlash(new Vector2(positionX1, 0), new Vector2(flashWidth1, BossHpbarHeight), curMonsterInstanceId);
                else
                    instance1.GetComponent<FlashArea>().StartFlash(new Vector2(positionX1, 0), new Vector2(flashWidth1, commonHpbarHeight), curMonsterInstanceId);
                //queue.Enqueue(new Vector2(0, GetDotAmount(lastRatio)));
                //Debug.Log(1);

                //if(positionX2!= BarWidth)
                //{
                GameObject instance2 = Instantiate(FlashArea, FlashTrans);
                if (monsterInfo.isBoss)
                    instance2.GetComponent<FlashArea>().StartFlash(new Vector2(monsterInfo.BarWidth, 0), new Vector2(flashWidth2, BossHpbarHeight), curMonsterInstanceId);
                else
                    instance2.GetComponent<FlashArea>().StartFlash(new Vector2(monsterInfo.BarWidth, 0), new Vector2(flashWidth2, commonHpbarHeight), curMonsterInstanceId);
                //queue.Enqueue(new Vector2(GetDotAmount(ratio), 1));
                //Debug.Log(2);
                //}
            }

        }
        else
        {
            if (!init)
            {
                //经过检测之后的宽度与位置
                //float flashWidth = (lastRatio - CheckFlashQueue(ratio)) * BarWidth;
                //float positionX = GetDotAmount(CheckFlashQueue(ratio)) * BarWidth;

                GameObject instance = Instantiate(FlashArea, FlashTrans);
                if (monsterInfo.isBoss)
                    instance.GetComponent<FlashArea>().StartFlash(Vector2.zero, new Vector2(monsterInfo.BarWidth, BossHpbarHeight), curMonsterInstanceId);
                else
                    instance.GetComponent<FlashArea>().StartFlash(Vector2.zero, new Vector2(monsterInfo.BarWidth, commonHpbarHeight), curMonsterInstanceId);
                //queue.Enqueue(Vector2.one);
                //Debug.Log("full");
            }
        }
        #endregion
        //减去的血量小于一条
        if (Mathf.Abs(lastRatio - ratio) <= 1)
        {
            //如果涉及两个血条
            if ((int)lastRatio != (int)ratio)
            {
                TopBar.fillAmount = 0;
                BottomBar.fillAmount = GetDotAmount(ratio);
            }
            //同一个血条上增减
            else
            {
                TopBar.fillAmount = GetDotAmount(ratio);
                BottomBar.fillAmount = 1;
            } 
        }
        else
        {
            //减去的血量大于一条还多
            if (init)
                TopBar.fillAmount = GetDotAmount(ratio);
            else
            {
                TopBar.fillAmount = 0;
                BottomBar.fillAmount = 0;
                //Debug.Log("减血大于一条还多");
            }
            //TopBar.fillAmount = GetDotAmount(ratio);
        }
        //更新血条相关的显示
        MonsterName.text = monsterInfo.MonsterName;
        MonsterPortrait.sprite = monsterInfo.MonsterPortrait;
        if (monsterInfo.isBoss)
        {
            TopBar.rectTransform.sizeDelta = new Vector2(BarWidth, BossHpbarHeight);
            BottomBar.rectTransform.sizeDelta = new Vector2(BarWidth, BossHpbarHeight);
            HpShadowBar.Instance.TopShadowBar.rectTransform.sizeDelta = new Vector2(BarWidth, BossHpbarHeight);
            HpShadowBar.Instance.BottomShadowBar.rectTransform.sizeDelta = new Vector2(BarWidth, BossHpbarHeight);
            MonsterPortraitBox.sprite = bossPortraitBoxSprite;
            HpBarBox.sprite = bossHpBarBoxSprite;
            MonsterName.rectTransform.localPosition = new Vector2(MonsterName.rectTransform.localPosition.x, BossNameY);
        }
        else
        {
            TopBar.rectTransform.sizeDelta = new Vector2(BarWidth, commonHpbarHeight);
            BottomBar.rectTransform.sizeDelta = new Vector2(BarWidth, commonHpbarHeight);
            HpShadowBar.Instance.TopShadowBar.rectTransform.sizeDelta = new Vector2(BarWidth, commonHpbarHeight);
            HpShadowBar.Instance.BottomShadowBar.rectTransform.sizeDelta = new Vector2(BarWidth, commonHpbarHeight);
            MonsterPortraitBox.sprite = commonPortraitBoxSprite;
            HpBarBox.sprite = commonHpBarBoxSprite;
            MonsterName.rectTransform.localPosition = new Vector2(MonsterName.rectTransform.localPosition.x, commonMonsterNameY);

        }

        //如果当前击到的怪物和之前的不一样，如果之前的闪烁还在，就删掉之前的闪烁,仅留当前的闪烁

        if (curMonsterInstanceId != lastMonsterId)
        {
            DontDestoryFlashById(curMonsterInstanceId);
            lastMonsterId = curMonsterInstanceId;
            HpShadowBar.Instance.ForceUpdate();//强制更新阴影进度
        }
        HpShadowBar.Instance.UpdateColor();
    }


    //public void Test()
    //{
    //    curHp.Set(curHp.Get() - damage);
    //}

    /// <summary>
    /// 取浮点数的小数部分
    /// </summary>
    /// <param name="number"></param>
    /// <returns></returns>
    float GetDotAmount(float number)
    {
        return number - (int)number;
    }
    /// <summary>
    /// 检测是否能插入一个闪烁而不重叠
    /// </summary>
    /// <param name="leftValue"></param>
    //float CheckFlashQueue(float leftValue)
    //{
    //    if (queue.Count == 0)
    //        return leftValue;
    //    foreach (var temp in queue)
    //    {
    //        if (leftValue < temp.y)
    //            leftValue = temp.y;
    //    }
    //    Debug.Log(leftValue);
    //    return leftValue;
    //}
}
