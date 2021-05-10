using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class CameraSystem : MonoBehaviour {

	public static CameraSystem ctrl;
	public bool autoUpdate=true;
	public Transform focusTarget { get; private set; }
	public Transform target { get; private set; }
	private Vector3 targetPos;
	[Header("距離設定")]
	public float distance = 5f;
	[Header("速度設定")]
	[Range(0f, 5f)]
	public float followSpeed = 1f;
	[Header("角度設定")]
	[Range(15f,85f)]
	public float angX = 0f;
	[Range(0f, 360f)]
	public float angY = 0f;

	public Vector3 angXY;
    public Quaternion ang
	{
		get
		{
			angXY.x = angX;
			angXY.y = angY;
			return Quaternion.Euler(angXY);
		}
	}

	void Awake()
	{
		ctrl = this;
	}
	// Use this for initialization
	void Start () {
		target = new GameObject("CameraTarget").transform;
		transform.SetParent(target);
		LookTarget();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(autoUpdate)
		{
			//計算新的座標位置(與焦點的距離)
			LookTarget();
			if (focusTarget) UpdatePos();
		}
	}
	public void LookTarget()
	{
		transform.position = Follow();
		transform.LookAt(target);//盯著目標
	}
	public void UpdatePos(Vector3 pos)
	{
		targetPos = pos;
		//使用LERP線性差值，做跟隨的緩衝(視焦點和目標物)
		target.position = Vector3.Lerp(target.position, targetPos, Time.deltaTime * followSpeed);
	}
	public void UpdatePos()
	{
		//使用LERP線性差值2，做跟隨的緩衝(視焦點和目標物)
		target.position = Vector3.Lerp(target.position, focusTarget.position, Time.deltaTime * followSpeed);
	}
	Vector3 Follow()
	{
		//計算某角度+離他多遠的算式
		return target.position + Angle() * Distance();//角度 * 方向向量 = 一個新的座標
	}
	Vector3 Distance()
	{
		return  Vector3.back * distance;//玩家後方某個點
	}

	Quaternion Angle()
	{
		//角度
		return ang;
	}
}
