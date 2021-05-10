using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#region 門、波、生怪點資訊
/// <summary>
/// 關卡內門的資料
/// </summary>
[System.Serializable]
public class GateData
{
	[Header("波次清單")]
	public List<WaveData> waves;
	public int waveIndex { get; private set; }
	public BlockDB blocks;
	public List<SpawnPointData> GetWaveData()
	{
		//Debug.Log("波次:" + (waveIndex+1) + "/" + waves.Count);
		return waves[waveIndex].spawnPoints;
	}
	public bool NextWave()
    {
		waveIndex++;
		return waveIndex < waves.Count;
    }
}

/// <summary>
/// 生怪波次的資料
/// </summary>
[System.Serializable]
public class WaveData
{
	[Header("生怪點清單")]
	public List<SpawnPointData> spawnPoints;
}
/// <summary>
/// 生怪點的資料
/// </summary>
[System.Serializable]
public class SpawnPointData
{
	public Transform point;
	public MonsterCtrl monster;
}
#endregion
public class StageManager : ISystem {
	public static StageManager ctrl;
	public GameObject teleport;
	public Transform startPoint;
	[Header("門數量清單")]
	public List<GateData> gates;
	public int gatesIndex;
	

	[Header("波次間隔時間")]
	public float sec = 8f;
	private List<MonsterCtrl> sceneMonsters = new List<MonsterCtrl>();
	private bool stageClear=false;
	private bool gateClear=false;

	[Header("資料")]
	public float exp = 0f;
	public float expMax = 100f;
	public float expPercent { get { return exp / expMax; } }
	public float expTotal = 0f;
	public int moneyTotal = 0;
    public List<Sprite> sprites = new List<Sprite>();//圖示清單
	private List<ItemForRecovery> HeartList = new List<ItemForRecovery>();//血包管理
	public EffectBase raining;
	public BlockDB blockDB { get { return gates[gatesIndex].blocks; } }
	public Queue<BlockBase> blocksInStage = new Queue<BlockBase>();
	public bool isRain = false;

	#region 設定計時器
	private Timer m_timer;
	private Timer timer
	{
		get
		{
			if (m_timer == null) m_timer = new Timer(5);
			return m_timer;
		}
	}
    #endregion

	void Awake()
    {
		ctrl = this;
    }
	// Use this for initialization
	void Start()
	{
		SystemCheck();
		//生怪(進場後1.5s)
		Invoke("Spawn", 1.5f);
		if (AudioManager.ctrl == null) Debug.Log("gggg");
		AudioManager.ctrl.PlayBGM("bgm_wind");
		//關卡遊戲介面刷新
		//UIStageInfoCtrl.ctrl.UpdateExpBar(expPercent);
		//Rain();
		CreateBlocks();
	}
	#region 障礙物生成
	void CreateBlocks()
    {
		for(int i =0; i< blockDB.blockData.Count;i++)
        {
			for (int j = 0; j < blockDB.blockData[i].posList.Count; j++)
			{
				Vector3 pos = blockDB.blockData[i].posList[j];
				BlockBase block = GameManager.ctrl.objectPool.Reuse(blockDB.blockData[i].block);
				if (block != null)
				{
					block.Reuse(pos, Quaternion.identity);
					block.gameObject.SetActive(true);
				}
				else
				{
					block = Instantiate(blockDB.blockData[i].block, pos, Quaternion.identity);
				}
				blocksInStage.Enqueue(block);
			}
		}
    }
	void ClearBlocks()
    {
        while (blocksInStage.Count > 0) 
		{
			blocksInStage.Dequeue().Recycle();
		}
    }
#endregion 
	#region 下雨特效
	/*
    void Rain()
    {
		int x = Random.Range(0, 1);
        if(x == 0)
        {
			isRain = true;
			SpawnRain();
        }
        else 
		{
			if (isRain == true)
            {
				raining.Recycle(); Debug.Log("沒下雨成功");
			}
			isRain = false;
		}
    }
	void SpawnRain()
    {
		EffectBase effect = GameManager.ctrl.objectPool.Reuse(raining);
		if (effect != null)
		{
			effect.Reuse(transform.position, transform.rotation);
		}
		Instantiate(raining, transform.position, transform.rotation);
		Debug.Log("下雨成功");
	}
	*/
	#endregion

	// Update is called once per frame
	void Update () {
		if (stageClear || gateClear) return;
		if(timer.Update(Time.deltaTime))
        {
			if (gates[gatesIndex].NextWave())
			{
				Spawn();
			}
		}
	}
	/// <summary>
	/// 生怪
	/// </summary>
	void Spawn()
	{
		List<SpawnPointData> pointDatas = gates[gatesIndex].GetWaveData();
        for (int i = 0; i < pointDatas.Count; i++)
		{
            MonsterCtrl monster = null;
			Vector3 pos = pointDatas[i].point.position;
            monster = GameManager.ctrl.objectPool.Reuse(pointDatas[i].monster);
			if(pointDatas[i].monster.boss)
            {
				monster = Instantiate(pointDatas[i].monster, pos, Quaternion.identity);
			}
			else if (monster != null)
			{
				monster.Reuse(pos, Quaternion.identity);			
			}
			else
			{
				//回收池未找到怪物，直接生成
				monster=Instantiate(pointDatas[i].monster, pos, Quaternion.identity);
			}
			sceneMonsters.Add(monster);
        }
        timer.Start();
	}
	public void RemoveMonster(MonsterCtrl monster) 
	{
		sceneMonsters.Remove(monster);
        {
            if (sceneMonsters.Count == 0)
            {
				if (gates[gatesIndex].NextWave())
				{
					Spawn();
				}
				else
				{
					Debug.Log("全波次過關");
					AudioManager.ctrl.PlaySFX("Portal_open");
					StageClear(true);
				}
            }
        }
	}
	/// <summary>
	/// 過關 開啟傳送門
	/// </summary>
	/// <param name="B"></param>
	void StageClear(bool B)
    {
		gateClear = B;
		teleport.SetActive(B);
        //回收掉落消耗品
        if (B)
        {
			Invoke("ClearHeartList", 0.7f);
		}
	}
	/// <summary>
	/// 下一道門的檢查(出口=結束)
	/// </summary>
	public void NextGate()
	{
		StageClear(false);
		if(gatesIndex < gates.Count)
		{//下一道門，生怪
			Spawn();
		}
	}
	//public void CountMonster() { Debug.Log(sceneMonsters.Count); }
	/// <summary>
	/// 掉寶
	/// </summary>
	/// <param name="dropList">資料庫讀取</param>
	/// <param name="dropCount">掉落數量</param>
	public string Drop(DropListDB dropList, int dropCount = 1)
	{
		string itemID = "";
		for (int j = 1; j <= dropCount; j++)
		{
			float chance = Random.Range(0f, 100f);
			for (int i = 0; i < dropList.dropList.Count; i++)
			{
				if (chance < dropList.dropList[i].probability)
				{
					//物品送進背包&結算紀錄
					itemID = dropList.dropList[i].itemID;

					if (itemID != "none" && itemID != Buffs.Heal)
					{
						Item item = DataBaseManager.ctrl.ItemDB.SearchItemByID(itemID);
						GM.BagSys.UpdateItem(item);
						//if (!sprites.Contains(item.icon))
						sprites.Add(item.icon);
					}	
				}
			}
		}
		float e = dropList.exp;
		expTotal += e;
		GM.PlayerInfoSys.GetExp(e);
		int m = dropList.Money();
		moneyTotal += m;
		GM.PlayerInfoSys.SetCoin(m);

		return itemID;
	}
	public void SetExp(float f)
    {
		exp += f;
		if(exp>= expMax)
        {
			exp -= expMax;
			Time.timeScale = 0;//時間速度為0(暫停)
			UIStageInfoCtrl.ctrl.OpenSelectSkillPanel();
		}
		UIStageInfoCtrl.ctrl.UpdateExpBar(expPercent);
	}

	public void GameOver()
	{
		StartCoroutine(GameOverWaiting());
	}
	IEnumerator GameOverWaiting()
	{
		float time = 1;
		while (time > 0)
		{
			yield return new WaitForSeconds(0.05f);
			time -= 0.05f;
		}
		UIStageInfoCtrl.ctrl.OpenGameOverPanel(expTotal, moneyTotal, sprites);
	}
	/// <summary>
	/// 淡出函數 裡面的StartCorotine可以做出延遲效果
	/// </summary>
	public void FadeOut()
	{
		if (sceneMonsters.Count != 0) return;
		gatesIndex++;
		if (gatesIndex >= gates.Count)
		{//彈出恭喜過關介面(顯示掉落物，經驗，錢...BABABALA)
		 //Debug.Log("關卡完成"); 
			stageClear = true;
			//掉落資訊的傳遞(StageInfoCtrl, 獲得總經驗、金錢、物品圖片清單)
			UIStageInfoCtrl.ctrl.OpenResultPanel(expTotal, moneyTotal, sprites);
			AudioManager.ctrl.PlaySFX("Stage_clear");
		}
		else
		{
			AudioManager.ctrl.PlaySFX("Portal_enter");
			StartCoroutine(Count());
		}
	}
	IEnumerator Count()
	{
		float time = 1;
		while (time > 0)
		{
			yield return new WaitForSeconds(0.05f);
			time -= 0.05f;
			UIStageInfoCtrl.ctrl.UpdateBlackScreen(1 - time);
		}
		NextGate();
		ClearBlocks();
		CreateBlocks();
		GM.objectPool.RecycleAllBullet();
		PlayerCtrl.ctrl.ResetPos(startPoint);
		UIStageInfoCtrl.ctrl.UpdateBlackScreen(0);
		//Rain();
	}
#region 血包相關
	public void AddHeartToList(ItemForRecovery heart)
    {
		HeartList.Add(heart);
    }

	public void RemoveHeartFromList(ItemForRecovery heart)
	{
		HeartList.Remove(heart);
	}

	public void ClearHeartList()
	{
		while (HeartList.Count > 0) 
		{
			HeartList[0].UseItem();
		}
		//HeartList.Clear();
	}
#endregion
}
