using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;

public class Ship : ObjectScene, ITick, ITickFixed
{
    [Header("Колличество и скороть мигания ")]
    [SerializeField]
    [Range(1,10)] private int BlinkAmount = 3; 
    [SerializeField]
    [Range(0.01f,1f)] private float BlinkRate = 0.5f; 

    [Header("Скорострельность/с")]
    [SerializeField]
    [Range(0.1f, 1f)] private float ShotAmount = 0.1f;

    private bool blinkFlag = false;
    private int BlinkCounter = 0; 
    private GameObject bullet;
    private bool timerFlag = true;

    public override void Init()
    {
        base.Init();
        bullet = gameManager.PlayerBulletPrefab;
    }

    public void Tick()
    {
        CheckPosition();
        BlinkShip();
    }

    public void TickFixed()
    {
        SpeedLimit(body, MaxMoveSpeed);
        AssignRotationSmooth(Rotation);
    }

    public void ShipControl(InputKeys direction, float angle = -1) 
    {
        if (direction == InputKeys.up) { Move(transform.forward, Axeleration); }

        if (direction == InputKeys.shoot) { Shoot(); }

        if(angle != -1) { Rotation = angle; }
        else if (direction == InputKeys.left) { Rotation -= 1 * Time.deltaTime * AngleSpeed; }
        else if (direction == InputKeys.right) { Rotation += 1 * Time.deltaTime * AngleSpeed; }
    }

    private void OnCollisionEnter(Collision collision)
    {
        PlaySound(source, Audio[0], collision.impulse.magnitude);
        gameManager.TakeLife();
    }

    public void Shoot()
    {
        if (timerFlag)
        {
            pool.Spawn(PoolType.Bullet, bullet,transform.position, transform.rotation, null);
           StartTimer(ShotAmount);
        }
    }

    public void Blink()
    {
        blinkFlag = true;
        BlinkCounter = 0;
    }

    public void BlinkShip()
    {
        if (blinkFlag && BlinkCounter < BlinkAmount)
        {
            shipCollider.enabled = false;
            if (render.enabled)
            {
                render.enabled = false;
            }
            else
            {
                render.enabled = true;
                BlinkCounter++;
            }
            blinkFlag = false;
            StartCoroutine("ExecuteStart");
        }

        if (BlinkCounter == BlinkAmount)
        {
            shipCollider.enabled = true;
        }
    }

    IEnumerator ExecuteStart()
    {
        yield return new WaitForSeconds(BlinkRate);
        blinkFlag = true;
    }

    public virtual void StartTimer(float time)
    {
        timerTime = time;
        timerFlag = false;
        Timer flagTimer = timerManager.CreateTimer(time, () => { timerFlag = true; });
    }
}
