using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_GetHitBehavior : StateMachineBehaviour {

    Transform monsterTrans;
    MonsterStateInfo monsterInfo;
    float StiffTime;
    float Timer;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        monsterTrans = animator.transform.parent;
        monsterInfo = monsterTrans.GetComponent<MonsterStateInfo>();
        Timer = 0;
        animator.SetBool("僵直", true);
        int r = Random.Range(1, 3);
        animator.transform.GetComponent<tk2dSpriteAnimator>().Play("被击中"+r.ToString());
        StiffTime = monsterInfo.HitStiffTime;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Timer += Time.deltaTime;
        if (Timer >= StiffTime)
        {
            Timer = 0;
            animator.SetBool("僵直", false);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("僵直", false);
    }
}
