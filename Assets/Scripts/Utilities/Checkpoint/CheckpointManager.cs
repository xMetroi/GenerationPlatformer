using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    [Header("Checkpoint Properties")]
    [SerializeField] private bool isActive;
    [SerializeField] private Sprite activeSprite;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerStats>() != null)
        {
            if (!isActive)
            {
                isActive = true;
                GetComponent<SpriteRenderer>().sprite = activeSprite;
                collision.GetComponent<PlayerStats>().SetActualCheckpoint(transform.position);
            }            
        }
    }
}
