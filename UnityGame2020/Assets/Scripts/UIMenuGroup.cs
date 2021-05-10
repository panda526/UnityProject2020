using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
[System.Serializable]
public struct MenuActionEvent
{
	public UICanvasGroupCtrl canvasGroupCtrl;
	public UnityEvent actionEvent;

	public void Switch(bool B)
	{
		canvasGroupCtrl.Switch(B);
		if (B) actionEvent.Invoke();
	}
}

public class UIMenuGroup : MonoBehaviour
{
	public MenuActionEvent[] objs;
	public Transform cameraTrans;
	public Vector3 orgPos;
	public Vector3 bias;
	void Start()
    {
		orgPos = cameraTrans.position;
    }
	public void ToggleSwitch(Toggle toggle)
	{
		AudioManager.ctrl.PlaySFX("UI_buttontoggle");
		//print(toggle.name + ":" + toggle.isOn);
		switch(toggle.name)
		{
			case "Toggle(角色)":
				objs[0].Switch(toggle.isOn);
				bias = Vector3.zero;
				break;
			case "Toggle(背包)":
				objs[1].Switch(toggle.isOn);
				bias = Vector3.zero;
				break;
			case "Toggle(地圖)":
				objs[2].Switch(toggle.isOn);
				bias = Vector3.right*5;
				break;
			case "Toggle(設定)":
				objs[3].Switch(toggle.isOn);
				break;
		}
		cameraTrans.position = orgPos + bias;
	}
}
