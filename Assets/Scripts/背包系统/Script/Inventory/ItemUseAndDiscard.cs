using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
/// <summary>
/// 控制物品使用事件的类，订阅格子的点击事件，获取点击物品的数据
/// </summary>
public class ItemUseAndDiscard : MonoSingleton<ItemUseAndDiscard> {

	///// <summary>
 //   ///使用物品的函数 
 //   /// </summary>
 //   public void UseItem(Slot slot)
 //   {
 //       if (slot.SlotType == SlotType.背包&&slot.CurrentItem.MainType==ItemMainType.消耗品)
 //       {
 //           //使用减少
 //           slot.ItemHolder.RemoveFromStack(1);
 //       }
       
 //   }
 //   /// <summary>
 //   ///使用装备的函数 
 //   /// </summary>
 //   public void UseEquipment(Slot slot)
 //   {
 //       if (slot.SlotType == SlotType.背包)
 //       {
 //           //使用减少
 //           slot.ItemHolder.RemoveFromStack(1);
 //       }

 //   }

 //   /// <summary>
 //   /// 卸下装备的函数
 //   /// </summary>
 //   public void DisUseEquipment(Slot slot)
 //   {
 //       //反应用物品属性效果
 //       foreach (ItemProperty temp in slot.CurrentItem.Propertys)
 //       {
 //           switch (temp.property)
 //           {
 //               case PropertyType.生命值:
 //                   PlayerState.Instance.HP.Set(PlayerState.Instance.HP.Get() - temp.Value);
 //                   break;
 //               case PropertyType.魔法值:
 //                   PlayerState.Instance.MP.Set(PlayerState.Instance.MP.Get() - temp.Value);
 //                   break;
 //               case PropertyType.经验:
 //                   //PlayerState.Instance.Exp.Set(PlayerState.Instance.Exp.Get() + temp.Value);
 //                   break;
 //               case PropertyType.力量:
 //                   PlayerState.Instance.Strength.Set(PlayerState.Instance.Strength.Get() - temp.Value);
 //                   break;
 //               case PropertyType.体力:
 //                   PlayerState.Instance.Endurance.Set(PlayerState.Instance.Endurance.Get() - temp.Value);
 //                   break;
 //               case PropertyType.智力:
 //                   PlayerState.Instance.Intelligence.Set(PlayerState.Instance.Intelligence.Get() - temp.Value);
 //                   break;
 //               case PropertyType.精神:
 //                   PlayerState.Instance.Spirit.Set(PlayerState.Instance.Spirit.Get() - temp.Value);
 //                   break;
 //               case PropertyType.物理攻击:
 //                   PlayerState.Instance.PhysicalAttack.Set(PlayerState.Instance.PhysicalAttack.Get() - temp.Value);
 //                   break;
 //               case PropertyType.物理防御:
 //                   PlayerState.Instance.PhysicalDefence.Set(PlayerState.Instance.PhysicalDefence.Get() - temp.Value);
 //                   break;
 //               case PropertyType.魔法攻击:
 //                   PlayerState.Instance.MagicAttack.Set(PlayerState.Instance.MagicAttack.Get() - temp.Value);
 //                   break;
 //               case PropertyType.魔法防御:
 //                   PlayerState.Instance.MagicDefence.Set(PlayerState.Instance.MagicDefence.Get() - temp.Value);
 //                   break;
 //               case PropertyType.武器种类:
 //                   //if (!InventoryController.Instance.AddItemToCollectionByData(tempItem.CurrentItem.ItemData, 1, "武器"))
 //                       //Debug.Log("添加装备失败");
 //                   break;
 //               case PropertyType.上衣种类:
 //                   //InventoryController.Instance.AddItemToCollectionByData(tempItem.CurrentItem.ItemData, 1, "上衣");
 //                   break;
 //               case PropertyType.下装种类:
 //                   //InventoryController.Instance.AddItemToCollectionByData(tempItem.CurrentItem.ItemData, 1, "下装");
 //                   break;
 //               case PropertyType.戒指种类:
 //                   //InventoryController.Instance.AddItemToCollectionByData(tempItem.CurrentItem.ItemData, 1, "戒指");
 //                   break;
 //               default:
 //                   break;
 //           }
 //       }

 //       //背包栏增加
 //       InventoryController.Instance.AddItemToCollectionByData(slot.CurrentItem.ItemData, 1, "背包");
 //       //使用减少
 //       slot.ItemHolder.RemoveFromStack(1);
 //       //tempItem.ClearItem();
 //   }
 //   /// <summary>
 //   /// 用于交换物品时，把原物品的属性减掉
 //   /// </summary>
 //   public void DisUseProperty(ItemHolder holder)
 //   {

 //       foreach (ItemProperty temp in holder.CurrentItem.Propertys)
 //       {
 //           switch (temp.property)
 //           {
 //               case PropertyType.生命值:
 //                   PlayerState.Instance.HP.Set(PlayerState.Instance.HP.Get() - temp.Value);
 //                   break;
 //               case PropertyType.魔法值:
 //                   PlayerState.Instance.MP.Set(PlayerState.Instance.MP.Get() - temp.Value);
 //                   break;
 //               case PropertyType.经验:
 //                   //PlayerState.Instance.Exp.Set(PlayerState.Instance.Exp.Get() + temp.Value);
 //                   break;
 //               case PropertyType.力量:
 //                   PlayerState.Instance.Strength.Set(PlayerState.Instance.Strength.Get() - temp.Value);
 //                   break;
 //               case PropertyType.体力:
 //                   PlayerState.Instance.Endurance.Set(PlayerState.Instance.Endurance.Get() - temp.Value);
 //                   break;
 //               case PropertyType.智力:
 //                   PlayerState.Instance.Intelligence.Set(PlayerState.Instance.Intelligence.Get() - temp.Value);
 //                   break;
 //               case PropertyType.精神:
 //                   PlayerState.Instance.Spirit.Set(PlayerState.Instance.Spirit.Get() - temp.Value);
 //                   break;
 //               case PropertyType.物理攻击:
 //                   PlayerState.Instance.PhysicalAttack.Set(PlayerState.Instance.PhysicalAttack.Get() - temp.Value);
 //                   break;
 //               case PropertyType.物理防御:
 //                   PlayerState.Instance.PhysicalDefence.Set(PlayerState.Instance.PhysicalDefence.Get() - temp.Value);
 //                   break;
 //               case PropertyType.魔法攻击:
 //                   PlayerState.Instance.MagicAttack.Set(PlayerState.Instance.MagicAttack.Get() - temp.Value);
 //                   break;
 //               case PropertyType.魔法防御:
 //                   PlayerState.Instance.MagicDefence.Set(PlayerState.Instance.MagicDefence.Get() - temp.Value);
 //                   break;
 //               case PropertyType.武器种类:
 //                   //if (!InventoryController.Instance.AddItemToCollectionByData(tempItem.CurrentItem.ItemData, 1, "武器"))
 //                   //Debug.Log("添加装备失败");
 //                   break;
 //               case PropertyType.上衣种类:
 //                   //InventoryController.Instance.AddItemToCollectionByData(tempItem.CurrentItem.ItemData, 1, "上衣");
 //                   break;
 //               case PropertyType.下装种类:
 //                   //InventoryController.Instance.AddItemToCollectionByData(tempItem.CurrentItem.ItemData, 1, "下装");
 //                   break;
 //               case PropertyType.戒指种类:
 //                   //InventoryController.Instance.AddItemToCollectionByData(tempItem.CurrentItem.ItemData, 1, "戒指");
 //                   break;
 //               default:
 //                   break;
 //           }
 //       }
 //   }
 //   /// <summary>
 //   /// 丢掉物品的函数
 //   /// </summary>
 //   public void DiscardItem(Slot slot)
 //   {
 //       slot.ItemHolder.RemoveFromStack(1);
 //   }
}
    
