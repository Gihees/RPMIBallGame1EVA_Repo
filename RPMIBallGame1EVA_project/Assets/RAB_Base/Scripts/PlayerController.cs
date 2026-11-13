using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para reiniciar el nivel

public class EsqueletoPatrulla : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    public Transform puntoA;
    public Transform puntoB;
    public float velocidad = 3f;

    private Transform objetivoActual;

    void Start()
    {
        // El esqueleto empieza caminando hacia el punto B
        objetivoActual = puntoB;
    }

    void Update()
    {
        MoverEsqueleto();
    }

    void MoverEsqueleto()
    {
        // Mueve al esqueleto hacia el objetivo actual
        transform.position = Vector3.MoveTowards(transform.position, objetivoActual.position, velocidad * Time.deltaTime);

        // Si llega al punto, cambia de dirección y gira
        if (Vector3.Distance(transform.position, objetivoActual.position) < 0.1f)
        {
            objetivoActual = (objetivoActual == puntoB) ? puntoA : puntoB;
            transform.LookAt(objetivoActual); // En 2D, usa transform.Rotate(0f, 180f, 0f) en su lugar
        }
    }

    // Detecta cuando el jugador entra en el área del Collider (Is Trigger)
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ReiniciarNivel();
        }
    }

    void ReiniciarNivel()
    {
        // Obtiene la escena actual y la vuelve a cargar
        int escenaActual = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(escenaActual);
    }
}
