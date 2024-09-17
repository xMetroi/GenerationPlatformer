using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] float cameraSpeed;
    [Space]
    [SerializeField] float cameraOffsetResetSpeedChange;
    [SerializeField] float cameraOffsetX = 2;
    [SerializeField] float cameraOffsetXSpeedChange;
    [SerializeField] float cameraOffsetY;
    [SerializeField] float cameraOffsetYSpeedChange;

    private void OnEnable()
    {
        GameManager.OnPlayerSpawned += OnPlayerSpawn;
    }

    private void OnDisable()
    {
        GameManager.OnPlayerSpawned -= OnPlayerSpawn;
    }

    private void OnPlayerSpawn()
    {
        if (playerTransform == null) playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        //HandleCameraOffset();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        HandleCameraPosition();
    }

    private void HandleCameraPosition()
    {
        if (playerTransform != null)
        {
            transform.position = new Vector3(Mathf.Lerp(transform.position.x, playerTransform.position.x + cameraOffsetX, cameraSpeed * Time.deltaTime),
                                         Mathf.Lerp(transform.position.y, playerTransform.position.y + cameraOffsetY, cameraSpeed * Time.deltaTime), -10);
        }
    }

    #region CameraOffset

    private void HandleCameraOffset()
    {
        if (playerTransform != null)
        {
            PlayerMovement playerMovement = playerTransform.GetComponent<PlayerMovement>();

            Vector2 input = playerMovement.GetMovementInput();

            if (input.x == 0) { SetCameraOffsetX(0, cameraOffsetResetSpeedChange); }
            else if (input.x > 0) { SetCameraOffsetX(2, cameraOffsetXSpeedChange); }
            else if(input.x < 0) { SetCameraOffsetX(-2, cameraOffsetXSpeedChange); }
        }
    }

    public float GetCameraOffsetX()
    {
        return cameraOffsetX;
    }

    public void SetCameraOffsetX(float x, float speedChange)
    {
        cameraOffsetX = Mathf.Lerp(cameraOffsetX, x, speedChange * Time.deltaTime);
    }

    public float GetCameraOffsetY()
    {
        return cameraOffsetY;
    }

    public void SetCameraOffsetY(float y, float speedChange)
    {
        cameraOffsetY = Mathf.Lerp(cameraOffsetY, y, speedChange * Time.deltaTime);
    }

    #endregion
}

