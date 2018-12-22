using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monster_SpriteSyn : MonoBehaviour {
    public MonsterAvatarDataBase database;
    public string MonsterName;

    tk2dSpriteAnimator Animator;
    //图集
    tk2dSpriteCollection skinCollection;
    tk2dSpriteCollection weaponCollection;
    tk2dSpriteCollection decoration1Collection;
    tk2dSpriteCollection decoration2Collection;
    //位置
    Transform weaponTrans;
    Transform decoration1Trans;
    Transform decoration2Trans;
    //精灵
    tk2dSprite bodySprite;
    tk2dSprite weaponSprite;
    tk2dSprite decoration1Sprite;
    tk2dSprite decoration2Sprite;

    void Start ()
    {
        Animator = GetComponent<tk2dSpriteAnimator>();
        Animator.ChangeSprite = false;
        //图集
        skinCollection = database.GetCollectionByName(MonsterName, "皮肤");
        weaponCollection = database.GetCollectionByName(MonsterName, "武器");
        decoration1Collection = database.GetCollectionByName(MonsterName, "装饰1");
        decoration2Collection = database.GetCollectionByName(MonsterName, "装饰2");
        //位置
        weaponTrans = transform.Find("武器");
        decoration1Trans = transform.Find("装饰1");
        decoration2Trans = transform.Find("装饰2");
        //精灵
        bodySprite = GetComponent<tk2dSprite>();
        weaponSprite= weaponTrans.GetComponent<tk2dSprite>();
        decoration1Sprite = decoration1Trans.GetComponent<tk2dSprite>();
        decoration2Sprite = decoration2Trans.GetComponent<tk2dSprite>();
        //初始化各个部件的显示
        if (skinCollection == null)
            Debug.LogError("未找到皮肤图集");
        if (weaponCollection == null)
            weaponTrans.gameObject.SetActive(false);
        if (decoration1Collection == null)
            decoration1Trans.gameObject.SetActive(false);
        if (decoration2Collection == null)
            decoration2Trans.gameObject.SetActive(false);
    }

    private void Update()
    {
        //if (weaponCollection == null)
        //{
        //    Debug.LogWarning("武器图集为空");
        //    return;
        //}

        //if (skinCollection == null)
        //{
        //    Debug.LogWarning("皮肤图集为空");
        //    return;
        //}

        if (skinCollection != null)
            bodySprite.SetSprite(skinCollection.spriteCollection, Animator.FrameId);
        if (weaponCollection != null)
            weaponSprite.SetSprite(weaponCollection.spriteCollection, Animator.FrameId);
        if (decoration1Collection != null)
            decoration1Sprite.SetSprite(decoration1Collection.spriteCollection, Animator.FrameId);
        if (decoration2Collection != null)
            decoration2Sprite.SetSprite(decoration2Collection.spriteCollection, Animator.FrameId);
    }
}
