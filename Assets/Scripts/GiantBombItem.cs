using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantBombItem : ItemBase
{
    [SerializeField] Explosion giantExplosionPrefab_;
    public override void Get()
    {
        Instantiate(giantExplosionPrefab_, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
