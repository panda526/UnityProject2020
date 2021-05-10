
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterCtrl : UIPosForHUD
{
	#region 系統相關
    private CharacterController m_charCtrl;
	public CharacterController charCtrl
	{
		get
		{
			if (m_charCtrl == null) m_charCtrl = GetComponent<CharacterController>();
			return m_charCtrl;
		}
	}
	private AnimationCtrl m_animator;
	private AnimationCtrl animator
	{
		get
		{
			if (m_animator == null) m_animator = GetComponentInChildren<AnimationCtrl>();
			return m_animator;
		}
	}
	#endregion
	#region 參數相關
	[Header("基本參數")]
	public float hp;
	public float hpMax = 100;
	public float hpPercent { get { return hp / hpMax; } }
	public int damage = 100;
	public float exp = 20.0f;
	public string ID 
	{
		get { return gameObject.name; }
	}
	public bool isDead
	{
		get { return hp <= 0; }
	}
	[Range(0.5f, 10f)]
	public float speed = 5f;
	[Header("攻擊參數")]
	[Range(0.5f, 50f)]
	public float attackSpeed = 3f;
	[Range(4f, 50f)]
	public float attackRange = 15f;
	public bool isMelee;
	public float attackMeleeRange = 4f;
	public float attackDis { get { return isMelee ? attackMeleeRange : attackRange; } }
	[Header("掉落物資料表")]
	public DropListDB dropList;
	public ItemForRecovery heart;
	public int dropCount = 1;
	public bool boss;
	public float atkCoolDown 
	{get 
		{	return 1f / attackSpeed;}
	}
	public bool isMoving
	{
		get { return Vector3.Distance(transform.position, target.transform.position) >= attackDis; }
	}

	public AmmoBase ammo;
	public PlayerCtrl target;
	public float mapH = 22;
	public float mapW = 52;

	private Vector3 attackPos; //子彈出發位置
	private float attackAng;//子彈射擊角度
	protected DirectionBullet DB; //子彈演算參數
#endregion
	#region 設定計時器
	private Timer m_timer;
	protected Timer timer
	{
		get
		{
			if (m_timer == null) m_timer = new Timer(atkCoolDown);
			return m_timer;
		}
	}
	#endregion
	
	// Use this for initialization
	void Start()
	{
		hp = hpMax;
		GameManager.ctrl.TargetSys.AddMonster(this);
		target = PlayerCtrl.ctrl;
		hpBar = UIHUDManager.ctrl.SetBarCtrl();
		animator.SetAttackSpeed2(attackSpeed);		
		DB.Bow = 0;
		DB.Cycle = 0;
		DB.Side = 0;
		SpacialAction();
	}
	public virtual void SpacialAction()
    {

    }
	// Update is called once per frame
	void Update()
	{
		if (!target || isDead) { return; } 
		if (GetHpBar()) hpBar.UpdatePos(GetScreenPos());
		if (isMoving)
		{
			animator.SetType2(AnimaType.Move);
			Move();
			Rotate();
		}
		else if (target && !target.isDead) 
		{ 
			transform.LookAt(target.transform);
			if (timer.Update(Time.deltaTime))
			{
				animator.SetType2(AnimaType.Attack1);
				attackPos = transform.position;
				attackAng = Vector3.Angle(Vector3.forward, transform.forward)
					* Mathf.Sign(transform.forward.x);
				Invoke("Attack",0.65f *atkCoolDown);
				timer.Start();
			}
		}
		else
		{
			animator.SetType2(AnimaType.Idle);
		}
	}
	void Move()
	{
		charCtrl.SimpleMove(transform.forward * speed);
	}

	void Rotate()
	{
		transform.LookAt(target.transform);
	}
	void Attack()
	{
		if (isMelee)
		{
			target.Hurt(damage);
		}
		else
		{
			List<ShootData> SDList = attackPos.GetShootData(attackAng, DB);
			foreach (ShootData SD in SDList)
			{
				AmmoBase obj = GameManager.ctrl.objectPool.Reuse(ammo);
				if (SD.bowCount > 0 && !SD.isCycle)
				{
					if (obj != null)
					{
						obj.Reuse(SD.ShootPoint(60), SD.quat, Ammotype.Monster, damage);
					}
					else Instantiate(ammo, SD.ShootPoint(60), SD.quat).SetType(Ammotype.Monster,damage);
				}
				else if (SD.isCycle)
				{
					if (obj != null)
					{
						obj.Reuse(SD.ShootPoint(360), SD.quat, Ammotype.Monster, damage);
					}
					else Instantiate(ammo, SD.ShootPoint(360), SD.quat).SetType(Ammotype.Monster, damage);
				}
				else
				{
					if (obj != null)
					{
						obj.Reuse(SD.ShootPoint(), SD.quat, Ammotype.Monster, damage);
					}
					else
					{
						Instantiate(ammo, SD.ShootPoint(), SD.quat).SetType(Ammotype.Monster, damage);
					}
				}
			}
		}
	}
	/// <summary>
	/// 受到傷害(包含死亡處理)
	/// </summary>
	/// <param name="damage">傷害量</param>
	public void Hurt(float damage)
	{
		if (isDead) return;
		hp -= damage;
		hpBar.UpdateUI(hpPercent);
		if (hp <= 0)//死去
		{
			hp = 0;
			CancelInvoke("Attack");
			hpBar.UpdateUI(0);
			animator.SetType2(AnimaType.Death);
			//通知GM我屎了
			charCtrl.enabled = false;
			GameManager.ctrl.TargetSys.RemoveMonster(this);
			if(!boss)GameManager.ctrl.objectPool.GetRecycle(this);
			string thing = StageManager.ctrl.Drop(dropList, Random.Range(1, dropCount + 1));
            if (thing == Buffs.Heal)//如果掉下來的是血包
            {//丟到場上
				ItemForRecovery heartDrop = GameManager.ctrl.objectPool.Reuse(heart);
                if (heartDrop != null)
                {
					heartDrop.Reuse(transform.position, Quaternion.identity);
                }
                else
                {
					heartDrop = Instantiate(heart, transform.position, Quaternion.identity);
				}
				StageManager.ctrl.AddHeartToList(heartDrop);
				AudioManager.ctrl.PlaySFX("Heart_drop");
			}
			StageManager.ctrl.RemoveMonster(this);
			StageManager.ctrl.SetExp(exp);
			//StageManager.ctrl.CountMonster();
			if(boss)this.gameObject.SetActive(false);
		}
	}
    /// <summary>
    /// 重生怪物
    /// </summary>
    /// <returns>怪物</returns>
    public MonsterCtrl Reuse(Vector3 pos,Quaternion quat)
	{
		transform.position = pos;
		transform.rotation = quat;
		charCtrl.enabled = true;
		gameObject.SetActive(true);
		hp = hpMax;
		hpBar = UIHUDManager.ctrl.SetBarCtrl();
		GameManager.ctrl.TargetSys.AddMonster(this);
		//GameManager.ctrl.objectPool.Remove(this);
		target = PlayerCtrl.ctrl;
		return this;
	}
}