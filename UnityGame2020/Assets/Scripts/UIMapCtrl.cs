using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMapCtrl : MonoBehaviour
{
    public static UIMapCtrl ctrl;
    private int stageCount;
    private int index = 0;
    public RectTransform stageRT;
    public UIStageData stageTMP;
    public bool isCycle = true;
    private void Awake()
    {
        ctrl = this;
    }
    public void Initial()
    {
        stageCount = DataBaseManager.ctrl.stageDB.stageDatas.Count;
        stageRT.sizeDelta = new Vector2(stageCount * 1600,0);
        for (int i = 0; i < stageCount; i++)
            Instantiate(stageTMP, stageRT).SetData(DataBaseManager.ctrl.stageDB.stageDatas[i]);
    }
    public void Left()
    {
        index--;
        if (index < 0) index = isCycle ? stageCount-1 : 0;
        stageRT.anchoredPosition = new Vector2(-1600 * index, 0);
    }
    public void Right()
    {
        index++;
        if (index >= stageCount) index = isCycle ? 0 : stageCount - 1;
        stageRT.anchoredPosition = new Vector2(-1600 * index, 0);
    }
}
