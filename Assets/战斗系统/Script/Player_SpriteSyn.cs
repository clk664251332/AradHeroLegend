using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_SpriteSyn : MonoBehaviour {

    public AvatarDataBase database;
    //影子
    //public Transform ShadowTrans;
    //tk2dSprite[] ShadowSprites;

    tk2dSpriteCollection playerCollection;
    tk2dSpriteCollection weaponCollection;

    tk2dSpriteAnimator Animator;

    tk2dSprite bodySprite;
    public tk2dSprite weaponSprite;

    private void Start()
    {
        Animator = GetComponent<tk2dSpriteAnimator>();
        Animator.ChangeSprite = false;
        bodySprite = GetComponent<tk2dSprite>();
        DefaultSkin();
        //ShadowSprites= ShadowTrans.GetComponentsInChildren<tk2dSprite>();
    }

    public void DefaultSkin()
    {
        playerCollection = null;
        playerCollection = database.GetCollectionByName("默认皮肤");
        if (playerCollection == null)
            Debug.LogWarning("加载图集失败！");

        weaponCollection = null;
        weaponCollection = database.GetCollectionByName("默认武器");
        if (weaponCollection == null)
            Debug.LogWarning("加载图集失败！");
    }

    public void SuperSkin()
    {
        SelectBody1();
        SelectWeapon1();
    }

    public void SelectBody1()
    {
        playerCollection = null;
        playerCollection = database.GetCollectionByName("皮肤1");
        //playerCollection = Resources.Load("Collections/人物1图集", typeof(tk2dSpriteCollection))as tk2dSpriteCollection;
        if (playerCollection == null)
           Debug.LogWarning("加载图集失败！");
    }
    public void SelectBody2()
    {
        playerCollection = null;
        playerCollection = database.GetCollectionByName("皮肤2");
        //playerCollection = Resources.Load("Collections/人物2图集", typeof(tk2dSpriteCollection)) as tk2dSpriteCollection;
        if (playerCollection == null)
            Debug.LogWarning("加载图集失败！");
    }
    public void SelectBody3()
    {
        playerCollection = null;
        playerCollection = database.GetCollectionByName("皮肤3");
        //playerCollection = Resources.Load("Collections/人物3图集", typeof(tk2dSpriteCollection)) as tk2dSpriteCollection;
        if (playerCollection == null)
            Debug.LogWarning("加载图集失败！");
    }


    public void SelectWeapon1()
    {
        weaponCollection = null;
        weaponCollection = database.GetCollectionByName("武器1");
        //weaponCollection = Resources.Load("Collections/武器1图集", typeof(tk2dSpriteCollection)) as tk2dSpriteCollection;
        if (weaponCollection == null)
            Debug.LogWarning("加载图集失败！");
    }
    public void SelectWeapon2()
    {
        weaponCollection = null;
        weaponCollection = database.GetCollectionByName("武器2");
        //weaponCollection = Resources.Load("Collections/武器2图集", typeof(tk2dSpriteCollection)) as tk2dSpriteCollection;
        if (weaponCollection == null)
            Debug.LogWarning("加载图集失败！");
    }
    public void SelectWeapon3()
    {
        weaponCollection = null;
        weaponCollection = database.GetCollectionByName("武器3");
        //weaponCollection = Resources.Load("Collections/武器3图集", typeof(tk2dSpriteCollection)) as tk2dSpriteCollection;
        if (weaponCollection == null)
            Debug.LogWarning("加载图集失败！");
    }


    private void Update()
    {
        if (weaponCollection == null)
        {
            Debug.LogWarning("皮肤图集为空");
            return;
        }
          
        if (playerCollection == null)
        {
            Debug.LogWarning("武器图集为空");
            return;
        }
         
        bodySprite.SetSprite(playerCollection.spriteCollection, Animator.FrameId);
        weaponSprite.SetSprite(weaponCollection.spriteCollection, Animator.FrameId);
        //同步影子
        //ShadowSprites[0].SetSprite(playerCollection.spriteCollection, Animator.FrameId);
        //ShadowSprites[1].SetSprite(playerCollection.spriteCollection, Animator.FrameId);

        //ShadowSprites[0].FlipX = bodySprite.FlipX;
        //ShadowSprites[1].FlipX = weaponSprite.FlipX;
    }
}
