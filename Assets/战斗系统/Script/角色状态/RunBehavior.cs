using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunBehavior : StateMachineBehaviour {

    MovementBehavior baseMovement;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        baseMovement = animator.GetBehaviour<MovementBehavior>();
        animator.transform.GetComponent<tk2dSpriteAnimator>().Play("跑步");
        animator.SetBool("跑动", true);
        //设置速度
        baseMovement.currentSpeed = baseMovement.RunSpeed;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //监听按键
        if (Input.GetButtonDown("Attack"))
        {
            animator.SetTrigger("RunAttack");
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("跑动", false);
        //设置速度
        animator.SetFloat("Speed", 0);
    }
}
