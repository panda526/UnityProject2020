using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour {
	public Dictionary<string, object> pool 
		= new Dictionary<string, object>();
	
	// Use this for initialization
	void Start ()
	{
		DontDestroyOnLoad(this);
	}
	/// <summary>
	/// 重用後從物件池移出
	/// </summary>
	/// <typeparam name="T">泛型</typeparam>
	/// <param name="type">任何需要被回收的物件</param>
	/// <returns>丟出去的物件</returns>
	public T Reuse<T>(T type)
	{
		string typename = type.GetType().Name;
		object obj = null;
		if (pool.ContainsKey(typename) && ((Queue<T>)pool[typename]).Count > 0)
		{
            obj = ((Queue<T>)pool[typename]).Dequeue();
			//Debug.Log(typename + "已重用完成");
			//((Queue<T>)pool[typename]).Remove((T)obj); 原本用List要刪除 改Queue後可自動刪除
			(type as MonoBehaviour).transform.SetParent(null);//實體管理(放出)
			(type as MonoBehaviour).gameObject.SetActive(true);//管理-重新顯示
		}
		return (T)obj;
	}
	/// <summary>
	/// 摧毀後回收到物件池
	/// </summary>
	/// <typeparam name="T">泛型類別</typeparam>
	/// <param name="type">任何需要被回收的物件</param>
	public void GetRecycle<T> (T type)
	{
		string typename = type.GetType().Name;
		if (!pool.ContainsKey(typename))
		{//未曾有紀錄，先建立pool資料庫
			pool.Add(typename, new Queue<T>());
			//Debug.Log(typename + "已回收完成");
		}
		((Queue<T>)pool[typename]).Enqueue(type);
		(type as MonoBehaviour).transform.SetParent(transform);//實體管理(回收)
		(type as MonoBehaviour).gameObject.SetActive(false);//管理-不顯示
	}
	public void RecycleAll()
	{
		Queue<AmmoBase> ammoList = new Queue<AmmoBase>(FindObjectsOfType<AmmoBase>());
		while (ammoList.Count > 0)
		{
			GetRecycle(ammoList.Dequeue());
		}
		
		Queue<MonsterCtrl> monsterList = new Queue<MonsterCtrl>(FindObjectsOfType<MonsterCtrl>());
		while (monsterList.Count > 0)
		{
			GetRecycle(monsterList.Dequeue());
		}
		
		Queue<BlockBase> blocklist = new Queue<BlockBase>(FindObjectsOfType<BlockBase>());
		while (blocklist.Count > 0)
		{
			GetRecycle(blocklist.Dequeue());
		}
	}
	public void RecycleAllBullet()
	{
		Queue<AmmoBase> ammoList = new Queue<AmmoBase>(FindObjectsOfType<AmmoBase>());
		while (ammoList.Count > 0)
		{
			GetRecycle(ammoList.Dequeue());
		}	
	}
	public void RecycleAllHeart()
	{
		Queue<ItemForRecovery> heartList = new Queue<ItemForRecovery>(FindObjectsOfType<ItemForRecovery>());
		while (heartList.Count > 0)
		{
			GetRecycle(heartList.Dequeue());
		}	
	}
}
