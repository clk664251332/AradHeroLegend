using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_FlyGroundBehavior : StateMachineBehaviour {

    Transform monsterTrans;
    MonsterStateInfo monsterInfo;
    MonsterController monsterControl;
    bool die;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        die = false;
        monsterTrans = animator.transform.parent;
        monsterControl = monsterTrans.GetComponent<MonsterController>();
        monsterInfo = monsterTrans.GetComponent<MonsterStateInfo>();
        
        animator.SetBool("在躺着", true);
        animator.SetBool("在空中", false);

        if (monsterInfo.CurrentHp.Get() <= 0)
        {
            //die = true;
            //monsterInfo.IsDie = true;
            //monsterControl.ShowBreakEffect();
            //monsterControl.ShowWhiteEffect();
            animator.SetBool("死亡", true);
            monsterControl.Die();
        }
        animator.transform.GetComponent<tk2dSpriteAnimator>().Play("躺下");
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("在躺着", false);
    }
}
