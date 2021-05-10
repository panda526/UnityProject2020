using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBagSlotCtrl : MonoBehaviour {
	public Text countText; //堆疊數量UI
	public Image iconImg; //圖片
	private Color onColor = Color.white, offColor = Color.clear; //顏色 開啟->白 關閉->透明
	/// <summary>
	/// 設定背包UI
	/// </summary>
	/// <param name="item">物品(Item) 預設為空</param>
	public void SetData (Item item = null,bool full = false) 
	{
		if (!gameObject.activeSelf) gameObject.SetActive(true);
		if (item != null)
        {
			iconImg.color = onColor;
			iconImg.sprite = item.icon;
            if (item.isStackable)
            {
				countText.text = full ? item.stackLimit.ToString() : item.amount_remainder.ToString();
			}
			else countText.text = "";
        }
        else
        {
			iconImg.color = offColor;
			countText.text = "";
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

