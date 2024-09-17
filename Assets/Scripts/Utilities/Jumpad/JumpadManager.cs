using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpadManager : MonoBehaviour
{
    [Header("Jumpad Properties")]
    [SerializeField] private float launchForce;
    [SerializeField] private float impulseDuration;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerMovement playerMovement = collision.GetComponent<PlayerMovement>();

            if (playerMovement != null)
            {
                playerMovement.ApplyImpulse(transform.up, launchForce, impulseDuration);
            }
        }

        //If not is a player

        Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();

        rb.velocity = Vector2.zero;

        rb.AddForce(transform.up * launchForce, ForceMode2D.Impulse);
    }
}
