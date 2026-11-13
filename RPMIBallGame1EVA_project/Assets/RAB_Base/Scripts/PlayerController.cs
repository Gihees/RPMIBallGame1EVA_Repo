using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Parameters")]
    public float speed = 8f;
    public float jumpForce = 6f;
    public bool isCinematic; //si es verdadero usa mov. cenematico, si no, usa el fisico
    public bool useLinearVelocity; //si es verdadero: mov fisico por motor de acereracion, si no, por empije
    public bool isGrounded; //bool que determina si se puede saltar o no

    [Header("Reference Variables")]

    public Rigidbody PlayerRb; //Almacen del rb del jugador, permite moverlo por fisicas

    private Vector2 moveInput; // Almacen de 0/1 valor de los botones de movimiento

    public AudioSource sfxPlayer; //Ref al reploductor del sonidos del player
    public PlayerInteractor playerInteractor; //Ref al scrip que tiene la lista de sonidos

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isGrounded = true; //al inicio del juego activo la posibilidad de salto
    }

    // Update is called once per frame
    void Update()
    {
        moveInput.x = Input.GetAxis("Horizontal");
        moveInput.y = Input.GetAxis("Vertical");
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        //en el update
        if (isCinematic == true)
        {
            CinematicMove();
        }
        else
        {
            PhysicalMove();
        }

    }
    void FixedUpdate()
    {
        //en el fixedUpdte se llama a todo lo que tiene que ver con fisicas
        if (isCinematic == false)
        {
            PhysicalMove();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            //se devuelve la posibilidad de saltar (estar en el suelo)
            isGrounded = true;
        }
    }
    void CinematicMove()
    {
        // movimieno cinematico: consiste en multiplicar un eje position de transform por la velocidad
        // Eje potision: X (izquierda, derecha), Y (arriba, abajo), Z (alfrente, atras)
        // el movimiento cinematico siempre se multiplica por Time.deltaTime para normalizar
        transform.Translate(Vector3.forward * speed * moveInput.y * Time.deltaTime);
        transform.Translate(Vector3.right * speed * moveInput.x * Time.deltaTime);
    }

    void PhysicalMove()
    {
        //dos modos de movimiento fisico
        if (useLinearVelocity == true)
        {
            //1- cambiar la velocidad de cada eje del velocity del RigyBody
            PlayerRb.linearVelocity = new Vector3(moveInput.x * speed, PlayerRb.linearVelocity.y, moveInput.y * speed);
        }
        else
        {
            //2- aplicar una fuerza de empuje en cada uno de los ejes deseades(movimiento con inercia) si no se pone modo se pone automatico en fuerza
            PlayerRb.AddForce(Vector3.forward * speed * moveInput.y); //fuerza delante/atras
            PlayerRb.AddForce(Vector3.right * speed * moveInput.x); //fuerza derecha/izquierda
        }

    }

    void Jump()
    {
        if (isGrounded == true)
        {
            // salto = RigyBody . addForce(direccion * velocidad, modo de fuerza)
            PlayerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            //quitar la posibilidad de saltar hasta tocar el suelo de nuevo
            isGrounded = false;
            //reproduce el sonido de salto accediendo a la lista del player interactor
            sfxPlayer.PlayOneShot(playerInteractor.sfxList[2]);
        }

    }




    #region ImputMetodos
    // Metodos de imput: siempre es public void NombreImput (contexto = cualquier boton asociado a la accion)
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        //sino se declara estado del boron hace la accion 3 veses ( started, perfoment, cancelled)
        //para usar un estado determinado se usa condicional if
        if (context.performed)
        {
            //si se a usado el botton
            Jump();
        }
    }

    #endregion

}
