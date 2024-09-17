using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Properties")]
    [SerializeField] private float movementSpeed;
    private Vector2 movementInput;
    private Vector2 movementDirection;
    [SerializeField] private float jumpForce;
    [SerializeField] bool canMove;
    bool pressedJump;

    [Header("Dash Properties")]
    [SerializeField] private float dashForce;
    [SerializeField] private float dashDuration;
    bool pressedDash;
    [SerializeField] private bool isDashing;

    [Header("Gravity Properties")]
    [SerializeField] private float maxFallSpeed;
    [SerializeField] private Vector2 rbVelocity;
    [SerializeField] private float timeToLightFall;
    [SerializeField] float timeFalling;
    bool isFalling;

    #region Platform Movement Properties

    [HideInInspector] public bool isOnPlatform;
    [HideInInspector] public Rigidbody2D platformRb;

    #endregion

    #region Impulse Manager
    Vector2 impulseDirection;
    float impulseForce;
    float impulseDuration;
    bool applyImpulse;
    [SerializeField] bool isImpulsed;

    #endregion

    [Header("GroundCheck Properties")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Vector2 groundCheckBoxSize;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private bool isGrounded;

    private Rigidbody2D rb;
    private PlayerInputs playerInputs;
    private PlayerStats playerStats;
    private SpriteRenderer spriteRenderer;

    #region Events

    public static event Action OnPlayerGoLeft;
    public static event Action OnPlayerGoRight;

    public static event Action OnPlayerJump;
    public static event Action OnPlayerFalling;
    public static event Action OnPlayerLightLanded;
    public static event Action OnPlayerHeavyLanded;

    public static event Action OnPlayerStartImpulse;
    public static event Action OnPlayerEndedImpulse;

    public static event Action OnPlayerDashed;
    public static event Action OnPlayerEndDashed;

    #endregion

    private void OnEnable()
    {
        GameManager.OnEndedCountdown += OnCountdownEnded;
    }

    private void OnDisable()
    {
        GameManager.OnEndedCountdown -= OnCountdownEnded;
    }

    // Start is called before the first frame update
    void Start()
    {
        playerInputs = new PlayerInputs();
        playerInputs.Movement.Enable();
        rb = GetComponent<Rigidbody2D>();
        playerStats = GetComponent<PlayerStats>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        rbVelocity = rb.velocity;
        CheckInput();       
    }

    #region Getter / Setter

    public Vector2 GetMovementInput() { return movementInput; }

    public bool GetIsGrounded() { return isGrounded; }

    public bool GetIsDashing() { return isDashing; }

    #endregion

    private void FixedUpdate()
    {
        if (!isDashing && !isImpulsed)
        {
            Movement();
            Gravity();
        }

        if (pressedJump)
            Jump();

        if (applyImpulse)
            StartCoroutine(Impulse(impulseDirection, impulseForce, impulseDuration));

        if (pressedDash)
            StartCoroutine(Dash());
    }

    # region Input
    private void CheckInput()
    {
        if (!canMove) return;

        movementInput = playerInputs.Movement.Movement.ReadValue<Vector2>().normalized;

        if (movementInput.x > 0) OnPlayerGoRight?.Invoke();
        else if (movementInput.x < 0) OnPlayerGoLeft?.Invoke();

        movementDirection = new Vector2(movementInput.x, movementInput.y) * movementSpeed;

        if (playerInputs.Movement.Dash.WasPerformedThisFrame())
        {
            if (playerStats.GetDashCount() > 0)
                pressedDash = true;
        }

        if (playerInputs.Movement.Jump.WasPerformedThisFrame() && isGrounded) 
        {
            pressedJump = true;

            rb.gravityScale = 1;
        }

        if (!isOnPlatform)
            if (playerInputs.Movement.Jump.WasReleasedThisFrame() | rb.velocity.y < -0.1f) rb.gravityScale = 2;
    }
    #endregion

    #region Movement

    private void Movement()
    {
        if (isOnPlatform) rb.velocity = new Vector2(movementDirection.x + platformRb.velocity.x, rb.velocity.y);
        else rb.velocity = new Vector2(movementDirection.x, rb.velocity.y);
    }

    private void OnCountdownEnded()
    {
        canMove = true;
    }

    private void Gravity()
    {
        bool wasGrounded = isGrounded;
        isGrounded = Physics2D.BoxCast(groundCheck.position, groundCheckBoxSize, 0, Vector2.down, 0, groundLayer);
        
        if (!isGrounded) timeFalling += Time.fixedDeltaTime;

        if (timeFalling >= 0.5f && !isFalling)
        {
            isFalling = true;
            OnPlayerFalling?.Invoke();
        }

        if (!wasGrounded && isGrounded)
        {
            if (timeFalling <= timeToLightFall)
            {
            OnPlayerLightLanded?.Invoke();
            //Debug.Log("Light Land");
            }
            else if (timeFalling > timeToLightFall)
            {
                OnPlayerHeavyLanded?.Invoke();
                //Debug.Log("Heavy Land");
            }

            timeFalling = 0;
            isFalling = false;
        }

        if (rb.velocity.y < 0 && !isFalling)
        {
            isFalling = true;            
        }
        else if (rb.velocity.y >= 0 && isFalling)
            isFalling = false;

        //Higher gravity if player is falling
        //if (rb.velocity.y < 0) rb.gravityScale = rb.gravityScale * 1.5f;

        rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -maxFallSpeed));
    }

    private void Jump()
    {
        if (!isOnPlatform) rb.velocity += new Vector2(0, jumpForce);
        else rb.velocity += new Vector2(0, jumpForce * 1.3f);
        pressedJump = false;
        OnPlayerJump?.Invoke();
    }

    private IEnumerator Dash()
    {
        isDashing = true;
        pressedDash = false;

        float dashDirection = spriteRenderer.flipX ? -1f : 1f;
        rb.velocity = new Vector2(dashDirection * dashForce, rb.velocity.y);
        playerStats.RemoveDash(1);

        OnPlayerDashed?.Invoke();

        yield return new WaitForSeconds(dashDuration);

        OnPlayerEndDashed?.Invoke();
        isDashing = false;
    }

    public void ApplyImpulse(Vector2 impulseDirection, float impulseForce, float impulseDuration)
    {
        applyImpulse = true;

        this.impulseDirection = impulseDirection; 
        this.impulseForce = impulseForce;
        this.impulseDuration = impulseDuration;  
    }

    private IEnumerator Impulse(Vector2 direction, float impulseForce, float impulseDuration)
    {
        isImpulsed = true;
        applyImpulse = false;

        rb.velocity = Vector2.zero;
        rb.AddForce(direction * impulseForce, ForceMode2D.Impulse);

        OnPlayerStartImpulse?.Invoke();

        yield return new WaitForSeconds(impulseDuration);

        isImpulsed = false;
        this.impulseDirection = Vector2.zero;
        this.impulseForce = 0;
        this.impulseDuration = 0;

        OnPlayerEndedImpulse?.Invoke();
    }

    #endregion

    #region Debug
    private void OnDrawGizmos()
    {
        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawWireCube(groundCheck.position, groundCheckBoxSize); //Draw box check
    }
    #endregion
}
