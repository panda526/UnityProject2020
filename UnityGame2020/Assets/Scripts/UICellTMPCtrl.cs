using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICellTMPCtrl : MonoBehaviour
{
    public Image itemImg;
    public void SetData(Sprite sp)
    {
		if (sp == null) return;
        itemImg.sprite = sp;
		gameObject.SetActive(true);
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
