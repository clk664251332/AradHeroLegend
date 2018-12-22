using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITakeDamage
{
    void HitBack(float speed, float time);
    void HitFly(float Yspeed, float Xspeed);
    void ExecuteDie();
    void ShowHitEffect(string actionName);
    bool InAttackArea();
    bool IsLying();
    bool IsFlying();
    bool IsDie();
    bool TakeDamage(string actionName);
    void SetFlySpeed(float speed);
}
/// <summary>
/// 该类放在怪物的身体上，也就是碰撞盒
/// </summary>
public class Monster_HitReceive : MonoBehaviour,ITakeDamage
{
    MonsterController monsterControl;
    MonsterStateInfo monsterInfo;
    Animator anim;

    private void Start()
    {
        monsterControl = transform.parent.GetComponent<MonsterController>();
        monsterInfo = transform.parent.GetComponent<MonsterStateInfo>();
        anim = GetComponent<Animator>();
    }

    public void HitBack(float speed, float time)
    {
        //anim.SetTrigger("被击中");
        monsterControl.StartCoroutine(monsterControl.XTimeMove(speed, time));
    }

    public void HitFly(float Yspeed,float Xspeed)
    {
        //anim.SetTrigger("被击中");
        monsterControl.StartCoroutine(monsterControl.HitFly(Yspeed, Xspeed));
    }

    public void ShowHitEffect(string actionName)
    {
        monsterControl.ShowHitEffect(actionName);
    }

    public bool InAttackArea()
    {
        return Mathf.Abs(PlayerContorller.Instance.transform.position.y - monsterControl.transform.position.y) < 20;
    }

    public bool IsLying()
    {
        return anim.GetBool("在躺着");
    }
    public void SetFlySpeed(float speed)
    {
        monsterControl.currentFlySpeed = speed;
    }
    public bool IsFlying()
    {
        return anim.GetBool("在空中");
    }
    public bool IsDie()
    {
        //return monsterInfo.IsDie;
        return anim.GetBool("死亡");
    }
    //计算伤害血量减去伤害,死亡的话返回true，并显示伤害数字
    public bool TakeDamage(string actionName)
    {

        bool critical = false;
        //基础伤害，保底伤害，最终伤害
        int BaseDamage = 0;
        int MinDamage = PlayerStateInfo.Instance.PhysicalAttack.Get() / 2 + UnityEngine.Random.Range(1, 4);
        int Damage = 0;

        BaseDamage= (int)((PlayerStateInfo.Instance.PhysicalAttack.Get()-monsterInfo.PhysicalDefence)* UnityEngine.Random.Range(0.9f, 1.1f));

        if (BaseDamage <= 0)
            Damage = MinDamage;
        else
        {
            BaseDamage += MinDamage;
            Damage = (int)(BaseDamage * UnityEngine.Random.Range(0.8f, 1.4f));
        }
        if (UnityEngine.Random.Range(1, 101) <= PlayerStateInfo.Instance.CriticalChance.Get())
        {
            critical = true;
            Damage = (int)(Damage * UnityEngine.Random.Range(1.1f, 1.9f));
        }
        //怪物掉血
        monsterInfo.CurrentHp.SetWithArgs(monsterInfo.CurrentHp.Get() - Damage, monsterInfo.gameObject, monsterInfo.monsterArgs);
        //创建伤害文字
        if (!critical)
            FloatingTextController.Instance.CreatDamageText(Damage.ToString(), new Vector2(transform.position.x, transform.position.y + 80));
        else
            FloatingTextController.Instance.CreatCriticalDamageText(Damage.ToString(), new Vector2(transform.position.x, transform.position.y + 80));

        //return monsterInfo.CurrentHp.Get() <= 0;
        if (monsterInfo.CurrentHp.Get() <= 0)
        {
            return true;
        }else
        {
            anim.SetTrigger("被击中");
            return false;
        }
    }
    /// <summary>
    /// 对怪物执行死亡操作,停止怪物的动画
    /// </summary>
    public void ExecuteDie()
    {
        monsterControl.Die();
    }
}
