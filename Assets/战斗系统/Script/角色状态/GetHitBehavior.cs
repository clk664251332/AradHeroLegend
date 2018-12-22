using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetHitBehavior : StateMachineBehaviour {

    //被攻击位移控制
    //public float moveSpeed;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        int r = Random.Range(1, 3);
        animator.transform.GetComponent<tk2dSpriteAnimator>().Play("被击中"+r.ToString());
        //PlayerContorller.Instance.XMove(moveSpeed);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
