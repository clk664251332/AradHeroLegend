using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//一般状态，包括停止，行走，跑动（可进行攻击，跳跃，技能等操作）
public class MovementBehavior : StateMachineBehaviour {

    Transform playerTrans;
    Rigidbody2D rig;
    tk2dSprite[] sprites;
    //设置变量
    public float WalkSpeed;
    public float RunSpeed;
    //控制变量
    [HideInInspector]
    public bool facingLeft;
    [HideInInspector]
    public float currentSpeed;


    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        playerTrans = animator.transform.parent;
        rig = playerTrans.GetComponent<Rigidbody2D>();
        sprites = animator.GetComponentsInChildren<tk2dSprite>();
        animator.SetBool("动作暂停", false);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //移动
        MoveLogic(animator);
        //跳跃检测
        JumpLogic(animator);
        //技能检测
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rig.velocity = Vector2.zero;
    }

    void MoveLogic(Animator animator)
    {
        //左右键
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        if (PlayerContorller.Instance.canMove)
            rig.velocity = new Vector2(h * currentSpeed, v * WalkSpeed);
        else
            rig.velocity = Vector2.zero;
        if (h > 0)
        {
            //foreach (var sprite in sprites)
            //    sprite.FlipX = false;
            for(int i = 0; i < sprites.Length; i++)
            {
                sprites[i].FlipX = false;
            }
            facingLeft = false;
        }else if (h < 0)
        {
            //foreach (var sprite in sprites)
            //    sprite.FlipX = true;
            for (int i = 0; i < sprites.Length; i++)
            {
                sprites[i].FlipX = true;
            }
            facingLeft = true;
        }

        animator.SetFloat("Speed", h != 0 || v != 0 ? currentSpeed : 0);
    }
   
    void JumpLogic(Animator animator)
    {
        //animator.SetBool("在地面", true);
        if (Input.GetButtonDown("Jump"))
        {
            if (!animator.GetBool("在地面"))
            {
                return;
            }
            else
            {
                //playerControl.Ypos = playerTrans.position.y;//保存Y坐标
                //playerControl.currentJumpSpeed = playerControl.jumpSpeed;//赋初始速度
                //playerControl.StartCoroutine("Jump");//开启协程
                animator.SetBool("在地面", false);//改变状态
            }
        }
    }

    //void Flip()
    //{
    //    //facingRight = !facingRight;
    //    //Vector2 scale = playerTrans.localScale;
    //    //scale.x *= -1;
    //    //playerTrans.localScale = scale;
    //    foreach (var i in sprites)
    //        i.FlipX = true;
    //}

}
