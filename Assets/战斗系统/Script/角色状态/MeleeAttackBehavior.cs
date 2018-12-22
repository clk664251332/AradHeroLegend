using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackBehavior : StateMachineBehaviour {

    MovementBehavior baseMovement;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        baseMovement = animator.GetBehaviour<MovementBehavior>();
        animator.SetBool("动作暂停", true);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
