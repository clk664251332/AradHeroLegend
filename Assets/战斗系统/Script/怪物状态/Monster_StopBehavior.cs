using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_StopBehavior : StateMachineBehaviour {

    Transform monsterTrans;
    MonsterStateInfo monsterInfo;
    MonsterController monsterControl;
    float Timer;
    float MoveDelta;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        monsterTrans = animator.transform.parent;
        monsterInfo = monsterTrans.GetComponent<MonsterStateInfo>();
        monsterControl = monsterTrans.GetComponent<MonsterController>();
        monsterControl.rig.velocity = Vector2.zero;
        animator.transform.GetComponent<tk2dSpriteAnimator>().Play("停止");
        animator.SetBool("可行走", false);
        Timer = 0;
        MoveDelta = Random.Range(monsterInfo.WalkTimeDelta-0.5f, monsterInfo.WalkTimeDelta+0.5f);
    }

	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Timer += Time.deltaTime;
        if(Timer>=MoveDelta)
        {
            if (Mathf.Abs(PlayerContorller.Instance.transform.position.x - monsterTrans.position.x) <= 400)
                MoveTo(animator,PlayerContorller.Instance.transform.position.x + Random.Range(-200, 201),
                    PlayerContorller.Instance.transform.position.y + Random.Range(-20, 21));
            else
                MoveTo(animator,PlayerContorller.Instance.transform.position.x + Random.Range(-100, 101),
                    PlayerContorller.Instance.transform.position.y + Random.Range(-20, 21));
        }
    }

	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Timer = 0;
    }


    void MoveTo(Animator animator,float x,float y)
    {
        animator.SetFloat("TargetX", x);
        animator.SetFloat("TargetY", y);
        animator.SetBool("可行走", true);
    }
}
