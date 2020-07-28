using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigAsteroid : Asteroid, IPoolable
{
    public override void OnDespawn()
    {
        if (gameManager.GetGameState() == GameState.Process)
        {
            gameManager.CreateAsteroid(2, gameManager.MidSateroidPrefab, transform.position);
        }
        base.OnDespawn();

    }

    
}
