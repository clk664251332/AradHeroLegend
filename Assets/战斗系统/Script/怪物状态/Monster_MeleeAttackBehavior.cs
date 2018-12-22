using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_MeleeAttackBehavior : StateMachineBehaviour {

    tk2dSpriteAnimator tkAnimator;
    MonsterController monsterControl;
    MonsterStateInfo monsterStateInfo;
    Transform monsterTrans;
    Transform weaponTrans;
    tk2dSprite weaponSprite;
    bool FunctionLock;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        monsterTrans = animator.transform.parent;
        monsterControl = monsterTrans.GetComponent<MonsterController>();
        monsterStateInfo = monsterTrans.GetComponent<MonsterStateInfo>();
        weaponTrans = animator.transform.Find("武器");
        weaponSprite = weaponTrans.GetComponent<tk2dSprite>();
        tkAnimator = animator.transform.GetComponent<tk2dSpriteAnimator>();
        tkAnimator.Play("攻击");
        FunctionLock = false;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (tkAnimator.CurrentFrame == 2 && !FunctionLock)
        {
            FunctionLock = true;
            var bounds = weaponSprite.GetBounds();
            Collider2D colider =
             Physics2D.OverlapBox(bounds.center + weaponTrans.position, bounds.size, 0, 1 << LayerMask.NameToLayer("Player"));
            if(colider!=null)
            {
                if (Mathf.Abs(monsterTrans.position.y - PlayerContorller.Instance.transform.position.y) <= 20)
                {
                    //怪物在角色右边，给向左的击打，速度方向为负
                    if (monsterTrans.position.x >= PlayerContorller.Instance.transform.position.x)
                    {
                        PlayerContorller.Instance.StartCoroutine(PlayerContorller.Instance.GetHitMove(-50, 0.3f));
                        PlayerContorller.Instance.ShowHitEffect("击打");
                    }
                    else
                    {
                        PlayerContorller.Instance.StartCoroutine(PlayerContorller.Instance.GetHitMove(50, 0.3f));
                        PlayerContorller.Instance.ShowHitEffect("击打");
                    }
                    PlayerContorller.Instance.TakeDamage(monsterStateInfo.PhysicalAttack, monsterStateInfo.MagicAttack);
                }
            }
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("可攻击", false);
        monsterControl.StartCoroutine(monsterControl.AttackTimer());
    }
}
