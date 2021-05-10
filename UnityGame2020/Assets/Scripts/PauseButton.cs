using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseButton : MonoBehaviour
{
	/// <summary>
	/// 如果為0就變成1，如果不是0就變成0。
	/// </summary>
	public void ZAWARUDO()
	{
		Time.timeScale = Time.timeScale==0?1:0;
	}
	
}
