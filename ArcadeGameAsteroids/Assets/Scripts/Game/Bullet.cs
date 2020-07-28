using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : ObjectScene ,ITick ,ITickFixed, IPoolable
{
    [SerializeField]
    [Range(0.1f, 10f)] float BulletLifeTime = 1;

    Timer bulletLifeTime;

    void Start()
    {
        Init();
        OnSpawn();
    }

    public void Tick()
    {
        CheckPosition();
    }

    public void TickFixed()
    {
        SpeedLimit(body, MaxMoveSpeed);
    }

    public void BulletTimer()
    {
        bulletLifeTime = timerManager.CreateTimer(BulletLifeTime, () => { pool.Despawn(PoolType.Bullet, gameObject); });
    }

    public void OnSpawn()
    {
        transform.LookAt(gameManager.GetPlayer().transform);
        Move(transform.forward, Axeleration);//
        BulletTimer();
        PlaySound(source, Audio[0], 1);
        
    }

    public void OnDespawn()
    {
        ResetObject();
    }

    private void OnCollisionEnter(Collision collision)
    {
        timerManager.DeleteTimer(bulletLifeTime);
        pool.Despawn(PoolType.Bullet, gameObject);
    }
}
