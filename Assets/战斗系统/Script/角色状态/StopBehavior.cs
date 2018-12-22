using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopBehavior : StateMachineBehaviour
{

    MovementBehavior baseMovement;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        baseMovement = animator.GetBehaviour<MovementBehavior>();

        animator.transform.GetComponent<tk2dSpriteAnimator>().Play("停止");
        animator.SetBool("停止", true);
        animator.SetBool("动作暂停", false);
        animator.SetBool("在地面", true);
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
        animator.SetBool("停止", false);
    }

}
