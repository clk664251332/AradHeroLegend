using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_WalkBehavior : StateMachineBehaviour {

    Transform monsterTrans;
    Rigidbody2D monsterRig;
    MonsterStateInfo monsterInfo;
    float walkSpeed;
    bool XMove;
    bool YMove;
    Vector2 X_Velocity;
    Vector2 Y_Velocity;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        monsterTrans = animator.transform.parent;
        monsterRig = monsterTrans.GetComponent<Rigidbody2D>();
        monsterInfo = monsterTrans.GetComponent<MonsterStateInfo>();
        walkSpeed = monsterInfo.WalkSpeed;
        animator.transform.GetComponent<tk2dSpriteAnimator>().Play("行走");
        X_Velocity = new Vector2(walkSpeed, 0);
        Y_Velocity = new Vector2(0, walkSpeed);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        AvoidObstacle(animator);
        MoveToTargrtPosition(animator);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
    /// <summary>
    /// 随时监测障碍，动态改变目的地
    /// </summary>
    /// <param name="animator"></param>
    void AvoidObstacle(Animator animator)
    {
        float TargetX = animator.GetFloat("TargetX");
        float TargetY = animator.GetFloat("TargetY");

        if (TargetX == monsterTrans.position.x && TargetY == monsterTrans.position.y)
            return;
        //随时监测上下左右是否能通过
        bool ObstacleRight = !Physics2D.Raycast(monsterTrans.position, Vector2.right, 3, 1 << LayerMask.NameToLayer("障碍物"));
        bool ObstacleLeft = !Physics2D.Raycast(monsterTrans.position, Vector2.left, 3, 1 << LayerMask.NameToLayer("障碍物"));
        bool ObstacleUp = !Physics2D.Raycast(monsterTrans.position, Vector2.up, 3, 1 << LayerMask.NameToLayer("障碍物"));
        bool ObstacleDown = !Physics2D.Raycast(monsterTrans.position, Vector2.down, 3, 1 << LayerMask.NameToLayer("障碍物"));

        bool boundRight = !Physics2D.Raycast(monsterTrans.position, Vector2.right, 3, 1 << LayerMask.NameToLayer("地图边界"));
        bool boundLeft = !Physics2D.Raycast(monsterTrans.position, Vector2.left, 3, 1 << LayerMask.NameToLayer("地图边界"));
        bool boundUp = !Physics2D.Raycast(monsterTrans.position, Vector2.up, 3, 1 << LayerMask.NameToLayer("地图边界"));
        bool boundDown = !Physics2D.Raycast(monsterTrans.position, Vector2.down, 3, 1 << LayerMask.NameToLayer("地图边界"));

        //-------左右走遇到障碍会上下移动避开-----------------------------
        //尝试向右走
        if (TargetX > monsterTrans.position.x&&XMove)
        {
            float X_Offset = TargetX - monsterTrans.position.x;
            #region 障碍检测
            //如果右边能走
            if (ObstacleRight)
            {

            }
            else
            {
                //如果上下都可以走，找路程短的走
                if (ObstacleUp && ObstacleDown)
                {
                    TargetX = monsterTrans.position.x;//X就在本地
                    float nextY_up = monsterTrans.position.y + 5;
                    int added_up = 0;
                    float nextY_down = monsterTrans.position.y - 5;
                    int added_down = 0;
                    //先尝试上边
                    while (Physics2D.Raycast(new Vector2(TargetX, nextY_up), Vector2.right, 3, 1 << LayerMask.NameToLayer("障碍物")))
                    {
                        nextY_up += 5;
                        added_up += 1;
                    }
                    //再尝试下边
                    while (Physics2D.Raycast(new Vector2(TargetX, nextY_down), Vector2.right, 3, 1 << LayerMask.NameToLayer("障碍物")))
                    {
                        nextY_down -= 5;
                        added_down += 1;
                    }
                    //Debug.Log("下检测" + added_down);
                    //Debug.Log("上检测" + added_up);
                    //比较大小,用次数少的
                    if (added_down <= added_up)
                        TargetY = nextY_down-5;
                    else
                        TargetY = nextY_up+5;
                }else
                {
                    if (ObstacleUp)
                    {
                        TargetX = monsterTrans.position.x;//X就在本地
                        while (Physics2D.Raycast(new Vector2(TargetX, TargetY + 5), Vector2.right, 3, 1 << LayerMask.NameToLayer("障碍物")))
                        {
                            TargetY += 5;
                        }

                    }
                    else if(ObstacleDown)
                    {
                        TargetX = monsterTrans.position.x;//X就在本地
                        while (Physics2D.Raycast(new Vector2(TargetX, TargetY - 5), Vector2.right, 3, 1 << LayerMask.NameToLayer("障碍物")))
                        {
                            TargetY -= 5;
                        }
                    }
                    else if(ObstacleLeft)
                    {
                        TargetX = monsterTrans.position.x - X_Offset;
                        TargetY = monsterTrans.position.y;
                    }
                }
            }
            #endregion
            #region 地图边界检测
            if(!boundRight)
            {
                TargetX = monsterTrans.position.x;//X就在本地
            }
            #endregion
        }
        //尝试向左走
        else if (TargetX < monsterTrans.position.x && XMove)
        {
            float X_Offset = TargetX - monsterTrans.position.x;
            #region 障碍检测
            //如果左边能走
            if (ObstacleLeft)
            {

            }
            else
            {
                //如果上下都可以走，找路程短的走
                if (ObstacleUp && ObstacleDown)
                {
                    TargetX = monsterTrans.position.x;//X就在本地
                    float nextY_up = monsterTrans.position.y + 5;
                    int added_up = 0;
                    float nextY_down = monsterTrans.position.y - 5;
                    int added_down = 0;
                    //先尝试上边
                    while (Physics2D.Raycast(new Vector2(TargetX, nextY_up), Vector2.left, 3, 1 << LayerMask.NameToLayer("障碍物")))
                    {
                        nextY_up += 5;
                        added_up += 1;
                    }
                    //再尝试下边
                    while (Physics2D.Raycast(new Vector2(TargetX, nextY_down), Vector2.left, 3, 1 << LayerMask.NameToLayer("障碍物")))
                    {
                        nextY_down -= 5;
                        added_down += 1;
                    }
                    //Debug.Log("下检测" + added_down);
                    //Debug.Log("上检测" + added_up);
                    //比较大小,用次数少的
                    if (added_down <= added_up)
                        TargetY = nextY_down-5;
                    else
                        TargetY = nextY_up+5;
                }
                else
                {
                    if (ObstacleUp)
                    {
                        TargetX = monsterTrans.position.x;//X就在本地
                        while (Physics2D.Raycast(new Vector2(TargetX, TargetY + 5), Vector2.left, 3, 1 << LayerMask.NameToLayer("障碍物")))
                        {
                            TargetY += 5;
                        }

                    }
                    else if (ObstacleDown)
                    {
                        TargetX = monsterTrans.position.x;//X就在本地
                        while (Physics2D.Raycast(new Vector2(TargetX, TargetY - 5), Vector2.left, 3, 1 << LayerMask.NameToLayer("障碍物")))
                        {
                            TargetY -= 5;
                        }
                    }
                    else if (ObstacleRight)
                    {
                        TargetX = monsterTrans.position.x + X_Offset;
                        TargetY = monsterTrans.position.y;
                    }
                }
            }
            #endregion
            #region 地图边界检测
            if (!boundLeft)
            {
                TargetX = monsterTrans.position.x;//X就在本地
            }
            #endregion
        }
        //-------上下走遇到障碍会直接停止---------------------------------
        if (TargetY > monsterTrans.position.y && YMove)
        {
            if(!ObstacleUp||!boundUp)
                TargetY = monsterTrans.position.y;
        }
        else if (TargetY < monsterTrans.position.y && YMove)
        {
            if (!ObstacleDown || !boundDown)
                TargetY = monsterTrans.position.y;
        }
        animator.SetFloat("TargetX", TargetX);
        animator.SetFloat("TargetY", TargetY);
    }
    /// <summary>
    /// 移动到目的地
    /// </summary>
    /// <param name="animator"></param>
    void MoveToTargrtPosition(Animator animator)
    {
        Vector2 targetPosition = new Vector2(animator.GetFloat("TargetX"), animator.GetFloat("TargetY"));
        if (Mathf.Abs(targetPosition.x - monsterTrans.position.x) > 1)
        {
            XMove = true;
            YMove = false;
            //monsterRig.MovePosition(new Vector2(targetPosition.x, 0));

            if (animator.GetFloat("TargetX") > monsterTrans.position.x)
                //monsterRig.velocity = new Vector2(walkSpeed, 0);
                monsterTrans.Translate(Vector3.right * walkSpeed * Time.smoothDeltaTime);
            else if (animator.GetFloat("TargetX") < monsterTrans.position.x)
                //monsterRig.velocity = new Vector2(-walkSpeed, 0);
                monsterTrans.Translate(Vector3.left * walkSpeed * Time.smoothDeltaTime);
        }
        else if (Mathf.Abs(animator.GetFloat("TargetY") - monsterTrans.position.y) > 1)
        {
            XMove = false;
            YMove = true;
            if (animator.GetFloat("TargetY") > monsterTrans.position.y)
                //monsterRig.velocity = new Vector2(walkSpeed, 0);
            monsterTrans.Translate(Vector3.up * walkSpeed * Time.smoothDeltaTime);
            else if (animator.GetFloat("TargetY") < monsterTrans.position.y)
                //monsterRig.velocity = new Vector2(-walkSpeed, 0);
            monsterTrans.Translate(Vector3.down * walkSpeed * Time.smoothDeltaTime);
        }
        else
        {
            animator.SetBool("可行走", false);
        }
    }
}
