using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiganticBombItem : ItemBase
{
    [SerializeField] private Explosion giganticExplosionPrefab_;

    public override void Get()
    {
        Instantiate(giganticExplosionPrefab_, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}