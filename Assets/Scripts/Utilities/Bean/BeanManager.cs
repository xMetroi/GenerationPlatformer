using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeanManager : MonoBehaviour
{
    [Header("Bean Properties")]
    [SerializeField] private int beanQuantity;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerStats playerStats = collision.GetComponent<PlayerStats>();

        if (playerStats != null)
        {
            if (playerStats.GetBeanCount() <= playerStats.GetMaxBeanCount())
            {
                playerStats.AddBean(beanQuantity);
                Destroy(this.gameObject);
            }
        }
    }
}
