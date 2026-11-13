using UnityEngine;

using UnityEngine.SceneManagement;
//libreria que que permite la carga/descarga de escenas   

public class SceneManagement : MonoBehaviour
{
    public void ChangeScene(int sceneToLoad)
    {
        //cambia la ecena con el numero indicado en el botton
        SceneManager.LoadScene(sceneToLoad);
    }

    public void ExitGame()
    {
        //accion que cierra la aplicacion 
        Debug.Log("El boton de salir funciona");
        Application.Quit();
    }
}
