using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_MovementBehavior : StateMachineBehaviour {

    Transform monsterTrans;
    MonsterStateInfo monsterInfo;
    tk2dSprite[] sprites;

    float X_AttackDistance;
    float Y_AttackDistance;
    bool FunctionLock;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        monsterTrans = animator.transform.parent;
        monsterInfo = monsterTrans.GetComponent<MonsterStateInfo>();
        X_AttackDistance = monsterInfo.X_AttackDistance;
        Y_AttackDistance = monsterInfo.Y_AttackDistance;
        sprites = animator.GetComponentsInChildren<tk2dSprite>();
        FunctionLock = false;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Facelayer();
        AttackCheck(animator);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    void Facelayer()
    {
        if (monsterTrans.position.x <= PlayerContorller.Instance.transform.position.x)
        {
            //foreach (var sprite in sprites)
            //    sprite.FlipX = false;
            for (int i = 0; i < sprites.Length; i++)
            {
                sprites[i].FlipX = false;
            }
        }
        else
        {
            //foreach (var sprite in sprites)
            //    sprite.FlipX = true;
            for (int i = 0; i < sprites.Length; i++)
            {
                sprites[i].FlipX = true;
            }
        }
    }

    void AttackCheck(Animator animator)
    {
        if (Mathf.Abs(monsterTrans.position.x - PlayerContorller.Instance.transform.position.x) <= X_AttackDistance
            && Mathf.Abs(monsterTrans.position.y - PlayerContorller.Instance.transform.position.y) <= Y_AttackDistance
            && !FunctionLock&&animator.GetBool("可攻击"))
        {
            animator.SetTrigger("攻击");
            //monsterControl.StartCoroutine(monsterControl.AttackTimer());
            FunctionLock = true;
        }
    }
}
