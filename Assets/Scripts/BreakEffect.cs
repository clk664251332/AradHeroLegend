using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 破碎效果
/// </summary>
public class BreakEffect : MonoBehaviour {

    public Sprite[] sprites;
    public float scale;
    public float rotateSpeed;
    SpriteRenderer spriteRenderer;

    float speedX;
    float speedY;
    float gravity;
    //掉落点Y坐标
    float destinationY;
    //方向
    int direction;
    //
    Vector2 startPosition;
    float currentYSpeed;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }
    private void Start()
    {
        spriteRenderer.sprite = sprites[Random.Range(0, 4)];
        transform.localScale = new Vector2(scale, scale);
        startPosition = transform.position;

        speedX = Random.Range(-2, 3);
        speedY = Random.Range(8, 12);//10
        gravity = Random.Range(23, 28);//25
        destinationY = transform.position.y + Random.Range(-20, 21);
        StartCoroutine("Fly");
    }

    public void SetDirection(int _direction)
    {
        direction = _direction;
    }

   IEnumerator Fly()
    {
        currentYSpeed = speedY;
        bool isFly = true;
        float rotation = 0;
        while (isFly)
        {
            currentYSpeed -= gravity * Time.deltaTime;
            transform.Translate(new Vector2(speedX, currentYSpeed) * Time.deltaTime * 60);

            rotation += rotateSpeed * Time.deltaTime;
            if (speedX < 0)
                spriteRenderer.transform.rotation = Quaternion.Euler(0, 0, rotation);
            else
                spriteRenderer.transform.rotation = Quaternion.Euler(0, 0, -rotation);

            if (currentYSpeed < 0)
            {
                if (transform.position.y < destinationY)
                {
                    transform.position = new Vector2(transform.position.x, destinationY);
                    isFly = false;
                    speedX = 0;
                }
            }
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
}
