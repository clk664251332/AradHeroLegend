using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateInfo : MonoSingleton<PlayerStateInfo> {
    public LevelUpDatabase levelUpDatabase;
    //角色属性
    public Value<int> HP = new Value<int>(30);//当前生命值
    public Value<int> MP = new Value<int>(30);//当前魔法值
    public Value<int> CurrentExp=new Value<int>(0);//经验值
    public Value<int> NeedExp = new Value<int>(0);//升级需要经验值
    public Value<int> Level = new Value<int>(1);//等级
    public Value<int> Gold = new Value<int>(1000);//金币

    public Value<int> MaxHp = new Value<int>(100);//最大生命值
    public Value<int> MaxMp = new Value<int>(100);//最大魔法值
    public Value<int> Strength = new Value<int>(6);     //力量
    public Value<int> Endurance = new Value<int>(7);    //体力
    public Value<int> Intelligence = new Value<int>(6); //智力
    public Value<int> Spirit = new Value<int>(4);       //精神
    public Value<int> PhysicalAttack = new Value<int>(20);//物理攻击
    public Value<int> PhysicalDefence = new Value<int>(50);//物理防御
    public Value<int> MagicAttack = new Value<int>(20);//魔法攻击
    public Value<int> MagicDefence = new Value<int>(10);//魔法防御
    public Value<int> CriticalChance = new Value<int>(5);//暴击几率
    //保存通过装备增加的属性值
    int Added_Strength = 0;
    int Added_Endurance = 0;
    int Added_Intelligence = 0;
    int Added_Spirit = 0;
    int Added_PhysicalAttack = 0;
    int Added_PhysicalDefence = 0;
    int Added_MagicAttack = 0;
    int Added_MagicDefence = 0;

    List<Slot> Slots;
    private void Awake()
    {
        NeedExp.Set(levelUpDatabase.LevelUpSet[Level.Get()]);
        Level.AddChangeListener(OnLevelUpdate);
        Strength.AddChangeListener(OnStrengthUpdate);
        Endurance.AddChangeListener(OnEnduranceUpdate);
        Intelligence.AddChangeListener(OnIntelligenceUpdate);
        Spirit.AddChangeListener(OnSpiritUpdate);
    }
    private void Start()
    {
        Slots = InventoryController.Instance.EquipmentContainer.Slots;
        for(int i = 0; i < Slots.Count; i++)
        {
            Slots[i].ItemHolder.Updated.AddListener(OnEquipmentUpdate);
        }
        HP.SetFilter(HpFilter);
        MP.SetFilter(MpFilter);
        Gold.SetFilter(GoldFilter);
    }
    //限制HP MP的最大值
    int HpFilter(int lastValue, int newValue)
    {
        if (newValue > MaxHp.Get())
        {
            //Debug.Log("生命值已经达到最大");+
            return MaxHp.Get();
        }
        else
            return newValue;
    }
    int MpFilter(int lastValue, int newValue)
    {
        if (newValue > MaxHp.Get())
        {
            //Debug.Log("魔法值已经达到最大");
            return MaxMp.Get();
        }
        else
            return newValue;
    }
    //限制金币的最小值
    int GoldFilter(int lastValue, int newValue)
    {
        if (newValue < 0)
            return 0;
        else
            return newValue;
    }
    void OnLevelUpdate()
    {
        Strength.SetBaseValue(Strength.m_initialValue + Level.Get());
        Endurance.SetBaseValue(Endurance.m_initialValue + Level.Get());
        Intelligence.SetBaseValue(Intelligence.m_initialValue + Level.Get());
        Spirit.SetBaseValue(Spirit.m_initialValue + Level.Get());
    }
    void OnStrengthUpdate()
    {
        PhysicalAttack.SetBaseValue((int)(PhysicalAttack.m_initialValue * (1 + (float)Math.Round((double)Strength.GetBaseValue() / 150, 2))));
        PhysicalAttack.SetTempValue((int)(PhysicalAttack.GetBaseValue() * (1 + (float)Math.Round((double)Strength.Get() / 150, 2))));

        PhysicalAttack.Set((PhysicalAttack.GetTempValue() + Added_PhysicalAttack));
    }
    void OnEnduranceUpdate()
    {
        PhysicalDefence.SetBaseValue((int)(PhysicalDefence.m_initialValue * (1 + (float)Math.Round((double)Endurance.GetBaseValue() / 150, 2))));
        PhysicalDefence.SetTempValue((int)(PhysicalDefence.GetBaseValue() * (1 + (float)Math.Round((double)Endurance.Get() / 150, 2))));

        PhysicalDefence.Set((PhysicalDefence.GetTempValue()) + Added_PhysicalDefence);
    }
    void OnIntelligenceUpdate()
    {
        MagicAttack.SetBaseValue((int)(MagicAttack.m_initialValue * (1 + (float)Math.Round((double)Intelligence.GetBaseValue() / 150, 2))));
        MagicAttack.SetTempValue((int)(MagicAttack.GetBaseValue() * (1 + (float)Math.Round((double)Intelligence.Get() / 150, 2))));

        MagicAttack.Set((MagicAttack.GetTempValue()) + Added_MagicAttack);
    }
    void OnSpiritUpdate()
    {
        MagicDefence.SetBaseValue((int)(MagicDefence.m_initialValue * (1 + (float)Math.Round((double)Spirit.GetBaseValue() / 150, 2))));
        MagicDefence.SetTempValue((int)(MagicDefence.GetBaseValue() * (1 + (float)Math.Round((double)Spirit.Get() / 150, 2))));

        MagicDefence.Set((MagicDefence.GetTempValue()) + Added_MagicDefence);
    }
    //更新装备时
    void OnEquipmentUpdate(ItemHolder itemholder)
    {
        //这是无装备的状态下的属性
        //每个属性都有自己的初始值，通过装备提升之后的是面板值
        //面板值可以增加或者减少，但是初始值是通过等级之后固定下来的，不可减少
        //而攻击力的面板值是根据属性的面板值来定的
        //攻击力的基础值同样也是根据属性的基础值来定的
        Added_Strength = 0;
        Added_Endurance = 0;
        Added_Intelligence = 0;
        Added_Spirit = 0;
        Added_PhysicalAttack = 0;
        Added_PhysicalDefence = 0;
        Added_MagicAttack = 0;
        Added_MagicDefence = 0;
        //遍历所有装备格子，显示面板属性
        for (int i = 0; i < Slots.Count; i++)
        {
            //所有通过装备增加的属性
            Added_Strength += Slots[i].Get_StrengthValue();
            Added_Endurance += Slots[i].Get_EnduranceValue();
            Added_Intelligence += Slots[i].Get_IntelligenceValue();
            Added_Spirit += Slots[i].Get_SpiritValue();
            Added_PhysicalAttack += Slots[i].Get_PhysicalAttackValue();
            Added_PhysicalDefence += Slots[i].Get_PhysicalDefenceValue();
            Added_MagicAttack += Slots[i].Get_MagicAttackValue();
            Added_MagicDefence += Slots[i].Get_MagicDefenceValue();
        }
        //基础属性，其基础值是根据等级得来的
        Strength.Set(Strength.GetBaseValue() + Added_Strength);
        Endurance.Set(Endurance.GetBaseValue() + Added_Endurance);
        Intelligence.Set(Intelligence.GetBaseValue() + Added_Intelligence);
        Spirit.Set(Spirit.GetBaseValue() + Added_Spirit);
        //战斗直接属性,根据基础属性和装备加成得来的（基础值(BaseValue)，面板属性(TempValue)，装备加成）
        OnStrengthUpdate();
        OnEnduranceUpdate();
        OnIntelligenceUpdate();
        OnSpiritUpdate();
    }
    //增加经验值
    public void AddExp(int value)
    {
        CurrentExp.Set(CurrentExp.Get() + value);

        if (CurrentExp.Get() >= NeedExp.Get())
        {
            int OutExp = CurrentExp.Get() - NeedExp.Get();
            LevelUp(OutExp);
        }
    }

    //升级
    void LevelUp(int outExp)
    {
        if (Level.Get() + 1 >= levelUpDatabase.LevelUpSet.Length)
            return;
        Level.Set(Level.Get() + 1);
        CurrentExp.Set(0);
        NeedExp.Set(levelUpDatabase.LevelUpSet[Level.Get()]);

        AddExp(outExp);
        //升级特效
        PlayerContorller.Instance.ShowLevelUpEffect();
    }
}
