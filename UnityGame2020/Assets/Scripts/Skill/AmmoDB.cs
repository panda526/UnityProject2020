using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "AmmoDB", menuName = "DB/AmmoDB", order = 5)]

public class AmmoDB : ScriptableObject
{
    public int index;//序號
    public List<AmmoBase> ammoList;
    public AmmoBase GetAmmo()
    {
        return ammoList[index];
    }
}
