using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 属性的类型
/// </summary>
public enum BasePropertyType
{
    物理攻击,
    物理防御,
    魔法攻击,
    魔法防御,

    生命值,
    魔法值,
    最大生命值,
    最大魔法值,
    经验   
}

public enum AdditionalPropertyType
{
    力量, 体力, 智力,精神
}

public enum SpecialPropertyType
{
    移动速度,攻击速度,冷却缩减
}

[Serializable]
public class ItemBaseProperty  {
    
    public BasePropertyType BaseProperty { get { return m_BaseType; } }
    
    [SerializeField]
    private BasePropertyType m_BaseType;
    public int Value;

}

[Serializable]
public class ItemAdditionalProperty
{
    public AdditionalPropertyType AdditionalProperty { get { return m_AdditionalType; } }

    [SerializeField]
    private AdditionalPropertyType m_AdditionalType;
    public int Value;
}

[Serializable]
public class ItemSpecialProperty
{
    public SpecialPropertyType SpecialProperty { get { return m_SpecialType; } }

    [SerializeField]
    private SpecialPropertyType m_SpecialType;
    public int Value;
}