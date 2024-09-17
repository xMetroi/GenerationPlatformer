using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashManager : MonoBehaviour
{
    [Header("Dash Properties")]
    [SerializeField] private int dashQuantity;
    [SerializeField] private bool destroyAfterPlayerColl;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerStats playerStats = collision.GetComponent<PlayerStats>();

        if (playerStats != null )
        {
            if (playerStats.GetDashCount() <= playerStats.GetMaxDashCount()) 
            {
                playerStats.AddDash(dashQuantity);
                if (destroyAfterPlayerColl)
                    gameObject.SetActive(false);
                    //Destroy(this.gameObject);
            }
            
        }
    }
}
