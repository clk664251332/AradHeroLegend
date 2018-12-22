using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack1Behavior : StateMachineBehaviour {

    Transform playerTrans;
    Transform weaponTrans;
    tk2dSprite weaponSprite;
    tk2dSpriteAnimator tkAnimator;
    //攻击位移控制
    //public float moveSpeed;
    //public float mvoveTime;
    [Space]
    public float DelayTime;
    float time;

    bool FunctionLock;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playerTrans = animator.transform.parent;
        weaponTrans = animator.transform.Find("武器");
        weaponSprite = weaponTrans.GetComponent<tk2dSprite>();
        tkAnimator = animator.transform.GetComponent<tk2dSpriteAnimator>();
        tkAnimator.Play("小砍1");
        time = 0;
        FunctionLock = false;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        time += Time.deltaTime;
        if (time >= DelayTime)
        {
            if (Input.GetButtonDown("Attack"))
                animator.SetTrigger("AttackLink");
        }
        if (tkAnimator.CurrentFrame == 2&&!FunctionLock)
        {
            FunctionLock = true;
            var bounds = weaponSprite.GetBounds();
            //获得所有击中的怪物列表，排除掉Y轴不适合的
            Collider2D[] coliders =
             Physics2D.OverlapBoxAll(bounds.center + weaponTrans.position, bounds.size, 0, 1 << LayerMask.NameToLayer("Monster"));
            if (coliders.Length!=0)
            {
                foreach (var colider in coliders)
                {
                    var monster = (ITakeDamage)colider.gameObject.GetComponent(typeof(ITakeDamage));
                    if (monster == null)
                        continue;
                    else if (!monster.InAttackArea())
                        continue;
                    else if(monster.IsDie())
                        continue;
                    else
                    {
                        if (monster.IsLying())
                        {
                            //躺着挨打
                            monster.TakeDamage("小砍1");
                        }
                        else if (monster.IsFlying())
                        {
                            //在空中挨打
                            monster.SetFlySpeed(5);
                            monster.TakeDamage("小砍1");
                        }
                        else
                        {
                            //这一击是否会导致怪物死亡
                            if (monster.TakeDamage("小砍1"))
                            {
                                int r = Random.Range(1, 4);
                                if (r == 1)
                                    monster.ExecuteDie();
                                else
                                    monster.HitFly(2, 10);
                            }
                            else
                                //击退
                                monster.HitBack(55, 0.22f);
                        }
                        monster.ShowHitEffect("小砍1");
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
