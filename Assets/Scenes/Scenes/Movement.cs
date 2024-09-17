using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed;
    [SerializeField] private CharacterController controller;

    [Header("Gravity")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float gravityForce;

    [Header("GroundCheck")]
    [SerializeField] private Vector2 boxSize;
    [SerializeField] private Transform groundCheckPosition;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private bool isGrounded;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GroundCheck();
        Gravity();
        PlayerMovement();
    }

    #region Player Movimientos
    private void PlayerMovement()
    {
        if (Input.GetKey(KeyCode.A))
        {
            controller.Move(new Vector3(-speed, 0, 0) * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.D))
        {
            controller.Move(new Vector3(speed, 0, 0) * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded) controller.Move(new Vector3(0, jumpForce, 0) * Time.deltaTime);
        }
    }
    #endregion

    #region Gravity

    private void GroundCheck()
    {
        isGrounded = Physics2D.BoxCast(groundCheckPosition.position, boxSize, 0, Vector2.down, 0, groundLayer);
    }

    private void Gravity()
    {
        if (!isGrounded) controller.Move(new Vector3(0, gravityForce, 0) * Time.deltaTime);
    }

    #endregion

    private void OnDrawGizmos()
    {
        if (isGrounded)
        {
            Gizmos.color = Color.green;
        }
        else
        {
            Gizmos.color = Color.red;
        }

        Gizmos.DrawWireCube(groundCheckPosition.position, boxSize);
    }
}
