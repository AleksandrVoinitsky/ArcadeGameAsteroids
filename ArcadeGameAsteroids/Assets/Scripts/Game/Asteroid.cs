using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;

public abstract class Asteroid : ObjectScene, ITick, ITickFixed, IPoolable
{
    public int Score = 0;
    
    public void Start()
    {
        Init();
        OnSpawn();
        
    }

    public void SetRandonStat()
    {
        Rotation = Random.Range(0, 360);
        Axeleration = Random.Range(1, Axeleration);
        MaxMoveSpeed = Random.Range(1, MaxMoveSpeed);
    }

    public void Tick()
    {
        CheckPosition();
    }

    public void TickFixed()
    {
        SpeedLimit(body, MaxMoveSpeed);
        SpeedMin(body, Axeleration);
        AssignRotation(Rotation);
        Move(transform.forward, Axeleration);
    }

    public void OnSpawn()
    {
        isDead = false;
        SetRandonStat();
    }

    public virtual void OnDespawn()
    {
        ResetObject();
    }


    public void Despawn(GameObject asteroid = null)
    {
        if (gameManager.GetGameState() == GameState.Process)
        {
            if(asteroid != null)
            {
                Asteroid ast1 = pool.Spawn(PoolType.Ateroids, asteroid, transform.position).GetComponent<Asteroid>();
                Asteroid ast2 = pool.Spawn(PoolType.Ateroids, asteroid, transform.position).GetComponent<Asteroid>();
                float Axelerate = Random.Range(Axeleration / 2, Axeleration);
                ast1.Rotation = this.Rotation + 45;
                ast2.Rotation = this.Rotation - 45;
                ast1.Axeleration = Axelerate; 
                ast2.Axeleration = Axelerate;
                gameManager.AddAsteroid(2);
            }
            gameManager.RemoveAsteroid(Score, gameObject);
        }
    }
}
