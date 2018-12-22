using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack3Behavior : StateMachineBehaviour {

    Transform playerTrans;
    Transform weaponTrans;
    tk2dSprite weaponSprite;
    tk2dSpriteAnimator tkAnimator;
    //攻击位移控制
    public float moveSpeed;
    public float quickMoveSpeed;

    bool FunctionLock;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playerTrans = animator.transform.parent;
        weaponTrans = animator.transform.Find("武器");
        weaponSprite = weaponTrans.GetComponent<tk2dSprite>();
        tkAnimator = animator.transform.GetComponent<tk2dSpriteAnimator>();
        tkAnimator.Play("小砍3");
        FunctionLock = false;

        float h = Input.GetAxisRaw("Horizontal");
        if (h != 0)
            PlayerContorller.Instance.XMove(quickMoveSpeed);
        else
            PlayerContorller.Instance.XMove(moveSpeed);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (tkAnimator.CurrentFrame == 3 && !FunctionLock)
        {
            FunctionLock = true;
            var bounds = weaponSprite.GetBounds();
            Collider2D[] coliders =
             Physics2D.OverlapBoxAll(bounds.center + weaponTrans.position, bounds.size, 0);
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
                        if (!monster.IsFlying())
                        {
                            if (!monster.IsLying())
                                monster.HitFly(10,40);
                            else
                                monster.HitFly(9,40);
                        }
                        //damage.HitBack(65, 0.4f);
                        monster.ShowHitEffect("小砍3");
                        monster.TakeDamage("小砍3");
                    }
                }
            }
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
