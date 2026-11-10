using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInteractor : MonoBehaviour
{
    [Header("Respawn Point")]
    public Transform respawnPoint; //ref al punto de respawn y su pocicion
    public float respawnFalLimit = -10; //valor de caida para respawn

    [Header("Point Sistem Management")]
    public int point = 0; //puntos actuales del jugador

    public int winPoints = 10; //puntos nesesarios para pasar de nivel

    public int sceneToLoad = 2; //numero de la escena para ganar la partida

    [Header("Referense Variables")]
    public Rigidbody playerRb;

    [Header(" Music Configuration")]
    public AudioClip[] sfxList; //lista de sonidosa triggerear por el player

    public AudioSource sfxPlayer; //Ref al reproductor de sonidos 




    // Update is called once per frame
    void Update()
    {
        if (transform.position.y <= respawnFalLimit) 
        {
            //respawn port caida
            Respawn();
        }

        if (point >= winPoints)
        {
            SceneChange();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("Obstacle"))
        {
            //respawn por choque
            Respawn();
        }
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            //sumar 1 punto al coger pick up
            point += 1;
            //reproduccir sonido de recoger positivo = sfxList[1]
            sfxPlayer.PlayOneShot(sfxList[1]);

            other.gameObject.SetActive(false); //Apagar el pick Up
        }
        if (other.gameObject.CompareTag("Swich"))
        {
            //reproduccimos el sonido del swich
            sfxPlayer.PlayOneShot(sfxList[3]);
        }


    }

    void Respawn()
    {
        //sustituir el transform . position del player por el transform . position del respawn
        transform.position = respawnPoint.position;
        // REsetear la velocidad del motor de aceleracion del rigybody
        playerRb.linearVelocity = new Vector3(0, 0, 0);
        // Reproduccir sonidos de respawn = sfxList[0]
        sfxPlayer.PlayOneShot(sfxList[0]);
    }

    void SceneChange()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
