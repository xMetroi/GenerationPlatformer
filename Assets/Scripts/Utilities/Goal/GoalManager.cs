using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalManager : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

            gameManager.FinishLevel();
        }
    }
}
