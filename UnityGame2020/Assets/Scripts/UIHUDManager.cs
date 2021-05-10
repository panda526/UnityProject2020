using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHUDManager : MonoBehaviour {

	public static UIHUDManager ctrl;
	public UIHPBarCtrl hpBarTMP;
	public List<UIHPBarCtrl> barCtrls = new List<UIHPBarCtrl>();

	void Awake()
	{
		ctrl = this;
	}
	/// <summary>
	/// 初始化HPBarCtrl並回傳
	/// </summary>
	/// <returns>回傳一個可用的HPBar</returns>
	public UIHPBarCtrl SetBarCtrl()
	{
		UIHPBarCtrl bar = null;

		for (int i = 0; i < barCtrls.Count; i++)
		{//搜尋已存在的HUD Bar
			if (!barCtrls[i].gameObject.activeSelf)
			{
				bar = barCtrls[i];
				bar.SetData();
				break;
			}
		}

		if (bar == null)
		{//都無閒置的HUD Bar建立新的
			bar = Instantiate(hpBarTMP, transform);
			barCtrls.Add(bar);
			bar.SetData();
		}
		return bar;
	}
}
