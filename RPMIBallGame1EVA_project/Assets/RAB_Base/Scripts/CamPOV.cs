using UnityEngine;

public class MovimientoCamara : MonoBehaviour
{
    private PlayerController playerController;
    private Transform camara;
    private float velocidadOriginal;

    void Start()
    {
        // 1. Conectamos con el script ya hecho
        playerController = GetComponent<PlayerController>();

        // 2. Obtenemos la dirección de la cámara activa
        if (Camera.main != null)
        {
            camara = Camera.main.transform;
        }

        // 3. Tomamos su velocidad y anulamos su avance recto para no sumar doble movimiento
        if (playerController != null)
        {
            velocidadOriginal = playerController.speed;
            playerController.speed = 0f;
        }
    }

    void Update()
    {
        if (playerController == null || camara == null) return;

        // Leemos las teclas (WASD / Flechas) desde el PlayerController de tus compañeros
        Vector2 input = playerController.moveInput;

        // Calculamos hacia dónde mira la cámara en el plano horizontal
        Vector3 adelanteCamara = camara.forward;
        Vector3 derechaCamara = camara.right;

        adelanteCamara.y = 0f;
        derechaCamara.y = 0f;

        adelanteCamara.Normalize();
        derechaCamara.Normalize();

        // Combinamos las teclas con el ángulo de la cámara (W siempre va hacia adelante de la cámara)
        Vector3 direccion = (adelanteCamara * input.y) + (derechaCamara * input.x);

        // Aplicamos el movimiento usando la velocidad original de tus compañeros
        transform.Translate(direccion * velocidadOriginal * Time.deltaTime, Space.World);
    }
}

