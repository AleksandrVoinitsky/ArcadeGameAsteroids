﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallAsteroid : Asteroid, IPoolable
{
    public void OnCollisionEnter(Collision collision)
    {
        PlaySound(source, Audio[0], collision.impulse.magnitude);

        if (collision.transform.tag != "Asteroid" && !isDead)
        {
            isDead = true;
            Despawn();
        }
    }
}
    

