using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Editor references")]
    public Rigidbody playerRb;

    [Header("Movement Parameters")]
    public float speed = 10;
    public Vector2 moveInput;

    [Header("Jump parameters")]
    public float jumpForce = 6;
    public bool isGrounded = true;

    [Header("Respawn Sistem")]
    public float fallLimit = -10;
    public Transform respawnPoint;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        CinematicMovement();
        if (transform.position.y <= fallLimit)
        {
            Respawn();
        }
    }

    private void FixedUpdate()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            //cargar escena # referenciando al scene management 
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            //cargar escena # referanciando al scene management
        }

    }

    void CinematicMovement()
    {
        transform.Translate(Vector3.right * speed * moveInput.x * Time.deltaTime);
        transform.Translate(Vector3.forward * speed * moveInput.y * Time.deltaTime);
    }

    void PhysicalMovement()
    {

    }

    void Jump()
    {
        playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    void Respawn()
    {
        transform.position = respawnPoint.position;
    }

    #region Input Methods

    public void OnMove(InputAction.CallbackContext contex)
    {
        moveInput = contex.ReadValue<Vector2>();
    }
    public void OnJump(InputAction.CallbackContext contex)
    {
        if (contex.performed && isGrounded == true)
        {
            isGrounded = false;
            Jump();
        }
    }



    #endregion
}
