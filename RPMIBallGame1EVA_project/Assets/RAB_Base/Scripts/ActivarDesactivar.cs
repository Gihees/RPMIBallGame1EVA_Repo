using UnityEngine;

public class Palanca : MonoBehaviour
{
    [Header("Objeto que controla la palanca")]
    [SerializeField] private GameObject controlObject;

    [Header("Efecto de la palanca")]
    [SerializeField] private GameObject[] objectsToDisable;
    [SerializeField] private GameObject[] objectsToEnable;
    [SerializeField] private bool onlyOnce = true;

    private bool used = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // Si el objeto de control está activo, la palanca no funciona
        if (controlObject != null && controlObject.activeInHierarchy) return;

        if (onlyOnce && used) return;

        used = true;

        foreach (GameObject obj in objectsToDisable)
        {
            obj.SetActive(false);
        }

        foreach (GameObject obj in objectsToEnable)
        {
            obj.SetActive(true);
        }
    }
}
