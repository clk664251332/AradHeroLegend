using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkBehavior : StateMachineBehaviour {

    MovementBehavior baseMovement;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        baseMovement = animator.GetBehaviour<MovementBehavior>();
        animator.transform.GetComponent<tk2dSpriteAnimator>().Play("走动");
        animator.SetBool("走动", true);
        //设置速度
        baseMovement.currentSpeed = baseMovement.WalkSpeed;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //监听按键
        if (Input.GetButtonDown("Attack"))
            animator.SetTrigger("AttackStart");
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("走动", false);
    }
}
