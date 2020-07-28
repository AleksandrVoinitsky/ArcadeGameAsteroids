using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallAsteroid : Asteroid, IPoolable
{
   public override void OnDespawn()
    {
        base.OnDespawn();
    }
}
    

