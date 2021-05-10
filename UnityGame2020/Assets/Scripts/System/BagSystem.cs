using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BagSystem
{
	public Dictionary<string, Item> items;//宣告物品清單(字典)
	public int slotCount { get; private set; }
	private List<Item> itemList
	{
		get
		{
			return items.Values.ToList();//字典轉清單
		}
	}
	public BagSystem(int slotCount = 1000)
	{
		items = new Dictionary<string, Item>(); //建立物品清單(字典)
		this.slotCount = slotCount;
	}
	public void UpdateItem(Item item, int Count = 1)
	{
		if (!items.ContainsKey(item.id))
		{
			items.Add(item.id, item);
		}
		items[item.id].UpdateAmount(Count);
		//Debug.Log(items[item.id].name + ":" + items[item.id].amount);
	}

	public Item SearchItemByIndex(int index)
    {
		return index < itemList.Count ? itemList[index] : null;
    }
}
