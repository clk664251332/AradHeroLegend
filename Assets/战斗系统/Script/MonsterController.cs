using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour {

    Animator anim;
    [HideInInspector]
    public MonsterStateInfo monsterInfo;
    public Transform bodyAnimTrans;
    public Transform effectTrans;
    //public Transform animTrans;
    MeshRenderer effectRenderer;
    tk2dSpriteAnimator effectAnimator;
    tk2dSpriteAnimator bodyAnimator;
    [HideInInspector]
    public Rigidbody2D rig;
    //public float FlySpeed;
    //public float FlyXSpeed;
    public float gravity;
    public float currentFlySpeed;
    public float AttackTimeDelta;   
    tk2dSprite bodySprite;
    public GameObject Break;
    tk2dSprite[] sprites;
    public GameObject BossDieFlash;
    //private void OnDrawGizmos()
    //{
    //    anim = GetComponentInChildren<Animator>();
    //   Gizmos.DrawWireSphere(new Vector2(anim.GetFloat("TargetX"), anim.GetFloat("TargetY")),5f);
    //}

    void Start () {
        anim = bodyAnimTrans.GetComponent<Animator>();
        rig = transform.GetComponent<Rigidbody2D>();
        sprites= bodyAnimTrans.GetComponentsInChildren<tk2dSprite>();


        effectRenderer = effectTrans.GetComponent<MeshRenderer>();
        effectAnimator= effectTrans.GetComponent<tk2dSpriteAnimator>();
        effectAnimator.AnimationCompleted = OnEffectAnimationCompleted;
        effectRenderer.enabled = false;

        bodyAnimator = bodyAnimTrans.GetComponent<tk2dSpriteAnimator>();
        bodyAnimator.AnimationCompleted = OnBodyAnimationCompleted;

        monsterInfo = GetComponent<MonsterStateInfo>();
        var mapInitial = FindObjectOfType<DungeonMapInitial>();
        if (mapInitial != null)
            FindObjectOfType<DungeonMapInitial>().AddMonster(this);
    }
	

    //public void TakeDamage(string action)
    //{
    //    ShowHitEffect(action);
    //    anim.SetTrigger("被击中");
    //    if (action == "小砍1")
    //    {
    //        if (!anim.GetBool("在躺着"))
    //            StartCoroutine(XTimeMove(50, 0.17f));
    //        //XMove(50);
    //    }
    //    else if (action == "小砍2")
    //    {
    //        if (!anim.GetBool("在躺着"))
    //            StartCoroutine(XTimeMove(50, 0.17f));
    //        //XMove(50);
    //    }
    //    else if (action == "小砍3")
    //    {
    //        if (anim.GetBool("在躺着"))
    //            StartCoroutine(HitFly(500));
    //        else
    //            StartCoroutine(HitFly(600));
    //        StartCoroutine(XTimeMove(50, 0.4f));
    //    }
        
    //}

    public void ShowHitEffect(string action)
    {
        if (!effectRenderer.enabled)
            effectRenderer.enabled = true;
        if(action=="小砍1")
        {
            int r = UnityEngine.Random.Range(1, 4);
            effectAnimator.Play(r==3? "large_1" : "small_1");
        }
        else if (action == "小砍2")
        {
            int r = UnityEngine.Random.Range(1, 4);
            effectAnimator.Play(r == 3 ? "large_2" : "small_2");
        }
        else if (action == "小砍3")
        {
            int r = UnityEngine.Random.Range(1, 4);
            effectAnimator.Play(r == 3 ? "large_3" : "small_3");
        }
    }

    void OnEffectAnimationCompleted(tk2dSpriteAnimator caller, tk2dSpriteAnimationClip currentClip)
    {
        effectRenderer.enabled = false; ;
    }
    void OnBodyAnimationCompleted(tk2dSpriteAnimator caller, tk2dSpriteAnimationClip currentClip)
    {
        //if (currentClip.name == "被击中1" || currentClip.name == "被击中2")
        //{
        //    Debug.Log("被打结束");
        //    rig.velocity = Vector2.zero;
        //}
    }
    //击飞
    public IEnumerator HitFly(float YSpeed,float XSpeed)//,float bounceSpeed)
    {
        //处理横向移动
        bool Right = transform.position.x > PlayerContorller.Instance.transform.position.x ? true : false;

        if (Right)
            rig.velocity = new Vector2(XSpeed, 0);
        else
            rig.velocity = new Vector2(XSpeed * -1, 0);
        //处理垂直移动
        currentFlySpeed = YSpeed;
        anim.SetFloat("FlySpeed", currentFlySpeed);
        int count = 0;
        anim.SetTrigger("击飞");
        bool isFly = true;
        while (isFly)
        {
            if (count == 1)
                anim.SetFloat("FlySpeed", currentFlySpeed);
            currentFlySpeed -= gravity * Time.deltaTime;
            bodyAnimTrans.Translate(Vector3.up * currentFlySpeed * Time.deltaTime * 60);
            if (bodyAnimTrans.position.y < transform.position.y)
            {
                bodyAnimTrans.position= new Vector2(bodyAnimTrans.position.x, transform.position.y);
                //rig.velocity = Vector2.zero;
                count += 1;
                if (count == 1)//第一次落地
                {
                    currentFlySpeed = YSpeed / 3;
                }
                if (count == 2)//第二次落地
                {
                    currentFlySpeed = 0;
                    rig.velocity = Vector2.zero;
                    anim.SetTrigger("着地");
                    //结束击飞
                    //StopAllCoroutines();
                    isFly = false;
                    //yield return null;
                    //StopCoroutine("HitFly");
                }
            }
            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }
    //击退

    /// <summary>
    /// 通过更改速度的位移
    /// </summary>
    public void XMove(float speed)
    {
        bool facingRight = transform.localScale.x == 1 ? true : false;

        if (facingRight)
            rig.velocity = new Vector2(speed, 0);
        else
            rig.velocity = new Vector2(speed * -1, 0);
    }

    public IEnumerator XTimeMove(float speed, float time)
    {
        float timer = 0;
        while (timer <= time)//&&!anim.GetBool("在躺着"))
        {
            timer += Time.smoothDeltaTime;
            bool Right = transform.position.x > PlayerContorller.Instance.transform.position.x ? true : false;

            if (Right)
                //transform.Translate(Vector2.right * speed * Time.smoothDeltaTime);
                rig.velocity = new Vector2(speed, 0);
            else
                //transform.Translate(Vector2.left * speed * Time.smoothDeltaTime); rig.velocity = new Vector2(speed * -1, 0);
                rig.velocity = new Vector2(speed * -1, 0);

            yield return new WaitForEndOfFrame();
        }
        rig.velocity = Vector2.zero;
        //StopAllCoroutines();
        StopCoroutine("XTimeMove");
    }

    public IEnumerator XPhysicsMove(float speed, float fiction)
    {
        while (speed > 0)
        {
            speed -= fiction;
            bool Right = transform.position.x > PlayerContorller.Instance.transform.position.x ? true : false;

            if (Right)
                transform.Translate(Vector2.right * speed * Time.smoothDeltaTime);
            else
                transform.Translate(Vector2.left * speed * Time.smoothDeltaTime);

            yield return new WaitForEndOfFrame();
        }
    }

    public IEnumerator AttackTimer()
    {
        //anim.SetBool("可攻击", false);
        yield return new WaitForSeconds(AttackTimeDelta);
        anim.SetBool("可攻击", true);
        yield return null;
    }

    public void Die()
    {
        var mapInitial = FindObjectOfType<DungeonMapInitial>();
        if (mapInitial != null)
        {
            if (monsterInfo.isBoss)
            {
                //如果这个怪物是Boss，重置隐藏小地图，清除其他小怪物
                //显示慢动作和特效，显示返回城镇的选项
                MiniMapManager.Instance.HideMiniMapUI();
                mapInitial.ClrearAllMonsterWhenBossDie();
                Instantiate(BossDieFlash, transform.position, Quaternion.identity);
                DungeonManager.Instance.StartCoroutine(DungeonManager.Instance.MoveToTown());
            }
            mapInitial.RemoveMonster(this);
        }
        anim.SetBool("死亡", true);
        bodyAnimator.Stop();
        anim.speed = 0;
        ShowBreakEffect();
        ShowWhiteEffect();
        PlayerStateInfo.Instance.AddExp(monsterInfo.Exp);
        StartCoroutine("DestoryTimer");
    }
    /// <summary>
    /// 显示怪物死亡破碎效果
    /// </summary>
    private void ShowBreakEffect()
    {
        for (int i = 0; i < 12; i++)
        {
            Instantiate(Break, transform.position, Quaternion.identity);
        }
    }
    /// <summary>
    /// 显示怪物死亡时的白色效果
    /// </summary>
    private void ShowWhiteEffect()
    {
        for (int i = 0; i < sprites.Length; i++)
        {
            sprites[i].showWhite = true;
            sprites[i].ForceUpdateWhite();
        }
    }

    IEnumerator DestoryTimer()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
