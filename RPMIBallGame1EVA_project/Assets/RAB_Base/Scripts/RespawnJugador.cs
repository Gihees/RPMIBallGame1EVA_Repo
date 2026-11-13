using UnityEngine;

public class RespawnJugador : MonoBehaviour
{
    public Transform puntoRespawn; // Arrastra aquí PuntoRespawn
    public float alturaMinimaCaida = -5f; // Si baja de esta altura Y, muere
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Si la bola cae al vacío por debajo de la altura mínima, reaparece
        if (transform.position.y < alturaMinimaCaida)
        {
            Reaparecer();
        }
    }

    // Si choca físicamente con el oso, huesos o suelo mortal (Tag "Obstacle")
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Reaparecer();
        }
    }

    // Por si algún obstáculo tiene "Is Trigger" activado
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            Reaparecer();
        }
    }

    public void Reaparecer()
    {
        transform.position = puntoRespawn.position;
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}