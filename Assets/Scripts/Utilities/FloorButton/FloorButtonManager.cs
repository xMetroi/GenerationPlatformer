using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class FloorButtonManager : MonoBehaviour
{
    [Header("Floor Button Properties")]
    [SerializeField] Sprite enabledFloorButtonSprite;
    [SerializeField] Sprite disabledFloorButtonSprite;
    [SerializeField] private bool isActive = false;
    private SpriteRenderer spriteRenderer;

    #region Events

    public event Action OnFloorButtonActivated;

    #endregion

    private void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Rigidbody2D rigidbody2D = collision.GetComponent<Rigidbody2D>();

        if (rigidbody2D != null && !isActive)
        {
            isActive = true;
            spriteRenderer.sprite = enabledFloorButtonSprite;
            OnFloorButtonActivated?.Invoke();
        }
    }
}
