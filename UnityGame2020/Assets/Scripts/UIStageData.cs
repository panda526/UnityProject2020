using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIStageData : MonoBehaviour
{
    //設立變數
    public Image iconImage;
    public Text enterStageText;
    public Text descText;
    public Text titleText;
    public Image symbolImg;
    public StageData stageData;
    /// <summary>
    /// 從Stagedata拿來變數並指定
    /// </summary>
    /// <param name="stageData">關卡資料</param>
    public void SetData(StageData stageData)
    {
        gameObject.SetActive(true);
        this.stageData.name = stageData.name;
        this.stageData.staminaCost = stageData.staminaCost;
        this.stageData.icon = stageData.icon; 
        this.stageData.symbol = stageData.symbol;
        this.stageData.description = stageData.description;
        this.stageData.stageID = stageData.stageID;
        enterStageText.text="消耗"+this.stageData.staminaCost+"點體力";
        titleText.text=this.stageData.name;
        descText.text = this.stageData.description;
        iconImage.sprite = this.stageData.icon;
        symbolImg.sprite = this.stageData.symbol;
    }
    public void EnterStage()
    {
        if (!GameManager.ctrl.PlayerInfoSys.SetStamina(-stageData.staminaCost)) return;
        SceneManager.LoadScene(stageData.stageID);
    }
}
