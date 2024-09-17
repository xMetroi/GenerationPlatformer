using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonballManager : MonoBehaviour
{
    [SerializeField] private Vector2 direction;
    [SerializeField] float speed;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("CannonBall")) return;

        

        if (collision.CompareTag("Player"))
        {
            PlayerStats playerStats = collision.GetComponent<PlayerStats>();

            playerStats.KillPlayer(1);
        }

        Destroy(gameObject);
    }

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }
}
