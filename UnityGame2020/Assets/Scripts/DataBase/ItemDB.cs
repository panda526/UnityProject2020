using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDB", menuName = "DB/ItemDB", order = 1)]
public class ItemDB : ScriptableObject
{
	public List<Item> itemList;
    public Item SearchItemByID(string itemID)
    {
        Item item = null;
        for (int i = 0; i < itemList.Count; i++)
        {
            if (itemList[i].id == itemID)
            {
                item = itemList[i].Clone();
                break;
            }
        }
        return item;
    }
}