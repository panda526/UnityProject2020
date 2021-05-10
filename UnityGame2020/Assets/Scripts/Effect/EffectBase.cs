using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EffectBase : MonoBehaviour {

	public float delay = 2f;
	public void Start()
	{
		Invoke("Recycle", delay);
	}
	public virtual void Reuse(Vector3 pos, Quaternion quat)
	{
		transform.position = pos;
		transform.rotation = quat;
		gameObject.SetActive(true);
	}
	public virtual void Recycle()
	{
		GameManager.ctrl.objectPool.GetRecycle(this);
	}
}
