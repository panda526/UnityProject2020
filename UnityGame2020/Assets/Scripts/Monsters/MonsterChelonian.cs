using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterChelonian : MonsterCtrl 
{
    public override void SpacialAction()
    {
        if (boss)
        {
            InvokeRepeating("Rage", 5f, 7f);
            InvokeRepeating("Calm", 10f, 7f);
        }
    }

    public void Rage()
    {
        speed = 10f;
        attackSpeed = 5f;
        timer.ChangeTimerDuration(atkCoolDown);
        CancelInvoke("Attack");
    }
    public void Calm()
    {
        attackSpeed = 1f;
        timer.ChangeTimerDuration(atkCoolDown);
        DB.Cycle = 0;
        CancelInvoke("Attack");
    }
}
