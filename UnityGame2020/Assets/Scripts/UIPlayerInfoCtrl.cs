using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerInfoCtrl : ISystem
{
	public static UIPlayerInfoCtrl ctrl;
	#region 人物基本資訊UI
	[Header("人物基本資訊")]
	public Image playerIcon;
	public Image expBar;
	public Text playerName;
	public Text levelText;
	#endregion
	private void Awake()
    {
		ctrl = this;
    }
	#region 貨幣資訊UI
	[Header("貨幣資訊")]
	public Text staminaText;
	public Text coinText;
	public Text crystalText;
    #endregion
    
    #region 計時器與體力回復
    private Timer m_timer;
	private Timer timer
	{
		get {
			if (m_timer == null) m_timer = new Timer(1);
			return m_timer;
		}
	}
	public void StaminaRecovery()
		{
		GM.PlayerInfoSys.StaminaRecovery();
		UpdateStaminaUI();
		}
	#endregion

	#region UI更新
	/// <summary>
	/// 更新全部UI啦
	/// </summary>
	public void UpdateAllUI()
	{
		UpdatePlayerName();
		UpdateLevelUI();
		UpdateExpUI();
		UpdateStaminaUI();
		UpdateCoinUI();
		UpdateCrystalUI();
	}
	public void UpdatePlayerName()
	{
		playerName.text = GM.PlayerInfoSys.playerName;
	}
	public void UpdateLevelUI()
	{
		levelText.text = GM.PlayerInfoSys.level.ToString();
		UpdateExpUI();
	}
	public void UpdateExpUI()
	{
		expBar.fillAmount = GM.PlayerInfoSys.expPercent;
	}
	public void UpdateStaminaUI()
	{
		staminaText.text = GM.PlayerInfoSys.staminaStr;
	}
	public void UpdateCoinUI()
	{
		coinText.text = GM.PlayerInfoSys.coin.ToString();
	}
	public void UpdateCrystalUI()
	{
		crystalText.text = GM.PlayerInfoSys.crystal.ToString();
	}
	#endregion
	// Use this for initialization
	public void Initial()
	{
		UpdateAllUI();
	}
	// Update is called once per frame
	void Update()
	{
		if (timer.isDone && !GM.PlayerInfoSys.staminaIsMax)
		{
			timer.Start();
			StaminaRecovery();
		}
        else { timer.Update(Time.deltaTime); }
		if (Input.GetKeyDown(KeyCode.KeypadPlus))
		{
			GM.PlayerInfoSys.SetStamina(30);
			GM.PlayerInfoSys.SetCoin(5000);
			GM.PlayerInfoSys.SetCrystal(5);
			UpdateAllUI();
		}
		if (Input.GetKeyDown(KeyCode.KeypadMinus))
		{
			GM.PlayerInfoSys.SetStamina(-50);
			GM.PlayerInfoSys.SetCoin(-2000);
			GM.PlayerInfoSys.SetCrystal(-1);
			UpdateAllUI();
		}
	}


}