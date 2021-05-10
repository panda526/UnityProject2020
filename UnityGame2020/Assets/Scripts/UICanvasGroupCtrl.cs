using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(CanvasGroup))]//表示需要某零件
public class UICanvasGroupCtrl : MonoBehaviour 
{
	private CanvasGroup m_CG; //實體對象
	public CanvasGroup CG //資料接口
	{
		get
		{ 
		if (m_CG ==null) m_CG=GetComponent<CanvasGroup>();
		//因為GetComponent吃效能 故只尋找一次 優化效能
		return m_CG;
		}
		//直接取得物件的零件且不給修改(沒有set;)
	}
	// Use this for initialization
	void Start () {
	}
	
	public void Switch(bool B)
	{
		CG.blocksRaycasts = B;
		CG.alpha = B ? 1 : 0;
		/*上面那兩行等於下面這整塊
		if (B)
		{
			CG.alpha = 1;
			CG.blocksRaycasts = true;
		}
		else
		{
			CG.alpha = 0;
			CG.blocksRaycasts = false;
		}
		*/
	}
	public void Switch()
	{
		CG.blocksRaycasts = !CG.blocksRaycasts;
		CG.alpha = CG.blocksRaycasts ? 1 : 0;
	}
	public void Switch(float F)
	{
		CG.alpha = F;
		CG.blocksRaycasts = CG.alpha > 0;
	}
}
