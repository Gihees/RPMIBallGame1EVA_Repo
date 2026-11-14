using UnityEngine;

public class PickUp : MonoBehaviour
{
    [Header("Rotate Parameters")]
    public float rotSpeedX = 90f;
    public float rotSpeedY = 0f;
    public float rotSpeedZ = 0f;

    void Update()
    {
        // Gira continuamente sobre sus propios ejes locales (Space.Self)
        transform.Rotate(new Vector3(rotSpeedX, rotSpeedY, rotSpeedZ) * Time.deltaTime, Space.Self);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameObject.SetActive(false);
        }
    }
}