using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunAttackBehavior : StateMachineBehaviour {

    Transform playerTrans;
    Transform weaponTrans;
    tk2dSprite weaponSprite;
    tk2dSpriteAnimator tkAnimator;
    MovementBehavior baseMovement;
    public float DelayTime;
    float time;
    bool FunctionLock;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playerTrans = animator.transform.parent;
        weaponTrans = animator.transform.Find("武器");
        weaponSprite = weaponTrans.GetComponent<tk2dSprite>();
        tkAnimator = animator.transform.GetComponent<tk2dSpriteAnimator>();
        FunctionLock = false;
        time = 0;
        tkAnimator.Play("前冲攻击");
        baseMovement = animator.GetBehaviour<MovementBehavior>();
        //设置速度
        baseMovement.currentSpeed = baseMovement.WalkSpeed;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        time += Time.deltaTime;
        if (time >= DelayTime)
        {
            if (Input.GetButtonDown("Attack"))
                animator.SetTrigger("AttackLink");
        }

        if (tkAnimator.CurrentFrame == 2 && !FunctionLock)
        {
            FunctionLock = true;
            var bounds = weaponSprite.GetBounds();
            Collider2D[] coliders =
             Physics2D.OverlapBoxAll(bounds.center + weaponTrans.position, bounds.size, 0, 1 << LayerMask.NameToLayer("Monster"));
            if (coliders.Length != 0)
            {
                foreach (var colider in coliders)
                {
                    var monster = (ITakeDamage)colider.gameObject.GetComponent(typeof(ITakeDamage));
                    if (monster == null)
                        continue;
                    else if (!monster.InAttackArea())
                        continue;
                    else if (monster.IsDie())
                        continue;
                    else
                    {
                        if (!monster.IsLying())
                        {
                            if (monster.TakeDamage("小砍2") && !monster.IsFlying())
                                monster.HitFly(10, 40);
                            else
                                monster.HitBack(50, 0.17f);
                        }
                        monster.ShowHitEffect("小砍2");
                    }
                }
            }
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        time = 0;
    }
}
