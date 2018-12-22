using UnityEngine;
using System.Collections;

public class BackGroundController : MonoBehaviour
{
    public static BackGroundController instance = null;

    public Renderer Background;
    public float speedBG;
    public Renderer Midground;
    public float speedMG;
    [HideInInspector]
    public Renderer Forceground;
    [HideInInspector]
    public float speedFG;
    public Texture TestTexture;

    [Tooltip("if this target == null, the target will be Main Camera")]
    public Transform target;
    float startPosX;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(transform.gameObject);
        }
        else
            Destroy(gameObject);
    }

    // Use this for initialization
    void Start()
    {
        if (target == null)
            target = Camera.main.transform;

        startPosX = target.position.x;
    }

    // Update is called once per frame
    //设置渲染-材质-纹理坐标偏移
    //x为 主摄像机当前位置 与 主摄像机初始位置 的距离，这个距离随着摄像机的移动而增大减小
    //
    void Update()
    {
        var x = target.position.x - startPosX;
        //我们对x的值进行不同程度的缩小（乘以一个小数），前景移动的最快，背景移动的最慢
        //取模运算，用处为使offset在0和1之间不停循环往复
        if (Background != null)
        {
            var offset = (x * speedBG) % 1;
            Background.material.mainTextureOffset = new Vector2(offset, Background.material.mainTextureOffset.y);
        }
        if (Midground != null)
        {
            var offset = (x * speedMG) % 1;
            Midground.material.mainTextureOffset = new Vector2(offset, Midground.material.mainTextureOffset.y);
        }
        if (Forceground != null)
        {
            var offset = (x * speedFG) % 1;
            Forceground.material.mainTextureOffset = new Vector2(offset, Forceground.material.mainTextureOffset.y);
        }
    }

    public void ChangeGround(Texture mid,Texture back)
    {
        Midground.material.mainTexture = mid;
        Background.material.mainTexture = back;
    }
}
