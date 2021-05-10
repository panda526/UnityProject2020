using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHPBarCtrl : MonoBehaviour {
	
	public RectTransform PanRT;
	private RectTransform m_RT;
	public RectTransform RT
	{
		get
		{
			if (m_RT == null) m_RT = GetComponent<RectTransform>();
			return m_RT;
		}
	}
	public float W
	{
		get { return PanRT.sizeDelta.x; }
	}
	public float H
	{
		get { return PanRT.sizeDelta.y; }
	}
	public Image hpBarImg;
	public void SetData()
	{
		hpBarImg.fillAmount = 1;
		gameObject.SetActive(true);
	}
	public void UpdateUI(float F)
	{
		hpBarImg.fillAmount = F;
		if (F <= 0)
        {
			gameObject.SetActive(false);
		}
	}
	public void UpdatePos(Vector2 pos)
	{
		RT.anchoredPosition = new Vector2(W * pos.x - 75f, H * pos.y - 100f);
	}
}
