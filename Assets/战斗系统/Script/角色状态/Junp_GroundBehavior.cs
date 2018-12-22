using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Junp_GroundBehavior : StateMachineBehaviour {

    Rigidbody2D rig;
    Transform playerTrans;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playerTrans = animator.transform.parent;
        rig = playerTrans.GetComponent<Rigidbody2D>();

        rig.velocity = Vector2.zero;
        animator.transform.GetComponent<tk2dSpriteAnimator>().Play("跳跃_着地");
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
