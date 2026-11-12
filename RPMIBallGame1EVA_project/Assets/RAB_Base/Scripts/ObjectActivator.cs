using Unity.VisualScripting;
using UnityEngine;

public class ObjectActivator : MonoBehaviour
{
    public GameObject objetToActivate; //Ref al objetoa encender al chocar con el player

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // primero: encender el objeto a activar
            objetToActivate.SetActive(true);
            //segundo: apaga el propio swich
            gameObject.SetActive (false);
        }
    }
}
