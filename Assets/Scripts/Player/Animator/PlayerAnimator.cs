using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [Header("Tilt Properties")]
    [SerializeField] private float maxTilt;
    [SerializeField] private float speedTilt;

    [Header("References")]
    [SerializeField] private Animator _anim;
    [SerializeField] private Animator anchorAnimator;

    private PlayerMovement playerMovement;
    SpriteRenderer spriteRenderer;
    TrailRenderer trailRenderer;

    private void OnEnable()
    {
        PlayerMovement.OnPlayerGoRight += OnPlayerMoveRight;
        PlayerMovement.OnPlayerGoLeft += OnPlayerMoveLeft;

        PlayerMovement.OnPlayerJump += OnPlayerJumped;
        PlayerMovement.OnPlayerLightLanded += OnPlayerLightLand;
        PlayerMovement.OnPlayerHeavyLanded += OnPlayerHeavyLand;
        PlayerMovement.OnPlayerFalling += OnPlayerFalled;
    }

    private void OnDisable()
    {
        PlayerMovement.OnPlayerGoRight -= OnPlayerMoveRight;
        PlayerMovement.OnPlayerGoLeft -= OnPlayerMoveLeft;

        PlayerMovement.OnPlayerJump -= OnPlayerJumped;
        PlayerMovement.OnPlayerLightLanded -= OnPlayerLightLand;
        PlayerMovement.OnPlayerHeavyLanded -= OnPlayerHeavyLand;
        PlayerMovement.OnPlayerFalling -= OnPlayerFalled;
    }

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        trailRenderer = GetComponent<TrailRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleCharacterTilt();
        OnPlayerDash();
    }

    void OnPlayerMoveLeft() { spriteRenderer.flipX = true; }

    void OnPlayerMoveRight() { spriteRenderer.flipX = false; }

    private void HandleCharacterTilt()
    {
        Vector2 input = playerMovement.GetMovementInput();

        var runningTilt = playerMovement.GetIsGrounded() ? Quaternion.Euler(0, 0, maxTilt * input.x) : Quaternion.identity;
        _anim.transform.up = Vector3.RotateTowards(_anim.transform.up, runningTilt * Vector2.up, speedTilt * Time.deltaTime, 0f);
    }

    private void OnPlayerJumped()
    {
        anchorAnimator.SetTrigger("JumpTrigger");
    }

    private void OnPlayerLightLand()
    {
        anchorAnimator.SetTrigger("LightLandedTrigger");
    }

    private void OnPlayerHeavyLand()
    {
        anchorAnimator.SetTrigger("HeavyLandedTrigger");
    }

    private void OnPlayerFalled()
    {
        anchorAnimator.SetTrigger("FallingTrigger");
    }

    private void OnPlayerDash()
    {
        if (playerMovement.GetIsDashing()) { trailRenderer.emitting = true; }
        else { trailRenderer.emitting = false; }
    }

}
