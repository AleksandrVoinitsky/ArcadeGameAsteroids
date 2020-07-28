using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFO : ObjectScene, ITick, ITickFixed
{

    [SerializeField] private int Score = 0;

    private bool timerFlag = false;

    public void TickFixed()
    {
        SpeedLimit(body, MaxMoveSpeed);
        AssignRotation(Rotation);
    }

    public void Tick()
    {
        CheckPosition();
        Shoot();
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        PlaySound(source, Audio[0], collision.impulse.magnitude);
        gameManager.DeadUfo(Score);
    }

    public void SetUfoPosition()
    {
        float limit = gameManager.sceneHeight/2 / 100 * 20; // 
        Vector3 newPos = new Vector3(LeftEdge, 0, Random.Range(limit, Random.Range(-gameManager.sceneHeight / 2 + limit, gameManager.sceneHeight / 2 - limit)));
        transform.position = newPos;
    }

    public void SummonsUfo()
    {
        switch (Random.Range(0,2))
        {
            case 0:
                Move(Vector3.right, Axeleration);
                break;
            case 1:
                Move(-Vector3.right, Axeleration);
                break;
        }
        isDead = false;
    }

    public void Shoot()
    {
        if (timerFlag && !isDead)
        {
            pool.Spawn(PoolType.Bullet, gameManager.UfoBulletPrefab, transform.position, transform.rotation, null);
            StartTimer(Random.Range(2,5));
        }
    }

    public virtual void StartTimer(float time)
    {
        timerTime = time;
        timerFlag = false;
        Timer flagTimer = timerManager.CreateTimer(time, () => { timerFlag = true; }); 
    }
}
