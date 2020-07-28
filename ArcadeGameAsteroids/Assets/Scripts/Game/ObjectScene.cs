using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;


[RequireComponent(typeof(Rigidbody), typeof(AudioSource))]
public abstract class ObjectScene : MonoBehaviour
{
    public bool isDead = false;
    [Space(10)]
    [SerializeField]
    protected AudioClip[] Audio;
    [Space(10)]

    [Header("Угол и скорость вращения")]
    [SerializeField]
    [Range(0f, 360f)] public float Rotation;
    [SerializeField]
    [Range(0, 500)] public float AngleSpeed = 1;

    [Header("Ускорение и максимальная скорость")]
    [SerializeField]
    [Range(0, 500)] public float Axeleration = 1;
    [SerializeField]
    [Range(0, 500)] public float MaxMoveSpeed = 1;

    protected GameManager gameManager;
    protected RegulatoryPools pool;
    protected TimerManager timerManager;
    protected Collider shipCollider;
    protected MeshRenderer render;
    protected Rigidbody body;
    protected AudioSource source;
    protected float timerTime;
    protected float bounds;
    protected float RightEdge;
    protected float LeftEdge;
    protected float TopEdge;
    protected float BottomEdge;
    protected bool activeObject = true;

    

    public virtual void Move(Vector3 direction, float speed)
    {
        body.AddForce(direction * speed);
    }

    public virtual void SpeedLimit(Rigidbody rigidbody, float maxSpeed)
    {
        if (rigidbody.velocity.magnitude > maxSpeed)
        {
            rigidbody.velocity = rigidbody.velocity.normalized * maxSpeed; 
        }
    }

    public virtual void SpeedMin(Rigidbody rigidbody, float Speed)
    {
        if (rigidbody.velocity.magnitude < Speed)
        {
            rigidbody.velocity = rigidbody.velocity.normalized * Speed;
        }
    }

    public virtual void AssignRotation(float angle)
    {
        if (Rotation > 360) Rotation = 0;
        else if (Rotation < 0) Rotation = 360;

        Quaternion target = Quaternion.Euler(0, angle, 0);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, target, Time.deltaTime * AngleSpeed);
    }
    
    public virtual float BoundsCalculate(GameObject obj)
    {
        return obj.GetComponent<MeshFilter>().sharedMesh.bounds.size.x/2;
    }

    public virtual void Init()
    {
        CoreTools.GetManager<Updater>().AddTo(this);
        body = GetComponent<Rigidbody>();
        gameManager = CoreTools.GetManager<GameManager>();
        bounds = BoundsCalculate(gameObject);
        SetScreenBoundaries(bounds);
        render = GetComponent<MeshRenderer>();
        shipCollider = GetComponent<Collider>();
        source = GetComponent<AudioSource>();
        pool = CoreTools.GetManager<RegulatoryPools>();
        timerManager = CoreTools.GetManager<TimerManager>();
    }

    public virtual void SetScreenBoundaries(float bounds)
    {
        RightEdge = gameManager.sceneRightEdge + bounds;
        LeftEdge = gameManager.sceneLeftEdge - bounds;
        TopEdge = gameManager.sceneTopEdge + bounds;
        BottomEdge = gameManager.sceneBottomEdge - bounds;
    }

    public virtual void ResetObject()
    {
        body.velocity = new Vector3(0f, 0f, 0f);
        body.angularVelocity = new Vector3(0f, 0f, 0f);
        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
        transform.position = new Vector3(0f, 0f, 0f);
    }

    public virtual void CheckPosition()
    {
        if (transform.position.x > RightEdge)
        {
            transform.position = new Vector3(LeftEdge, transform.position.y, transform.position.z);
        }
        if (transform.position.x < LeftEdge)
        {
            transform.position = new Vector3(RightEdge, transform.position.y, transform.position.z);
        }
        if (transform.position.z > TopEdge)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, BottomEdge);
        }
        if (transform.position.z < BottomEdge)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, TopEdge);
        }
    }

    public virtual void SetActive(bool state)
    {
        if (state == true)
        {
            render.enabled = true;
            body.WakeUp();
            shipCollider.enabled = true;
            source.enabled = true;
            activeObject = true;
        }
        else if (state == false)
        {
            render.enabled = false;
            body.Sleep();
            shipCollider.enabled = false;
            source.enabled = false;
            activeObject = false;
        }
    }

    public virtual void PlaySound(AudioSource Source,AudioClip clip, float volume)
    {
        if(gameManager.GetGameState() == GameState.Process)
        {
            Source.clip = clip;
            Source.volume = volume;
            Source.Play();
        }
    }
}
