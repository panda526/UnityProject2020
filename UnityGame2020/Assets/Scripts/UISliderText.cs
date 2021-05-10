using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UISliderText : MonoBehaviour
{
    public Slider BGMSlider;
    public Slider SFXSlider;
    public Text BGMText;
    public Text SFXText;
    private bool isMute = false;
    
    void Start()
    {
        BGMSlider.value = PlayerPrefs.GetFloat("BGM_Val",0.5f);
        SFXSlider.value = PlayerPrefs.GetFloat("SFX_Val",0.5f);
    }
    /// <summary>
    /// 使數值隨著卷軸變動
    /// </summary>
    /// <param name="value">數值</param>
    public void ChangeBGMValue(float value)
    {
        AudioManager.ctrl.BGMValueCtrl(value);
        int IntValue =(int)(value*100);
        BGMText.text = IntValue.ToString("0");
        PlayerPrefs.SetFloat("BGM_Val", value);
    }
    public void ChangeSFXValue(float value)
    {
        AudioManager.ctrl.SFXValueCtrl(value);
        int IntValue =(int)(value*100);
        SFXText.text = IntValue.ToString("0");
        PlayerPrefs.SetFloat("SFX_Val", value);
    }
    /// <summary>
    /// 靜音紐被按下時
    /// </summary>
    public void MuteChange()
    {
        isMute = !isMute;//靜音/解除靜音
        BGMSlider.value = isMute ? 0 : PlayerPrefs.GetFloat("BGM_Val"); ;//如果靜音則歸零，反靜音則回到中間
        SFXSlider.value = isMute ? 0 : PlayerPrefs.GetFloat("SFX_Val"); ;//同上
        AudioManager.ctrl.BGMValueCtrl(isMute ? 0 : PlayerPrefs.GetFloat("BGM_Val"));
        AudioManager.ctrl.SFXValueCtrl(isMute ? 0 : PlayerPrefs.GetFloat("SFX_Val"));
        BGMSlider.interactable = !BGMSlider.interactable;
        SFXSlider.interactable = !SFXSlider.interactable;
    }
}