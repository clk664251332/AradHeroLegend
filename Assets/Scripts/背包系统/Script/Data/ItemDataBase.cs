using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]//(menuName = "MySubMenue/Create MyScriptableObject ")]
public class ItemDataBase : ScriptableObject
{
    public ItemCategory[] Categories { get { return m_Categories; } }

    [SerializeField]
    private ItemCategory[] m_Categories;

    //[SerializeField]
   // private ItemProperty[] m_ItemProperties;

    /// <summary>
    /// 
    /// </summary>
    public bool FindItemById(int id, out ItemData itemData)
    {
        for (int i = 0; i < m_Categories.Length; i++)
        {
            var category = m_Categories[i];
            for (int j = 0; j < category.Items.Length; j++)
            {
                if (category.Items[j].Id == id)
                {
                    itemData = category.Items[j];
                    return true;
                }
            }
        }

        itemData = null;
        return false;
    }

    /// <summary>
    /// 
    /// </summary>
    public bool FindItemByName(string name, out ItemData itemData)
    {
        for (int i = 0; i < m_Categories.Length; i++)
        {
            var category = m_Categories[i];
            for (int j = 0; j < category.Items.Length; j++)
            {
                if (category.Items[j].Name == name)
                {
                    itemData = category.Items[j];
                    return true;
                }
            }
        }

        itemData = null;
        return false;
    }
}
