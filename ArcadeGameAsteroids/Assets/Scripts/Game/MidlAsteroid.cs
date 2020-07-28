using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MidlAsteroid : Asteroid, IPoolable
{
    public override void OnDespawn()
    {
        if(gameManager.GetGameState() == GameState.Process)
        {
            gameManager.CreateAsteroid(2, gameManager.SmallAteroidPrefab, transform.position);
        }
        base.OnDespawn();
    }
}
