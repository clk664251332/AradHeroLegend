using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStateUI : MonoBehaviour {

    public Text Exp;
    public Text HP;
    public Text MP;
    public Text Strength;     //力量
    public Text Endurance;    //体力
    public Text Intelligence; //智力
    public Text Spirit;       //精神
    public Text PhysicalAttack;
    public Text PhysicalDefence;
    public Text MagicAttack;
    public Text MagicDefence;
    public Text Gold;
    //public Text MaxHp;
    //public Text MaxMp;

    void Start () {
        PlayerStateInfo.Instance.HP.AddChangeListener(onHPChange);
        PlayerStateInfo.Instance.MP.AddChangeListener(onMPChange);
        PlayerStateInfo.Instance.Strength.AddChangeListener(onStrengthChange);
        PlayerStateInfo.Instance.Endurance.AddChangeListener(onEnduranceChange);
        PlayerStateInfo.Instance.Intelligence.AddChangeListener(onIntelligenceChange);
        PlayerStateInfo.Instance.Spirit.AddChangeListener(onSpiritChange);
        PlayerStateInfo.Instance.PhysicalAttack.AddChangeListener(onPhysicalAttackChange);
        PlayerStateInfo.Instance.PhysicalDefence.AddChangeListener(onPhysicalDefenceChange);
        PlayerStateInfo.Instance.MagicAttack.AddChangeListener(onMagicAttackChange);
        PlayerStateInfo.Instance.MagicDefence.AddChangeListener(onMagicDefenceChange);
        PlayerStateInfo.Instance.MaxHp.AddChangeListener(onMaxHpChange);
        PlayerStateInfo.Instance.MaxMp.AddChangeListener(onMaxMpChange);
        PlayerStateInfo.Instance.Gold.AddChangeListener(onGoldChange);

        HP.text = string.Format("{0}/{1}", PlayerStateInfo.Instance.HP.Get().ToString(), PlayerStateInfo.Instance.MaxHp.Get().ToString());
        MP.text = string.Format("{0}/{1}", PlayerStateInfo.Instance.MP.Get().ToString(), PlayerStateInfo.Instance.MaxMp.Get().ToString());
        Strength.text = PlayerStateInfo.Instance.Strength.Get().ToString();
        Endurance.text = PlayerStateInfo.Instance.Endurance.Get().ToString();
        Intelligence.text = PlayerStateInfo.Instance.Intelligence.Get().ToString();
        Spirit.text = PlayerStateInfo.Instance.Spirit.Get().ToString();
        PhysicalAttack.text = PlayerStateInfo.Instance.PhysicalAttack.Get().ToString();
        PhysicalDefence.text = PlayerStateInfo.Instance.PhysicalDefence.Get().ToString();
        MagicAttack.text = PlayerStateInfo.Instance.MagicAttack.Get().ToString();
        MagicDefence.text = PlayerStateInfo.Instance.MagicDefence.Get().ToString();
        Gold.text = PlayerStateInfo.Instance.Gold.Get().ToString();
    }

    private void onHPChange()
    {
        HP.text = string.Format("{0}/{1}", PlayerStateInfo.Instance.HP.Get().ToString(), PlayerStateInfo.Instance.MaxHp.Get().ToString());
    }
    private void onMPChange()
    {
        MP.text = string.Format("{0}/{1}", PlayerStateInfo.Instance.MP.Get().ToString(), PlayerStateInfo.Instance.MaxMp.Get().ToString());
    }
    private void onStrengthChange()
    {
        Strength.text = PlayerStateInfo.Instance.Strength.Get().ToString();
    }
    private void onEnduranceChange()
    {
        Endurance.text = PlayerStateInfo.Instance.Endurance.Get().ToString();
    }
    private void onIntelligenceChange()
    {
        Intelligence.text = PlayerStateInfo.Instance.Intelligence.Get().ToString();
    }
    private void onSpiritChange()
    {
        Spirit.text = PlayerStateInfo.Instance.Spirit.Get().ToString();
    }
    private void onPhysicalAttackChange()
    {
        PhysicalAttack.text = PlayerStateInfo.Instance.PhysicalAttack.Get().ToString();
    }
    private void onPhysicalDefenceChange()
    {
        PhysicalDefence.text = PlayerStateInfo.Instance.PhysicalDefence.Get().ToString();
    }
    private void onMagicAttackChange()
    {
        MagicAttack.text = PlayerStateInfo.Instance.MagicAttack.Get().ToString();
    }
    private void onMagicDefenceChange()
    {
        MagicDefence.text = PlayerStateInfo.Instance.MagicDefence.Get().ToString();
    }
    private void onMaxHpChange()
    {

    }
    private void onMaxMpChange()
    {

    }
    private void onGoldChange()
    {
        if (Gold == null)
            Debug.Log(1);
        else if (PlayerStateInfo.Instance.Gold == null)
            Debug.Log(2);
        else
            Gold.text = PlayerStateInfo.Instance.Gold.Get().ToString();
    }
}
