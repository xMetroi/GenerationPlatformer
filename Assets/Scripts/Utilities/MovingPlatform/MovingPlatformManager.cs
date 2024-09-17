using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformManager : MonoBehaviour
{
    [Header("Platform Settings")]
    [SerializeField] private float transformSpeed;
    [SerializeField] private Transform[] wayPoints;
    [SerializeField] private float OnPlatformPlayerGravityScale = 50;
    Vector3 targetPos;
    Vector3 moveDirection;
    Rigidbody2D rb;
    private float initialPlayerGravityScale;

    int actualPoint;
    int pointCount;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {      
        Initialize();
    }

    private void Update()
    {
        if (Vector2.Distance(transform.position, targetPos) < 0.05f)
        {
            NextPoint();
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = moveDirection * transformSpeed;
    }

    private void Initialize()
    {
        pointCount = wayPoints.Length;
        actualPoint = 1;
        targetPos = wayPoints[1].position;
        DirectionCalculate();
    }

    void NextPoint()
    {
        transform.position = targetPos;

        if (actualPoint != pointCount - 1) 
        {
            actualPoint++;
            targetPos = wayPoints[actualPoint].position;
        }
        else
        {
            actualPoint = 0;
            targetPos = wayPoints[actualPoint].position;
        }     

        DirectionCalculate();
    }

    void DirectionCalculate()
    {
        moveDirection = (targetPos - transform.position).normalized;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerMovement playerMovement = collision.GetComponent<PlayerMovement>();
            Rigidbody2D rigidbody2D = collision.GetComponent<Rigidbody2D>();

            initialPlayerGravityScale = rigidbody2D.gravityScale;

            playerMovement.isOnPlatform = true;
            playerMovement.platformRb = rb;

            rigidbody2D.gravityScale = OnPlatformPlayerGravityScale;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerMovement playerMovement = collision.GetComponent<PlayerMovement>();
            Rigidbody2D rigidbody2D = collision.GetComponent<Rigidbody2D>();

            playerMovement.isOnPlatform = false;
            rigidbody2D.gravityScale = initialPlayerGravityScale;
        }
    }
}
