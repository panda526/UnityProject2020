using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateTriggerCtrl : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			StageManager.ctrl.FadeOut();
		}
	}
}
