using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_RangeAttackBehavior : StateMachineBehaviour
{
    tk2dSpriteAnimator tkAnimator;
    MonsterController monsterControl;
    MonsterStateInfo monsterStateInfo;
    Transform monsterTrans;
    //Transform weaponTrans;
    //tk2dSprite weaponSprite;
    bool FunctionLock;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        monsterTrans = animator.transform.parent;
        monsterControl = monsterTrans.GetComponent<MonsterController>();
        monsterStateInfo = monsterTrans.GetComponent<MonsterStateInfo>();
        //weaponTrans = animator.transform.Find("武器");
        //weaponSprite = weaponTrans.GetComponent<tk2dSprite>();
        tkAnimator = animator.transform.GetComponent<tk2dSpriteAnimator>();
        tkAnimator.Play("攻击");
        FunctionLock = false;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (tkAnimator.CurrentFrame == 2 && !FunctionLock)
        {
            FunctionLock = true;
            //远程攻击行为，应该创建一个子弹物体，并且赋给子弹物体一些参数
            GameObject weapon =
            Instantiate(monsterStateInfo.weapon, monsterTrans.position, Quaternion.identity);
            int direction = (monsterTrans.position.x >= PlayerContorller.Instance.transform.position.x) ? -1 : 1;
            weapon.GetComponent<Bullet>().Initial(monsterStateInfo.PhysicalAttack, monsterStateInfo.MagicAttack, direction);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("可攻击", false);
        monsterControl.StartCoroutine(monsterControl.AttackTimer());
    }
}
