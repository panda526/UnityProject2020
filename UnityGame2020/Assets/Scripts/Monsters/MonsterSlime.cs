using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSlime : MonsterCtrl 
{
    public override void SpacialAction()
    {
        if (boss)
        {
            InvokeRepeating("Rage", 5f, 10f);
            InvokeRepeating("Calm", 10f, 10f);
        }
    }

    public void Rage()
    {
        attackSpeed = 4f;
        timer.ChangeTimerDuration(atkCoolDown);
        DB.Cycle = 8;
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
