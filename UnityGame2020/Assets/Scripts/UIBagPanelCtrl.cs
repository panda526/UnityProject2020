using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBagPanelCtrl :ISystem {
	[Header("格子欄位模組")]
	public UIBagSlotCtrl slotCtrl;
	[SerializeField]//強制顯示私變數於介面
	private float cellSize = 168;
	private int totalSlotCount
	{
		get { return GM.BagSys.slotCount; }//從GM的背包系統那裏取得背包格數資訊
	}
	private int colCount = 5;　//幾列(直的)
	private int rowCount //幾排(橫的)
	{
		get
		{
			return (totalSlotCount / colCount) //整除部分
				+ ((totalSlotCount % colCount) > 0 ? 1 : 0); //多出來的格子(不滿一排)
		}
	}
	private Vector2 bagBGSize //背景格大小(往下拉)
	{
		get { return Vector2.up * cellSize * rowCount; }
	}
	private RectTransform m_RT;
	public RectTransform RT
	{
		get
		{
			if (m_RT == null) m_RT = GetComponent<RectTransform>();
			return m_RT;
		}
	}
	public List<UIBagSlotCtrl> slotList = new List<UIBagSlotCtrl>(); //背包格控制
	// Use this for initialization
	void Start () 
	{
		RT.sizeDelta = bagBGSize;//把長方形拉成背景格大小
		for(int i = 0; i < totalSlotCount; i++)
        {
			UIBagSlotCtrl tmp = Instantiate(slotCtrl, RT);
			tmp.SetData();
			tmp.name = "Slot (" + i + ")";
			slotList.Add(tmp);
		}
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.I))
        {
			GM.BagSys.UpdateItem(DataBaseManager.ctrl.ItemDB.SearchItemByID("item0000"));
			//UpdateUI();//刷新背包UI
		}
		if (Input.GetKeyDown(KeyCode.O))
		{
			GM.BagSys.UpdateItem(DataBaseManager.ctrl.ItemDB.SearchItemByID("item0001"));
			//UpdateUI();//刷新背包UI
		}
	}
	public void UpdateUI()
	{
		int index = 0;//物品清單序號
		for (int i = 0; i < slotList.Count; i+=0)
		{
			//依照序號取得物品
			Item item = GM.BagSys.SearchItemByIndex(index);
			//物品不為空，更新格子內資訊
			if (item != null)
			{
				//看該物品要佔幾格
				int count = item.amount_divisor + (item.amount_remainder > 0 ? 1 : 0);
				for (int c = 0; c < count; c++)
				{
					//false的狀況:是最後一格 並且 餘數不為0。-> 並且條件寫完 整個驚嘆號掉。
					slotList[i].SetData(item, !((c == count - 1) && (item.amount_remainder > 0)));
					i++;
				}
				//設定下一件物品序號
				index++;
			}
			else //沒東西的話塞空的
			{
				slotList[i].SetData();
				i++;
			} 
		}
	}
}
