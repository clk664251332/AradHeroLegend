using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MonsterAvatarType
{
    public string MonsterName;
    public AvatarInfo[] avatars;
}

[CreateAssetMenu]
public class MonsterAvatarDataBase : ScriptableObject
{
    public List<MonsterAvatarType> MonsterAvatar;

    public tk2dSpriteCollection GetCollectionByName(string MonsterName, string CollectionName)
    {
        foreach (var temptype in MonsterAvatar)
        {
            if (temptype.MonsterName == MonsterName)
            {
                foreach (var info in temptype.avatars)
                {
                    if (info.name == CollectionName)
                        return info.collection;
                }
            }
        }
        //Debug.Log("未找到该图集");
        return null;
    }
}
