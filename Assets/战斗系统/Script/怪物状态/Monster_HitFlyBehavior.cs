using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_HitFlyBehavior : StateMachineBehaviour {

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.transform.GetComponent<tk2dSpriteAnimator>().Play("击飞");
        animator.SetBool("在空中", true);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
