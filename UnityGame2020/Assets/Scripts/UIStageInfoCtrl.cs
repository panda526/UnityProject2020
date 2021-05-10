using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public struct SkillDataGroup
{
	public string skillName;
	public Image img;
	public Text txt;
	public void SetData(string skillName)
	{
		this.skillName = skillName;
		txt.text = skillName;
	}
}
public class UIStageInfoCtrl : ISystem
{
	public static UIStageInfoCtrl ctrl;
	[Header("各式資訊")]
	public Image ExpBar;
	public Text expTotal, moneyTotal,expTotal2,moneyTotal2;
	public SkillDataGroup[] skillDataGroup = new SkillDataGroup[3];
	[Header("面板")]
	public UICanvasGroupCtrl skillPanel;
	public UICanvasGroupCtrl resultPanel;
	public UICanvasGroupCtrl blackScreen;
	public UICanvasGroupCtrl gameoverPanel;
	[Header("掉落物品資訊")]
	public Transform Content;
	public UICellTMPCtrl UICellTMPCtrl;
	public PlayerSkillDB playerSkillDB;
	void Awake()
	{
		ctrl = this;
	}
	public void UpdateBlackScreen(float F)
	{
		blackScreen.Switch(F);
	}
	public void UpdateExpBar(float F)
	{
		ExpBar.fillAmount = F;
	}
	public void SelectSkill(int i)
	{
		Time.timeScale = 1;
		skillPanel.Switch(false);
		GM.BuffSys.AddBuff(skillDataGroup[i].skillName);
		if (skillDataGroup[i].skillName == "maxhpup(small)" | skillDataGroup[i].skillName == "maxhpup(big)")
		{
			int num = GM.BuffSys.buffs[skillDataGroup[i].skillName].amount;
			PlayerCtrl.ctrl.Hurt(-num);
		}
		PlayerCtrl.ctrl.UpdateBuff();
	}
	public void OpenSelectSkillPanel()
	{
		skillPanel.Switch(true);
		List <string> name = GM.BuffSys.GetRandomBuffNames();
		for (int i = 0; i < 3; i++)
		{
			skillDataGroup[i].SetData(name[i]);
			skillDataGroup[i].img.sprite = playerSkillDB.GetSkillImg(name[i]);
		}
	}
	public void OpenGameOverPanel(float exp, int money, List<Sprite> imgList)
	{
		gameoverPanel.Switch(true);
		expTotal2.text = exp.ToString();
		moneyTotal2.text = money.ToString();
		StartCoroutine(ShowItem(imgList));
	}
	public void OpenResultPanel(float exp, int money, List<Sprite> imgList)
	{
		resultPanel.Switch(true);
		expTotal.text = exp.ToString();
		moneyTotal.text = money.ToString();
		StartCoroutine(ShowItem(imgList));
	}
	IEnumerator ShowItem(List<Sprite> imgList)
	{
		for (int i = 0; i < imgList.Count; i++)
		{
			Instantiate(UICellTMPCtrl, Content).SetData(imgList[i]);
			yield return new WaitForSeconds(0.2f);
		}
	}
	public void Pause()
	{
		Time.timeScale = Time.timeScale == 1 ? 0 : 1;
	}
}

