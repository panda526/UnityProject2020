using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 abstract是抽象類型
 用於class上
 可使用
 1.一般方法
 2.abtract方法(不可先寫內容，覆寫後定義)
 3.virtual方法(可先寫內容，覆寫後可保留或修改原定義)
 */
 public enum Ammotype{Player,Monster};
[RequireComponent(typeof(SphereCollider))]
public abstract class AmmoBase : MonoBehaviour {
	[Header("基本參數")]
	[Tooltip("子彈行進速度")]
	public float speed= 5.0f;
	public float damage = 100;
	[Header("攻擊次數")]
	public int hitCount = 1;
	public bool canPenetrate { get { return hitCount >= 1; } }
	public Ammotype ammotype;
	// Use this for initialization
	[Header("有效範圍")]
	public float maprange = 30f;
	public bool over
	{
		get { return Vector3.Distance(Vector3.zero, transform.position) > maprange; }
	}
	
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		Move();
	}
	//決定子彈是誰的
	public void SetType(Ammotype type,int damage)
	{
		ammotype = type;
		this.damage = damage;
	}
	/// <summary>
	/// 摧毀事件
	/// </summary>
	void Destroy()
	{
		GameManager.ctrl.objectPool.GetRecycle(this);
	}
	//穿透事件，若子彈穿透次數用完則摧毀(回收)
	void PenetrateEvent()
	{
		hitCount--;
		if (!canPenetrate) Destroy();
	}
	//被回收了
	public void Recycle(Transform objPool)
	{
		transform.SetParent(objPool);
		gameObject.SetActive(false);
	}
	//重新獲得參數(回收再利用)
	public void Reuse(Vector3 pos,Quaternion quat,Ammotype type,int damage)
	{
		//Debug.Log("回收喔喔喔喔喔");	
		transform.SetParent(null);
		gameObject.SetActive(true);
		transform.position = pos;
		transform.rotation = quat;
		SetType(type,damage);
	}
	public virtual void Move()
	{
		transform.Translate(Vector3.forward * Time.deltaTime * speed);
		if (over) Destroy();
	}
	//碰撞事件
	private void OnTriggerEnter(Collider other)
	{
		switch (ammotype)
		{
			case Ammotype.Player:
				if (other.tag == "Monster")
				{
					other.GetComponent<MonsterCtrl>().Hurt(damage);
                    PenetrateEvent();
				}
				break;
			case Ammotype.Monster:
				if (other.tag == "Player")
				{
					other.GetComponent<PlayerCtrl>().Hurt(damage);
                    PenetrateEvent();
                }
				break;
        }
		if(other.tag == "Obstacle")
		{
			PenetrateEvent();
		}
	}
}
