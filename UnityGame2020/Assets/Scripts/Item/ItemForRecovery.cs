using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemForRecovery : MonoBehaviour {
	public int val = 120;
	private BuffData buffdata;
	public EffectBase geteffect;
    // Use this for initialization
    void Start () {
		buffdata = new BuffData(val, -1, -1);
	}
	
	void Destroy()
    {
		Destroy(gameObject);
	}
	// Update is called once per frame
	void OnTriggerEnter(Collider other)
	{
        if (other.tag == "Player")
        {
			UseItem();
		}
	}
	public void UseItem()
    {
		PlayerCtrl.ctrl.Hurt(-buffdata.number);
		StageManager.ctrl.RemoveHeartFromList(this);
		SpawnGetEffect();
		GameManager.ctrl.objectPool.GetRecycle(this);	
	}

	public void Reuse(Vector3 pos, Quaternion quat)
	{
		transform.position = pos;
		transform.rotation = quat;
		transform.SetParent(null);
		gameObject.SetActive(true);
		buffdata = new BuffData(val, -1, -1);
	}
	public void SpawnGetEffect()
    {
		EffectBase effect = GameManager.ctrl.objectPool.Reuse(geteffect);
        if (effect != null)
        {
			effect.Reuse(transform.position, transform.rotation);
        }
		else
		{
			//沒找到 直接生成
			Instantiate(geteffect, transform.position, transform.rotation);
		}
	}
}
