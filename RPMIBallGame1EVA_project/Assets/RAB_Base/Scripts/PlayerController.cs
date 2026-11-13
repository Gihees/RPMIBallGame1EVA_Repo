using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Editor references")]
    public Rigidbody playerRb;

    [Header("Movement Parameters")]
    public float speed = 10f;
    public Vector2 moveInput;

    [Header("Jump parameters")]
    public float jumpForce = 6f;
    public bool isGrounded = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CinematicMovement()
    {

    }

    void PhysicalMovement()
    {

    }

    void Jump()
    {

    }

    #region Input Methods

    public void OnMove(InputAction.CallbackContext contex)
    {
        moveInput = contex.ReadValue<Vector2>();
    }
    public void OnJump(InputAction.CallbackContext contex)
    {
        if(contex.performed && isGrounded == true)
        {
            isGrounded = false;
            Jump();
        }
    }



    #endregion
}
