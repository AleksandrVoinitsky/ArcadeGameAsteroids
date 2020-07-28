using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;

public class Asteroid : ObjectScene, ITick, ITickFixed, IPoolable
{
    public int Score = 0;
    
    public void Start()
    {
        Init();
        OnSpawn(); 
    }

    void SetRandonStat()
    {
        transform.rotation = Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
        Axeleration = Random.Range(1, Axeleration);
        MaxMoveSpeed = Random.Range(1, MaxMoveSpeed);
        Move(transform.forward, Axeleration);
    }

    public void Tick()
    {
        CheckPosition();
    }

    public void TickFixed()
    {
        SpeedLimit(body, MaxMoveSpeed);
        SpeedMin(body, Axeleration);
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

    private void OnCollisionEnter(Collision collision)
    {
        PlaySound(source,Audio[0], collision.impulse.magnitude);

        if(collision.transform.tag != "Asteroid" && !isDead)
        {
            isDead = true;
            Despawn();
        }
    }

    public void Despawn()
    {
        if (gameManager.GetGameState() == GameState.Process)
        {
            gameManager.RemoveAsteroid(Score);
        }
        Timer DeadTime = timerManager.CreateTimer(0.3f, () => { pool.Despawn(PoolType.Ateroids, gameObject);});
        //pool.Despawn(type, gameObject);
    }

}
