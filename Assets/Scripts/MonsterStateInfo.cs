using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStateInfo : MonoBehaviour {
    //配置怪物的一些参数
    //物理攻击 物理防御 魔法攻击 魔法防御
    //行走速度 行走间隔 受击僵直时间
    //X轴攻击范围 Y轴攻击范围
    public string MonsterName;
    public Sprite MonsterPortrait;
    public int MaxHp;
    //[HideInInspector]
    public Value<int> CurrentHp;
    public int PhysicalAttack;
    public int PhysicalDefence;
    public int MagicAttack;
    public int MagicDefence;

    public float WalkSpeed;
    public float WalkTimeDelta;

    public float HitStiffTime;

    public float X_AttackDistance;
    public float Y_AttackDistance=20f;
    //血条长度
    public int HpBarWidth;
    //一层血条的血量
    public int OneBarHp;
    //
    public int Exp;
    public MonsterEventArgs monsterArgs;
    //[HideInInspector]
    //public bool IsDie=false;
    public bool isBoss;

    public GameObject weapon;

    private void Start()
    {
        CurrentHp = new Value<int>(0);
        monsterArgs = new MonsterEventArgs();

        if (OneBarHp > MaxHp)
            OneBarHp = MaxHp;

        monsterArgs.curHp = CurrentHp;
        monsterArgs.BarWidth = HpBarWidth;
        monsterArgs.OneBarHp = OneBarHp;
        monsterArgs.MonsterName = MonsterName;
        monsterArgs.MonsterPortrait = MonsterPortrait;
        monsterArgs.isBoss = isBoss;

        CurrentHp.SetWithArgs(MaxHp, gameObject, monsterArgs);
        CurrentHp.AddChangeEventListener(HpBarController.Instance.OnHpChange);
    }

}


public class MonsterEventArgs : EventArgs
{
    public string MonsterName;
    public Sprite MonsterPortrait;
    public bool isBoss;
    //当前血量
    public Value<int> curHp;
    //血条长度
    public int BarWidth;
    //一层血条的血量
    public int OneBarHp;
}
