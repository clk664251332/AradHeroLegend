using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody2D rig;

    public float speed;//飞行速度
    public int distance;//飞行距离
    //由调用时赋值
    private int direction;//飞行方向
    private int physicalAttack;//物理攻击
    private int magicAttack;//魔法攻击
    //
    private Vector2 startPosition;

    private void Awake()
    {
        rig = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        rig.velocity = new Vector2(speed * direction, 0);
        
        startPosition = transform.position;
    }
    public void Initial(int _physicalAttack, int _magicAttack,int _direction)
    {
        physicalAttack = _physicalAttack;
        magicAttack = _magicAttack;
        direction = _direction;
    }

    private void Update()
    {
        if (Mathf.Abs(startPosition.x - transform.position.x) >= distance)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerBody"))
        {
            if (Mathf.Abs(transform.position.y - PlayerContorller.Instance.transform.position.y) <= 20)
            {
                //怪物在角色右边，给向左的击打，速度方向为负
                if (transform.position.x >= PlayerContorller.Instance.transform.position.x)
                {
                    PlayerContorller.Instance.StartCoroutine(PlayerContorller.Instance.GetHitMove(-50, 0.3f));
                    PlayerContorller.Instance.ShowHitEffect("击打");
                }
                else
                {
                    PlayerContorller.Instance.StartCoroutine(PlayerContorller.Instance.GetHitMove(50, 0.3f));
                    PlayerContorller.Instance.ShowHitEffect("击打");
                }
                PlayerContorller.Instance.TakeDamage(physicalAttack, magicAttack);
                Destroy(gameObject);
            }
          
        }
    }
}
