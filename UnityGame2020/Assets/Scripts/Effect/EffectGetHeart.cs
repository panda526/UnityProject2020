using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectGetHeart : EffectBase
{
    public override void Reuse(Vector3 pos, Quaternion quat)
    {
        base.Reuse(pos, quat);
        Invoke("Recycle", delay);
    }
}