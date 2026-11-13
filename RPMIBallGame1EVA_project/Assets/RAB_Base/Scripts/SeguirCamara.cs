using UnityEngine;

public class SeguirCamara : MonoBehaviour
{
    public Transform objetivo; // Aquí arrastraremos BolaPrueba
    public Vector3 distancia = new Vector3(0f, 5f, -7f); // Altura y lejanía

    void LateUpdate()
    {
        if (objetivo != null)
        {
            // Mantiene la cámara a la distancia elegida sin girar con la bola
            transform.position = objetivo.position + distancia;
        }
    }
}