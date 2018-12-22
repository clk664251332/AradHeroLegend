using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Junp_StartBehavior : StateMachineBehaviour {

    PlayerContorller playerControl;
    Transform playerTrans;
    Rigidbody2D rig;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playerTrans = animator.transform.parent;
        rig = playerTrans.GetComponent<Rigidbody2D>();
        playerControl = playerTrans.GetComponent<PlayerContorller>();

        animator.transform.GetComponent<tk2dSpriteAnimator>().Play("跳跃_起跳");
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //playerControl.Ypos = animator.transform.position.y;//保存Y坐标
        //playerControl.currentJumpSpeed = playerControl.jumpSpeed;//赋初始速度
        playerControl.StartCoroutine("Jump");//开启协程
    }
}
