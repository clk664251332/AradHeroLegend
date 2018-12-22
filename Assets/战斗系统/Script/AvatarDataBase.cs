using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AvatarType
{
    public string typeName;
    public List<AvatarInfo> avatars;
}

[Serializable]
public class AvatarInfo
{
    /// <summary>
    /// 装备名字
    /// </summary>
    public string name;
    /// <summary>
    /// 该装备对应的图集
    /// </summary>
    public tk2dSpriteCollection collection;
}

[CreateAssetMenu]
public class AvatarDataBase : ScriptableObject {
    //public AvatarInfo[] avatars;
    
    public List<AvatarType> avatartypes;

    public tk2dSpriteCollection GetCollectionByName(string name)
    {
        foreach (var temptype in avatartypes)
        {

            foreach (var temp in temptype.avatars)
            {
                if (temp.name == name)
                {
                    return temp.collection;
                }
            }     
        }
        Debug.Log("未找到该图集");
        return null;
    }
}

