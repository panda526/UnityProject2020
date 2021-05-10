using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 關卡外部資訊
/// </summary>
[System.Serializable]
public struct StageData 
{
	public string name;
	public string stageID;
	public string description;
	public int staminaCost;
	[Header("關卡圖示")]
	public Sprite icon;
	public Sprite symbol;
}
[CreateAssetMenu(fileName = "StageDB", menuName = "DB/StageDB", order = 0)]
public class StageDB : ScriptableObject
{
	public List<StageData> stageDatas;
}

