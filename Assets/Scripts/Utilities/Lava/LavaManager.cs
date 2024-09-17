using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaManager : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerStats playerStats = collision.GetComponent<PlayerStats>();

            if (playerStats != null) playerStats.KillPlayer(1);
        }
    }
}
