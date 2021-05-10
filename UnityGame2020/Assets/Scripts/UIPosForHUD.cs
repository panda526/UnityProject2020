using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPosForHUD : ISystem
{
    protected UIHPBarCtrl hpBar;
    public Vector3 pos
    {
        get 
        { return transform.position; }
    }

    public Vector2 GetScreenPos()
    {
        Vector2 screenPos = Vector2.zero;
        screenPos = Camera.main.WorldToViewportPoint(pos);
        return screenPos;
    }
    public bool GetHpBar()
    {
        if (UIHUDManager.ctrl != null && hpBar == null) hpBar = UIHUDManager.ctrl.SetBarCtrl();
        return hpBar != null;
    }
}
