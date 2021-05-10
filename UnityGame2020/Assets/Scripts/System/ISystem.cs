using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ISystem : MonoBehaviour {

	private static GameManager m_GM; //資料實體
	public GameManager GM //資料接口
	{
		get //只呼叫一次 優化效能
		{
			if (GameManager.ctrl == null) m_GM = new GameManager();
			return m_GM;
		}
	}
	public void SystemCheck()
	{
		Debug.Log(GM != null ? "GM is running" : "GM is restart");
		if (EventSystem.current == null) SceneManager.LoadScene("GameUI", LoadSceneMode.Additive);
	}
}
