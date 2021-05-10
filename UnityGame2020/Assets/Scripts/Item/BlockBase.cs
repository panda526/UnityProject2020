using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBase : MonoBehaviour 
{
	public virtual void Reuse(Vector3 pos, Quaternion quat)
	{
		transform.position = pos;
		transform.rotation = quat;
	}
	public virtual void Recycle()
	{
		GameManager.ctrl.objectPool.GetRecycle(this);
	}
}
