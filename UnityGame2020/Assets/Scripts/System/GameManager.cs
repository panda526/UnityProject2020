using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
/// <summary>
/// GM本體
/// </summary>
public class GameManager
{
    //唯一的GM static表示唯一的自己
    public static GameManager ctrl;
    /*
    private PlayerInfoSystem m_PlayerInfoSys;
    //玩家資訊系統管理
    public PlayerInfoSystem PlayerInfoSys
    {
        get { return m_PlayerInfoSys; }
    }
    */
    /// <summary>
    /// 建構GM上線
    /// </summary>
    public GameManager()
    {
        if (ctrl != null) return;
        ctrl = this;
        Debug.Log("GM來惹喔喔喔");
        m_PlayerInfoSys = new PlayerInfoSystem();
        m_TargetSys = new TargetSystem();
        m_BagSys = new BagSystem();
        m_BuffSys = new BuffSystem();
    }
    private PlayerInfoSystem m_PlayerInfoSys;
    public PlayerInfoSystem PlayerInfoSys
    {
        get
        {
            //第一次啟動玩家資訊系統
           // if (m_PlayerInfoSys == null) m_PlayerInfoSys = new PlayerInfoSystem();
            return m_PlayerInfoSys;
        }
    }
    //目標系統管理
    private TargetSystem m_TargetSys;
    public TargetSystem TargetSys
    {
        get
        {
            //第一次啟動目標系統
            //if (m_TargetSys == null) m_TargetSys = new TargetSystem();
            return m_TargetSys;
        }
    }
    //背包系統管理
    private BagSystem m_BagSys;
    public BagSystem BagSys
    {
        get
        {
            //第一次啟動目標系統
            //if (m_TargetSys == null) m_TargetSys = new TargetSystem();
            return m_BagSys;
        }
    }
    //物件池管理
    private ObjectPoolManager m_objectPool;
    public ObjectPoolManager objectPool
    {
        get
        {
            if (m_objectPool == null) m_objectPool = new GameObject("ObjPoolManager").AddComponent<ObjectPoolManager>();
            return m_objectPool;
        }
    }
    private BuffSystem m_BuffSys;
    public BuffSystem BuffSys
    {
        get
        {
            //第一次啟動玩家資訊系統
            // if (m_PlayerInfoSys == null) m_PlayerInfoSys = new PlayerInfoSystem();
            return m_BuffSys;
        }
    }
    public void BackToMenu()
    {
        objectPool.RecycleAll();
        TargetSys.Clear();
        BuffSys.ResetAllBuff();
        objectPool.RecycleAllHeart();
    }
}
/// <summary>
/// 目標系統
/// </summary>
public class TargetSystem
{
    public List<MonsterCtrl> monsterList { get; private set; }

    public TargetSystem()
    {
        monsterList = new List<MonsterCtrl>();
    }
    public void AddMonster(MonsterCtrl monster)
    {
        monsterList.Add(monster);
    }
    public void RemoveMonster(MonsterCtrl monster)
    {
        monsterList.Remove(monster);
    }
    public void Clear()
    {
        monsterList.Clear();
    }
    /// <summary>
    /// 搜尋最近敵人
    /// </summary>
    /// <param name="pos">每隻怪物位置</param>
    /// <returns>回傳最近的怪物</returns>
    public MonsterCtrl Search(Transform pos)
    {
        MonsterCtrl monster = null;
        float range = 999999;
        for (int i = 0; i < monsterList.Count; i++)
        {
            if (Vector3.Distance(pos.position, monsterList[i].transform.position) < range)
            {
                range = Vector3.Distance(pos.position, monsterList[i].transform.position);
                monster = monsterList[i];
            }
        }
        return monster;
    }
}
public class Buffs
{
    public const string FrontShoot = "frontshoot";
    public const string SideShoot = "sideshoot";
    public const string BackShoot = "backshoot";
    public const string BowShoot = "bowshoot";
    //public const string CycleShoot = "cycleshoot";
    public const string AttackUp = "attackup(small)";
    public const string AttackUp2 = "attackup(big)";
    public const string MaxHPUp = "maxhpup(small)";
    public const string MaxHPUp2 = "maxhpup(big)";
    public const string Heal = "Recovery";
}
/// <summary>
/// Buff系統
/// </summary>
public class BuffSystem
{
    public Dictionary<string,BuffData> buffs;
    public BuffSystem()
    {
        buffs = new Dictionary<string, BuffData>();
        //建立技能字典
        buffs.Add(Buffs.FrontShoot, new BuffData(1,1,-1));
        buffs.Add(Buffs.SideShoot, new BuffData(1, 1, -1));
        buffs.Add(Buffs.BackShoot, new BuffData(1, 1, -1));
        buffs.Add(Buffs.BowShoot, new BuffData(1, 1, -1));
        //buffs.Add(Buffs.CycleShoot, new BuffData(1, 1, -1));
        buffs.Add(Buffs.AttackUp, new BuffData(15, 1, -1));
        buffs.Add(Buffs.AttackUp2, new BuffData(35, 1, -1));
        buffs.Add(Buffs.MaxHPUp, new BuffData(120, 1, -1));
        buffs.Add(Buffs.MaxHPUp2, new BuffData(280, 1, -1));
    }
    /// <summary>
    /// 隨機取得一個技能名稱(ID)
    /// </summary>
    /// <returns>技能名稱(KEY)</returns>
    public List<string> GetRandomBuffNames()
    {
        List<int> numL=new List<int>();
        List<string> stringList=new List<string>();
        for (int i=0;i<3;)
        {
            int j=Random.Range(0,buffs.Count);
            if(numL.Contains(j))
            {

            }
            else
            {
                numL.Add(j);
                stringList.Add(buffs.Keys.ElementAt(j));
                i++;
            }
        }
        return stringList;
    }
    public void AddBuff(string key)
    {
        buffs[key].AddAmount();
    }

    public int GetBuffAmount(string key)
    {
        return buffs[key].amount;
    }
    public int GetAttackBuff()
    {
        return buffs[Buffs.AttackUp].amount + buffs[Buffs.AttackUp2].amount;
    }
    public float GetHPBuff()
    {
        return buffs[Buffs.MaxHPUp].amount + buffs[Buffs.MaxHPUp2].amount;
    }
    public void ResetAllBuff()
    {
        for (int i = 0; i < buffs.Count; i++)
        {
            buffs.Values.ElementAt(i).ClearAmount();
        }
    }
}

public class BuffData
{
    public int number { get; private set; }//每層增加的值
    public int stack { get; private set; }//堆疊層數(不可堆疊為-1)
    public float duration { get; private set; }
    public int amount = 0;//數量
    public BuffData(int number,int stack,float duration)
    {
        this.number = number;
        this.stack = stack;
        this.duration = duration;
    }
    /// <summary>
    /// 增加堆疊數量
    /// </summary>
    public void AddAmount()
    {
        amount += number;
    }
    /// <summary>
    /// 清除堆疊數量
    /// </summary>
    public void ClearAmount()
    {
        amount = 0;
    }
}
public enum TimerType { CountUp,CountDown };
/// <summary>
/// 計時器
/// </summary>
public sealed class Timer
{
    public TimerType type { get; private set; }
    public float duration { get; private set; }
    public float time { get; private set; }
    /// <summary>
    /// 是否完成計時(倒數歸零or正數數到最後)
    /// </summary>
    public bool isDone
    {
        get
        {
            switch(type)
            {
                case TimerType.CountUp:
                    return time >= duration;
                case TimerType.CountDown:
                    return time <= 0;
                default: 
                    return true;
            }
        }
    }
    /// <summary>
    /// 創建計時器(可設定正數/倒數)
    /// </summary>
    /// <param name="duration">時間</param>
    /// <param name="type">類型(正數或倒數)</param>
    public Timer(float duration, TimerType type = TimerType.CountDown)
    {
        this.duration = duration;
        this.type = type;
        Start();
    }

    /// <summary>
    /// 更動計時器秒數
    /// </summary>
    /// <param name="duration">時間</param>
    /// <param name="type">類型(正數或倒數)</param>
    public void ChangeTimerDuration(float duration)
    {
        this.duration = duration;
        Start();
    }
    /// <summary>
    /// 開始計時
    /// </summary>
    public void Start()
    {
        switch (type)
        {
            case TimerType.CountUp:
                time = 0f;
                break;
            case TimerType.CountDown:
                time = duration;
                break;
        }
    }
    /// <summary>
    /// 更新計時器時間
    /// </summary>
    /// <param name="t">每次更新+t秒(通常會是Time.deltaTime)</param>
    public bool Update(float t)
    {
        switch (type)
        {
            case TimerType.CountUp:
                time += t;
                break;
            case TimerType.CountDown:
                time -= t;
                break;
        }
        return isDone;
    }
}