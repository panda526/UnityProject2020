using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfoSystem
{
	public PlayerInfoSystem() //初始值設定
	{
		playerName = "panda";
		level = 1;
		exp = 0;
		stamina = 0;
		coin = 1200;
		crystal = 0;
	}
	#region 人物基本資訊
	public string playerName { get; private set; }
	public int level { get; private set; }
	public float exp { get; private set; }
	public const float expBase = 100f; //基礎
	public float currentExp //當前值 (全部-之前總和) 分子
	{
		get { return exp - BeforeExp(); }
	}
	public float expMax //分母
	{
		get { return expBase * level; }
	}
	public float expPercent//幾%
	{
		get { return currentExp / expMax; }
	}
	#endregion
	#region 經驗值處理
	public void GetExp(float exp)
	{
		this.exp += exp;
		while (currentExp >= expMax) 
		{
			level++;
		}
	}

	public float BeforeExp()
	{
		float total = 0;
		for (int i = 1; i < level; i++)
		{
			total += expBase * i;
		}
		return total;
	}

	public void LoseExp(float exp)
	{
		if (currentExp - exp > 0) this.exp -= exp;
		else this.exp = BeforeExp();
	}
	#endregion
	#region 貨幣資訊
	public int stamina { get; private set; }
	public int coin { get; private set; }
	public int crystal { get; private set; }
	public const int staminaMax = 100;
	public string staminaStr
	{
		get { return stamina.ToString() + "/" + staminaMax.ToString(); }
	}
	public bool staminaIsMax
	{
		get { return stamina >= staminaMax; }
	}
	#endregion
	#region 貨幣處理
	/// <summary>
	/// 回體力
	/// </summary>
	public void StaminaRecovery()
	{
		if (stamina >= staminaMax) return;
		stamina++;
	}
	/// <summary>
	/// 補體力(滿了不溢補)
	/// </summary>
	/// <param name="stamina"></param>
	public bool SetStamina(int stamina)
	{
		if (this.stamina + stamina < 0) return false;
		this.stamina += stamina;
		if (this.stamina >= staminaMax)
		{
			this.stamina = staminaMax;
		}
		return true;
	}
	/// <summary>
	/// 加減錢錢
	/// </summary>
	/// <param name="coin"></param>
	public void SetCoin(int coin)
	{
		if (this.coin + coin < 0) return;
		this.coin += coin;
	}
	/// <summary>
	/// 加減水晶 抽抽啦
	/// </summary>
	/// <param name="crystal"></param>
	public void SetCrystal(int crystal)
	{
		if (this.crystal + crystal < 0) return;
		this.crystal += crystal;
	}
#endregion
}
