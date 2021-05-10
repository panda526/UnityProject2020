using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAmmoSelect : MonoBehaviour 
{
    public AmmoDB ammoDB;
    public void SetAmmoIndex(int index)
    {
        ammoDB.index = index;
    }
}
