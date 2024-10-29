using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ExplosionPillar : Explosion
{
    [SerializeField] float maxWidth_ = 3.0f;
    protected override void ScaleUp()
    {
        Vector3 newScale = transform.localScale;
        newScale.x = maxWidth_ * taimeRatio_;
        transform.localScale = newScale;
    }
}
