using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
#region 控制器系列(玩家控制、GM、面對方向、動畫、關注位置)
public class PlayerCtrl : UIPosForHUD {
	/// <summary>
	/// static類型，代表唯一的自己
	/// </summary>
	public static PlayerCtrl ctrl;
	/// <summary>
	/// 回傳面向方向
	/// </summary>
	private Vector2 m_faceDirection;
	public Vector2 faceDirection 
	{
		get
		{
			m_faceDirection.x = Input.GetAxis("Horizontal");
			m_faceDirection.y = Input.GetAxis("Vertical");
			return m_faceDirection;
		}
	}
	/// <summary>
	/// 回傳控制器(玩家)
	/// </summary>
	private CharacterController m_charCtrl;
	public CharacterController charCtrl
	{
		get
		{
			if (m_charCtrl == null) m_charCtrl = GetComponentInChildren<CharacterController>();
			return m_charCtrl;
		}
	}
	/// <summary>
	/// 回傳動畫控制器
	/// </summary>
	private AnimationCtrl m_animator;
	private AnimationCtrl animator
	{
		get
		{
			if (m_animator == null) m_animator =GetComponentInChildren<AnimationCtrl>();
			return m_animator;
		}
	}
	/// <summary>
	/// 回傳攝影機的位置
	/// </summary>
	private Vector3 m_focusPos;
	private Vector3 focusPos
	{
		get
		{
			m_focusPos = transform.position;
			//Mathf.Clamp(值,最小,最大) 放入的值如果超出範圍就會被鎖在最小/最大值
			m_focusPos.x = Mathf.Clamp(m_focusPos.x, -(mapW / 2 - 12.5f), (mapW / 2 - 12.5f));
			m_focusPos.z = Mathf.Clamp(m_focusPos.z, -(mapH / 2 - 9), (mapH / 2 - 9));
			return m_focusPos;
		}
	}
	#endregion
	[Header("基本參數")]
	public float hp;
	public float hpMax;
	public float hpPercent 
	{ 
		get 
		{ 
			return hp / hpMax; 
		} 
	}

	public int damage = 100;
	public bool isDead
    {
		get { return hp <= 0; }
    }
	[Tooltip("移動速度")]
	[Range(1f, 20f)]
	public float speed = 15f;
	[Header("攻擊參數")]
	[Tooltip("攻擊速度")]
	[Range(0.5f, 3f)]
	public float attackSpeed = 3f;
	
    #region 設定計時器
    private Timer m_timer;
	private Timer timer
	{
		get
		{
			if (m_timer == null) m_timer = new Timer(1f / attackSpeed);
			return m_timer;
		}
	}
	#endregion
	public AmmoDB ammoDB;
    public AmmoBase ammo { get { return ammoDB.GetAmmo(); } }
	public MonsterCtrl target
    {
        get { return GameManager.ctrl.TargetSys.Search(transform)!=null? GameManager.ctrl.TargetSys.Search(transform):null; }
    }
	//地圖的長寬
	public float mapH = 22;
	public float mapW = 52;
	public bool isMoving { get { return (faceDirection != Vector2.zero); } }
	private Vector3 attackPos;
	private float attackAng;
	private DirectionBullet DB;
	// Use this for initialization
	void Start () 
	{
		if (GetHpBar())
		{
			hpMax = 600f + GM.BuffSys.GetHPBuff();
			hpBar.UpdatePos(GetScreenPos());
		}
		hpMax = 600f + GM.BuffSys.GetHPBuff();
		hp = hpMax;
		animator.SetAttackSpeed(attackSpeed);
		//hpBar = UIHUDManager.ctrl.SetBarCtrl();
		UpdateBuff();
	}
	public void UpdateBuff()
    {
		DB.Front = GM.BuffSys.GetBuffAmount(Buffs.FrontShoot);
		DB.Side = GM.BuffSys.GetBuffAmount(Buffs.SideShoot);
		DB.Back = GM.BuffSys.GetBuffAmount(Buffs.BackShoot);
		DB.Bow = GM.BuffSys.GetBuffAmount(Buffs.BowShoot);
		//DB.Cycle = GM.BuffSys.GetBuffAmount(Buffs.CycleShoot);
		damage = 80 + GM.BuffSys.GetAttackBuff();
		if (GetHpBar())
        {
			hpMax = 600f + GM.BuffSys.GetHPBuff();
			hpBar.UpdatePos(GetScreenPos());
		} 
	}
	void Awake()
	{
		ctrl = this;
	}
	// Update is called once per frame
	void Update () {
		if (isDead) return;
		if(GetHpBar()) hpBar.UpdatePos(GetScreenPos());
		if (isMoving)
		{
			animator.SetType(AnimaType.Move);
			Move();
			Rotate();
		}
		else if (target && !target.isDead)
		{
			transform.LookAt(target.transform);
			if (timer.Update(Time.deltaTime))
			{
				animator.SetType(AnimaType.Attack);
				attackPos=transform.position;
				attackAng = Vector3.Angle(Vector3.forward, transform.forward)
					* Mathf.Sign(transform.forward.x);
				Invoke("Attack", time: 0.65f * (1f / attackSpeed));
				timer.Start();
			}
		}
        else
        {
			animator.SetType(AnimaType.Idle);
		}
		if(Input.GetKey(KeyCode.Space))
		{
			CameraSystem.ctrl.UpdatePos(target.transform.position);
		}
		else CameraSystem.ctrl.UpdatePos(focusPos);
	}
	public void ResetPos(Transform point)
    {
		transform.position = point.position;
		transform.rotation = point.rotation;
    }
    void Move() 
	{
		charCtrl.SimpleMove(transform.forward * speed);
	}

	void Rotate()
	{
		float angle =Vector2.Angle(Vector2.up,faceDirection)*Mathf.Sign(faceDirection.x);
		transform.rotation = Quaternion.AngleAxis(angle, Vector3.up);
	}

	
	void Attack()
	{
		AudioManager.ctrl.PlaySFX("Shoot");
		List<ShootData> SDList = attackPos.GetShootData(attackAng, DB);
		foreach (ShootData SD in SDList)
		{
			AmmoBase obj = GameManager.ctrl.objectPool.Reuse(ammo);
			if (SD.bowCount > 0 && !SD.isCycle)
			{
				if (obj != null)
				{
					obj.Reuse(SD.ShootPoint(60), SD.quat, Ammotype.Player, damage);
				}
				else Instantiate(ammo, SD.ShootPoint(60), SD.quat).SetType(Ammotype.Player, damage);
			}
			else if (SD.isCycle)
			{
				if (obj != null)
				{
					obj.Reuse(SD.ShootPoint(360), SD.quat, Ammotype.Player, damage);
				}
				else Instantiate(ammo, SD.ShootPoint(360), SD.quat).SetType(Ammotype.Player, damage);
			}
			else
			{
				if (obj != null)
				{
					obj.Reuse(SD.ShootPoint(), SD.quat, Ammotype.Player, damage);
				}
				else
				{
					Instantiate(ammo, SD.ShootPoint(), SD.quat).SetType(Ammotype.Player, damage);
				}
			}
		}
		/*
	Instantiate(ammo, transform.ShootPosition(Direction.Top, 1.3f), transform.ShootRotation(Direction.Top, attackAng));
	Instantiate(ammo, transform.ShootPosition(Direction.Top, 1.3f), transform.ShootRotation(Direction.Top, attackAng+45));
	Instantiate(ammo, transform.ShootPosition(Direction.Top, 1.3f), transform.ShootRotation(Direction.Top, attackAng-45));
	Instantiate(ammo, transform.ShootPosition(Direction.Right, 1.3f), transform.ShootRotation(Direction.Right, attackAng));
	Instantiate(ammo, transform.ShootPosition(Direction.Left, 1.3f), transform.ShootRotation(Direction.Left, attackAng));
	Instantiate(ammo, transform.ShootPosition(Direction.Down, 1.3f), transform.ShootRotation(Direction.Down, attackAng));
	*/
	}
	public void Hurt(float damage)
	{
		if (isDead) return;
		hp -= damage;
		if (GetHpBar()) hpBar.UpdateUI(hpPercent);
		if (hp <= 0)
		{
			hp = 0;
			animator.SetType(AnimaType.Death);
			AudioManager.ctrl.PlaySFX("Gameover");
			StageManager.ctrl.GameOver();
		}
	}
}
