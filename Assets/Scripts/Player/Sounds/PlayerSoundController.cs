using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundController : MonoBehaviour
{
    [Header("Sound List")]
    [SerializeField] private AudioClip playerJump;
    [SerializeField] private AudioClip playerLightLanded;
    [SerializeField] private AudioClip playerHeavyLanded;
    [SerializeField] private AudioClip playerDash;

    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        PlayerMovement.OnPlayerJump += OnPlayerJumped;
        PlayerMovement.OnPlayerLightLanded += OnPlayerLightLand;
        PlayerMovement.OnPlayerHeavyLanded += OnPlayerHeavyLand;
        PlayerMovement.OnPlayerDashed += OnPlayerDash;
    }

    private void OnDisable()
    {
        PlayerMovement.OnPlayerJump -= OnPlayerJumped;
        PlayerMovement.OnPlayerLightLanded -= OnPlayerLightLand;
        PlayerMovement.OnPlayerHeavyLanded -= OnPlayerHeavyLand;
        PlayerMovement.OnPlayerDashed -= OnPlayerDash;
    }

    private void OnPlayerJumped()
    {
        audioSource.PlayOneShot(playerJump);
    }

    private void OnPlayerLightLand()
    {
        audioSource.PlayOneShot(playerLightLanded);
    }

    private void OnPlayerHeavyLand()
    {
        audioSource.PlayOneShot(playerHeavyLanded);
    }

    private void OnPlayerDash()
    {
        audioSource.PlayOneShot(playerDash);
    }
}
