using UnityEngine;
using UnityEngine.SceneManagement;

public class WinTrigger : MonoBehaviour
{
    [SerializeField] public string winSceneName = "Win";

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(winSceneName);
        }
    }
}
