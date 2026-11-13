using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public float speed;
    public Vector3 rotation; 
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotation * Time.deltaTime * speed , Space.Self);
    }
}
