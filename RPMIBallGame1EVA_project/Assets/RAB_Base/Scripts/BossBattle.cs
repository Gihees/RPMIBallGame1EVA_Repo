using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBattle : MonoBehaviour
{
    [Header("Ataque 1 - Lava")]
    [SerializeField] private Transform lava;
    [SerializeField] private float lavaRiseHeight = 3f;
    [SerializeField] private float lavaRiseTime = 1f;
    [SerializeField] private float lavaStayTime = 1.5f;
    [SerializeField] private GameObject lavaWarning;

    [Header("Ataque 2 - Barrido lateral")]
    [SerializeField] private Transform sweeper;
    [SerializeField] private float leftX = -10f;
    [SerializeField] private float rightX = 10f;
    [SerializeField] private float sweepDuration = 2f;
    [SerializeField] private GameObject sweepWarningLeft;
    [SerializeField] private GameObject sweepWarningRight;

    [Header("Ataque 3 - Pilar que desactiva objetos")]
    [SerializeField] private Transform pillar;
    [SerializeField] private float pillarRiseHeight = 3f;
    [SerializeField] private float pillarRiseTime = 0.8f;
    [SerializeField] private float pillarStayTime = 1f;
    [SerializeField] private GameObject pillarWarning;
    [SerializeField] private GameObject[] objectsToDisable;
    [SerializeField] private GameObject[] objectsToEnable;
    [SerializeField] private int pillarHitsToFinish = 2;

    [Header("General")]
    [SerializeField] private float timeBetweenAttacks = 2f;
    [SerializeField] private float warningTime = 1.5f;
    [SerializeField] private float winDelay = 5f;
    [SerializeField] private GameObject winTrigger;

    private Vector3 lavaStartPos;
    private Vector3 pillarStartPos;
    private int pillarHits = 0;

    private void Start()
    {
        lavaStartPos = lava.position;
        pillarStartPos = pillar.position;

        sweeper.gameObject.SetActive(false);
        winTrigger.SetActive(false);

        lavaWarning.SetActive(false);
        sweepWarningLeft.SetActive(false);
        sweepWarningRight.SetActive(false);
        pillarWarning.SetActive(false);

        StartCoroutine(BossLoop());
    }

    private IEnumerator BossLoop()
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

    private IEnumerator LavaAttack()
    {
        lavaWarning.SetActive(true);
        yield return new WaitForSeconds(warningTime);

        yield return MoveTo(lava, lavaStartPos + Vector3.up * lavaRiseHeight, lavaRiseTime);
        yield return new WaitForSeconds(lavaStayTime);
        yield return MoveTo(lava, lavaStartPos, lavaRiseTime);

        lavaWarning.SetActive(false);
    }

    private IEnumerator SweepAttack()
    {
        bool fromLeft = Random.value > 0.5f;
        float startX = fromLeft ? leftX : rightX;
        float endX = fromLeft ? rightX : leftX;

        Vector3 startPos = new Vector3(startX, sweeper.position.y, sweeper.position.z);
        Vector3 endPos = new Vector3(endX, sweeper.position.y, sweeper.position.z);

        GameObject warning = fromLeft ? sweepWarningLeft : sweepWarningRight;
        warning.SetActive(true);
        yield return new WaitForSeconds(warningTime);

        sweeper.position = startPos;
        sweeper.gameObject.SetActive(true);

        yield return MoveTo(sweeper, endPos, sweepDuration);

        sweeper.gameObject.SetActive(false);
        warning.SetActive(false);
    }

    private IEnumerator PillarAttack()
    {
        pillarWarning.SetActive(true);
        yield return new WaitForSeconds(warningTime);

        yield return MoveTo(pillar, pillarStartPos + Vector3.up * pillarRiseHeight, pillarRiseTime);

        if (pillarHits < objectsToDisable.Length)
        {
            objectsToDisable[pillarHits].SetActive(false);
        }

        if (pillarHits < objectsToEnable.Length)
        {
            objectsToEnable[pillarHits].SetActive(true);
        }

        pillarHits++;

        if (pillarHits >= pillarHitsToFinish)
        {
            StartCoroutine(WinRoutine());
        }

        yield return new WaitForSeconds(pillarStayTime);
        yield return MoveTo(pillar, pillarStartPos, pillarRiseTime);

        pillarWarning.SetActive(false);
    }

    private IEnumerator WinRoutine()
    {
        yield return new WaitForSeconds(winDelay);
        winTrigger.SetActive(true);
    }

    private IEnumerator MoveTo(Transform target, Vector3 destination, float duration)
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
