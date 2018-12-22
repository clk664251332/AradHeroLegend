using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JunpBehavior : StateMachineBehaviour {

    Transform playerTrans;
    Rigidbody2D rig;
    MovementBehavior baseMove;
    tk2dSprite[] sprites;

    //bool facingRight;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playerTrans = animator.transform.parent;
        rig = playerTrans.GetComponent<Rigidbody2D>();
        baseMove = animator.GetBehaviour<MovementBehavior>();
        sprites = animator.GetComponentsInChildren<tk2dSprite>();
        //facingRight = playerTrans.localScale.x == 1 ? true : false;
        animator.transform.GetComponent<tk2dSpriteAnimator>().Play("跳跃_上升");
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        MoveLogic(animator);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    void MoveLogic(Animator animator)
    {
        //左右键
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        rig.velocity = new Vector2(h * baseMove.currentSpeed, 0);
        if (h > 0)
        {
            foreach (var sprite in sprites)
                sprite.FlipX = false;
            baseMove.facingLeft = false;
        }
        else if (h < 0)
        {
            foreach (var sprite in sprites)
                sprite.FlipX = true;
            baseMove.facingLeft = true;
        }

    }
}
