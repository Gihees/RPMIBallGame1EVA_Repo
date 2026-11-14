using UnityEngine;
using UnityEngine.SceneManagement;

public class WinTrigger: MonoBehaviour
{
    [Header("Scene Management")]
    public int sceneToLoad = 4;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
