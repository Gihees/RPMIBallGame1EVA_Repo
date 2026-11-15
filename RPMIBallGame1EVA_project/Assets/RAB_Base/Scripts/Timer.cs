
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    public float startTime = 120f;
    public TMP_Text timerLabel;

    void Start()
    {
        timerLabel.text = startTime.ToString();
    }

    void Update()
    {
        if (startTime > 0)
        {
            startTime -= Time.deltaTime;
            startTime = Mathf.Max(0, startTime);

            timerLabel.text = Mathf.Round(startTime).ToString();
        }
    }
}
