using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBattle : MonoBehaviour
{
    [Header("Ataque 1 - Lava")]
    [SerializeField] public Transform lava;
    [SerializeField] public float lavaRiseHeight = 3f;
    [SerializeField] public float lavaRiseTime = 1f;
    [SerializeField] public float lavaStayTime = 1.5f;

    [Header("Ataque 2 - Barrido lateral")]
    [SerializeField] public Transform sweeper;
    [SerializeField] public float leftX = -10f;
    [SerializeField] public float rightX = 10f;
    [SerializeField] public float sweepDuration = 2f;

    [Header("Ataque 3 - Pilar que desactiva objetos")]
    [SerializeField] public Transform pillar;
    [SerializeField] public float pillarRiseHeight = 3f;
    [SerializeField] public float pillarRiseTime = 0.8f;
    [SerializeField] public float pillarStayTime = 1f;
    [SerializeField] public GameObject[] objectsToDisable;
    [SerializeField] public int pillarHitsToFinish = 2;

    [Header("General")]
    [SerializeField] public float timeBetweenAttacks = 2f;
    [SerializeField] public float winDelay = 5f;
    [SerializeField] public GameObject winTrigger;

    public Vector3 lavaStartPos;
    public Vector3 pillarStartPos;
    public int pillarHits = 0;

    public void Start()
    {
        lavaStartPos = lava.position;
        pillarStartPos = pillar.position;

        sweeper.gameObject.SetActive(false);
        winTrigger.SetActive(false);

        StartCoroutine(BossLoop());
    }

    public IEnumerator BossLoop()
    {
        yield return new WaitForSeconds(timeBetweenAttacks);

        while (true)
        {
            bool pillarPhaseActive = pillarHits < pillarHitsToFinish;

            List<int> available = new List<int> { 2 };
            if (pillarPhaseActive)
            {
                available.Add(1);
                available.Add(3);
            }

            int chosen = available[Random.Range(0, available.Count)];

            switch (chosen)
            {
                case 1: yield return StartCoroutine(LavaAttack()); break;
                case 2: yield return StartCoroutine(SweepAttack()); break;
                case 3: yield return StartCoroutine(PillarAttack()); break;
            }

            yield return new WaitForSeconds(timeBetweenAttacks);
        }
    }

    public IEnumerator LavaAttack()
    {
        yield return MoveTo(lava, lavaStartPos + Vector3.up * lavaRiseHeight, lavaRiseTime);
        yield return new WaitForSeconds(lavaStayTime);
        yield return MoveTo(lava, lavaStartPos, lavaRiseTime);
    }

    public IEnumerator SweepAttack()
    {
        bool fromLeft = Random.value > 0.5f;
        float startX = fromLeft ? leftX : rightX;
        float endX = fromLeft ? rightX : leftX;

        Vector3 startPos = new Vector3(startX, sweeper.position.y, sweeper.position.z);
        Vector3 endPos = new Vector3(endX, sweeper.position.y, sweeper.position.z);

        sweeper.position = startPos;
        sweeper.gameObject.SetActive(true);

        yield return MoveTo(sweeper, endPos, sweepDuration);

        sweeper.gameObject.SetActive(false);
    }

    public IEnumerator PillarAttack()
    {
        yield return MoveTo(pillar, pillarStartPos + Vector3.up * pillarRiseHeight, pillarRiseTime);

        if (pillarHits < objectsToDisable.Length)
        {
            objectsToDisable[pillarHits].SetActive(false);
        }

        pillarHits++;

        if (pillarHits >= pillarHitsToFinish)
        {
            StartCoroutine(WinRoutine());
        }

        yield return new WaitForSeconds(pillarStayTime);
        yield return MoveTo(pillar, pillarStartPos, pillarRiseTime);
    }

    public IEnumerator WinRoutine()
    {
        yield return new WaitForSeconds(winDelay);
        winTrigger.SetActive(true);
    }

    public IEnumerator MoveTo(Transform target, Vector3 destination, float duration)
    {
        Vector3 origin = target.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            target.position = Vector3.Lerp(origin, destination, t);
            yield return null;
        }

        target.position = destination;
    }
}
