using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataBaseManager : MonoBehaviour
{
	//靜態欄位 表示唯一的自己
	public static DataBaseManager ctrl;
	public StageDB stageDB;
	public ItemDB ItemDB;
	private void Awake()
	{
		ctrl = this;
	}
	// Use this for initialization
	void Start()
	{
		{
			DontDestroyOnLoad(this);
		}
	}
}
