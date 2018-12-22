using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    #region 单例
    private static Player m_Instance;
    public static Player Instance
    {
        get
        {
            return m_Instance;
        }
    }
    #endregion
    Rigidbody2D rig;
    Animator anim;
    bool facingRight= true;
    bool isDead = false;
    //行走
    public float walkSpeed = 10;
    float currentSpeed;
    //跳跃
    Vector2 groundPos;
    public float jumpSpeed = 7.0f;
    float currentJumpSpeed;
    public float gravity;
    bool isGround = true;

    //进入场景的时候检查是否该场景已经有，如果有了就删除自己（只适用于第一个场景）
    //换新场景时候角色是不会再次调用Awake的，返回第一个场景会
    private void Awake()
    {
        if (m_Instance == null)
        {
            m_Instance = this;
            DontDestroyOnLoad(transform.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        groundPos.y = transform.position.y;
        currentSpeed = walkSpeed;
    }

    void Update()
    {
        
        //按空格键，如果在地上，就记录起跳的Y坐标，并开启跳跃协程
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGround)
            {
                isGround = false;
                groundPos.y = transform.position.y;

                currentJumpSpeed = jumpSpeed;
                StartCoroutine("Jump");
            }
        }
        //攻击
        if (Input.GetKeyDown(KeyCode.X))
        {
            anim.SetTrigger("Attack");
        }

        anim.SetBool("OnGround", isGround);
        anim.SetBool("Dead", isDead);

    }

    private void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        rig.velocity = new Vector2(h * currentSpeed, v * currentSpeed);

        if (h > 0 && !facingRight)
            Flip();
        else if (h < 0 && facingRight)
            Flip();

        anim.SetFloat("Speed", Mathf.Abs(rig.velocity.magnitude));
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector2 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    IEnumerator Jump()
    {
        while (true)
        {

            currentJumpSpeed -= gravity;

            transform.Translate(Vector3.up * currentJumpSpeed * Time.smoothDeltaTime);
            if (transform.position.y < groundPos.y)
            {
                transform.position = new Vector2(transform.position.x, groundPos.y);
                isGround = true;
                StopAllCoroutines();
            }
                yield return new WaitForEndOfFrame();
        }
    }

    void ZeroSpeed()
    {
        currentSpeed = 0;
    }

    void ResetSpeed()
    {
        currentSpeed = walkSpeed;
        currentSpeed = walkSpeed;
    }


}
