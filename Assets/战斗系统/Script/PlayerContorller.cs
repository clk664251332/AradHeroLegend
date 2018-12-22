using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//进行一些复杂的功能，如双击检测，跳跃的位移
public class PlayerContorller : MonoBehaviour {

    public static PlayerContorller m_Instance;
    public static PlayerContorller Instance { get { return m_Instance; } }

    [HideInInspector]
    public Rigidbody2D rig;
    Animator anim;
    tk2dSprite[] sprites;

    public Transform bodyAnimTrans;
    public Transform effectTrans;
    public Transform commonEffectTrans;

    MeshRenderer effectRenderer;
    tk2dSpriteAnimator effectAnimator;
    MeshRenderer commonEffectRenderer;
    tk2dSpriteAnimator commonEffectAnimator;
    //跳跃
    public float jumpSpeed = 2f;
    [HideInInspector]
    public float currentJumpSpeed;
    public float gravity;

    tk2dSpriteAnimator bodyAnimator;
    MovementBehavior baseMove;

    [HideInInspector]
    public bool canMove=true;
    private void Awake()
    {
        if (m_Instance == null)
            m_Instance = this;
        Application.targetFrameRate = 100;
        canMove = true;
    }
    void Start () {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        sprites= bodyAnimTrans.GetComponentsInChildren<tk2dSprite>();
        //挨打特效动画
        effectRenderer = effectTrans.GetComponent<MeshRenderer>();
        effectAnimator = effectTrans.GetComponent<tk2dSpriteAnimator>();
        effectAnimator.AnimationCompleted = OnEffectAnimationCompleted;
        effectRenderer.enabled = false;
        //通用特效动画
        commonEffectRenderer = commonEffectTrans.GetComponent<MeshRenderer>();
        commonEffectAnimator = commonEffectTrans.GetComponent<tk2dSpriteAnimator>();
        commonEffectAnimator.AnimationCompleted = OnCommonEffectAnimationCompleted;
        commonEffectRenderer.enabled = false;
        //身体动画
        bodyAnimator = bodyAnimTrans.GetComponent<tk2dSpriteAnimator>();
        bodyAnimator.AnimationCompleted = OnAnimationCompleted;
        baseMove = anim.GetBehaviour<MovementBehavior>();
    }

    void Update ()
    {
        DoubleClickCheck();
    }

    void OnAnimationCompleted(tk2dSpriteAnimator caller, tk2dSpriteAnimationClip currentClip)
    {
        if(currentClip.name=="小砍2"|| currentClip.name == "小砍3")
        {
            rig.velocity = Vector2.zero;
        }
    }
    void OnEffectAnimationCompleted(tk2dSpriteAnimator caller, tk2dSpriteAnimationClip currentClip)
    {
        effectRenderer.enabled = false; ;
    }
    void OnCommonEffectAnimationCompleted(tk2dSpriteAnimator caller, tk2dSpriteAnimationClip currentClip)
    {
        commonEffectRenderer.enabled = false; ;
    }

    /// <summary>
    /// 双击检测函数
    /// </summary>
    void DoubleClickCheck()
    {
        //按键抬起检测（用于双击）
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            StopCoroutine("StartTimer");
            StartCoroutine(StartTimer(KeyCode.RightArrow, 0.1f));
        }
        else if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            StopCoroutine("StartTimer");
            StartCoroutine(StartTimer(KeyCode.LeftArrow, 0.1f));
        }
    }

    /// <summary>
    /// 抬起按键计时
    /// </summary>
    /// <param name="key"></param>
    /// <param name="time"></param>
    /// <returns></returns>
    IEnumerator StartTimer(KeyCode key, float time)
    {
        //Debug.Log("开始协程");
        for (float i = time; i > 0; i -= Time.deltaTime)
        {
            if (Input.GetKeyDown(key))
            {
                baseMove.currentSpeed = baseMove.RunSpeed;
            }
            yield return null;
        }
    }

    /// <summary>
    /// 跳跃函数
    /// </summary>
    /// <returns></returns>
    public IEnumerator Jump()
    {
        currentJumpSpeed = jumpSpeed;
        bool isJump = true;
        while (isJump)
        {
            anim.SetFloat("JumpSpeed", currentJumpSpeed);
            currentJumpSpeed -= gravity * Time.deltaTime;
            bodyAnimTrans.Translate(Vector3.up * currentJumpSpeed * Time.deltaTime * 60);
            if (bodyAnimTrans.position.y < transform.position.y)
            {
                bodyAnimTrans.position = new Vector2(bodyAnimTrans.position.x, transform.position.y);
                currentJumpSpeed = 0;
                anim.SetBool("在地面", true);
                isJump = false;
            }
            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }
    /// <summary>
    /// 通过更改速度的位移
    /// </summary>
    public void XMove(float speed)
    {
        //bool facingRight = transform.localScale.x == 1 ? true : false;
        bool facingRight = !anim.GetBehaviour<MovementBehavior>().facingLeft;
        if (facingRight)
            rig.velocity = new Vector2(speed, 0);
        else
            rig.velocity = new Vector2(speed * -1, 0);
    }

    /// <summary>
    /// X方向计时位移
    /// </summary>
    /// <returns></returns>
    public IEnumerator XTimeMove(float speed,float time)
    {
        float timer = 0;
        while (timer <= time)
        {
            timer += Time.smoothDeltaTime;
            bool facingRight = transform.localScale.x == 1 ? true : false;

            if (facingRight)
                //transform.Translate(Vector2.right * speed * Time.smoothDeltaTime);
                rig.velocity = new Vector2(speed, 0);
            else
                //transform.Translate(Vector2.left * speed * Time.smoothDeltaTime);
                rig.velocity = new Vector2(speed * -1, 0);

            yield return new WaitForEndOfFrame();
        }
       // StopAllCoroutines();
    }
    /// <summary>
    /// 通过摩擦力来移动
    /// </summary>
    /// <param name="speed"></param>
    /// <param name="fiction"></param>
    /// <returns></returns>
    public IEnumerator XPhysicsMove(float speed, float fiction)
    {
        while (speed-fiction > 0)
        {
            speed -= fiction;
            bool facingRight = transform.localScale.x == 1 ? true : false;

            if (facingRight)
                //transform.Translate(Vector2.right * speed * Time.smoothDeltaTime);
                rig.velocity = new Vector2(speed, 0);
            else
                //transform.Translate(Vector2.left * speed * Time.smoothDeltaTime);
                rig.velocity = new Vector2(speed * -1, 0);

            yield return new WaitForEndOfFrame();
        }
    }

    public IEnumerator GetHitMove(float speed, float time)
    {
        anim.SetTrigger("被击中");
        float timer = 0;
        while (timer <= time)
        {
            timer += Time.smoothDeltaTime;
            rig.velocity = new Vector2(speed, 0);
            yield return new WaitForEndOfFrame();
        }
        StopCoroutine("GetHitMove");
    }

    public void TakeDamage(int PhysicalAttack, int MagicAttack )
    {
        int BasicDamage = (int)((PhysicalAttack * 3 - PlayerStateInfo.Instance.PhysicalDefence.Get() * 2) * Random.Range(0.9f, 1.1f));
        int Damage = (int)(BasicDamage * Random.Range(1.1f, 1.4f));
        if (Damage <= 0)
        {
            Damage= Random.Range(1, 6);
        }
        FloatingTextController.Instance.CreatBeHitDamageText(Damage.ToString(), new Vector2(transform.position.x, transform.position.y + 100));
        PlayerStateInfo.Instance.HP.Set(PlayerStateInfo.Instance.HP.Get() - Damage);
    }

    public void ShowHitEffect(string action)
    {
        if (!effectRenderer.enabled)
            effectRenderer.enabled = true;
        if (action == "击打")
            effectAnimator.Play("knock");
    }

    public void ShowHpEffect()
    {
        if (!commonEffectRenderer.enabled)
            commonEffectRenderer.enabled = true;
        commonEffectAnimator.Play("加血");
    }
    public void ShowMpEffect()
    {
        if (!commonEffectRenderer.enabled)
            commonEffectRenderer.enabled = true;
        commonEffectAnimator.Play("加蓝");
    }
    public void ShowLevelUpEffect()
    {
        if (!commonEffectRenderer.enabled)
            commonEffectRenderer.enabled = true;
        commonEffectAnimator.Play("升级");
    }
    /// <summary>
    /// 设置角色的朝向向左
    /// </summary>
    public void SetFacingLeft()
    {
        for (int i = 0; i < sprites.Length; i++)
        {
            sprites[i].FlipX = true;
        }
        anim.GetBehaviour<MovementBehavior>().facingLeft = true;
    }
    /// <summary>
    /// 设置角色的朝向向右
    /// </summary>
    public void SetFacingRight()
    {
        for (int i = 0; i < sprites.Length; i++)
        {
            sprites[i].FlipX = false;
        }
        anim.GetBehaviour<MovementBehavior>().facingLeft = false;
    }

    /// <summary>
    /// 获得角色的朝向
    /// </summary>
    /// <returns></returns>
    public bool IsFacingRight()
    {
        return !anim.GetBehaviour<MovementBehavior>().facingLeft;
    }
}
