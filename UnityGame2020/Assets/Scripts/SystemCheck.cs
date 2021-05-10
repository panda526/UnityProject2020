using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemCheck : ISystem 
{
	public DataBaseManager dataBaseManager;
	// Use this for initialization
	void Start () 
	{
		Invoke("RunCheck", 0.2f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void RunCheck()
    {
        //Debug.Log(GM != null ? "System running ." : "System restart .");
		if(UIPlayerInfoCtrl.ctrl) UIPlayerInfoCtrl.ctrl.Initial();
		//UIMapCtrl.ctrl.Initial();
		//找尋場上特定類別的物件
		if (!FindObjectOfType(typeof(DataBaseManager)))
		{
			Instantiate(dataBaseManager);
		}
	}
}
