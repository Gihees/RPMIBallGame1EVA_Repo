using UnityEngine;

[RequireComponent(typeof(Light))]
public class DayToNight : MonoBehaviour
{
    [Header("Tiempo")]
    public float startDelay = 5f; // segundos antes de empezar a oscurecer
    public float transitionDuration = 20f; // segundos que dura el atardecer

    [Header("Rotación del sol (eje X)")]
    public float startAngle = 50f; // sol alto (día)
    public float endAngle = 190f; // sol bajo el horizonte (noche)

    [Header("Luz del sol")]
    public float dayIntensity = 1f;
    public float nightIntensity = 0.05f; // un poco de "luz de luna"
    public Color dayColor = Color.white;
    public Color nightColor = new Color(0.4f, 0.5f, 0.8f);

    [Header("Ambiente")]
    public float dayAmbient = 1f; // intensidad de la luz ambiental de día
    public float nightAmbient = 0.1f;
    public float dayExposure = 1f; // brillo del cielo (skybox) de día
    public float nightExposure = 0.1f;

    [Header("Niebla (opcional)")]
    public bool changeFog = false;
    public Color dayFogColor = Color.gray;
    public Color nightFogColor = new Color(0.02f, 0.03f, 0.06f);

    [Header("Reinicio del nivel")]
    [Tooltip("Si el Player muere y se recarga la escena, mantiene la noche en vez de volver al día.")]
    public bool keepNightAfterRestart = true;

    private Light sun;
    private float baseYRotation;
    private float timer;
    private bool finished;
    private float environmentTimer;
    private Material skyboxInstance;

    // Recuerda si ya anocheció (sobrevive a las recargas de escena)
    private static bool nightReached;

    void Awake()
    {
        sun = GetComponent<Light>();
        baseYRotation = transform.eulerAngles.y;
    }

    void Start()
    {
        // Copia del skybox para no modificar el material original del proyecto.
        if (RenderSettings.skybox != null)
        {
            skyboxInstance = new Material(RenderSettings.skybox);
            RenderSettings.skybox = skyboxInstance;
        }

        // Si ya era de noche antes de reiniciar, empezar directamente de noche.
        if (keepNightAfterRestart && nightReached)
        {
            Apply(1f);
            finished = true;
            DynamicGI.UpdateEnvironment();
        }
        else
        {
            Apply(0f);
        }
    }

    void Update()
    {
        if (finished) return; // una vez es de noche, no se hace nada más

        timer += Time.deltaTime;
        float t = Mathf.Clamp01((timer - startDelay) / transitionDuration);
        float k = Mathf.SmoothStep(0f, 1f, t); // transición suave al inicio y al final

        Apply(k);

        // Actualizar la iluminación ambiental (es costoso, así que se hace 4 veces por segundo).
        environmentTimer += Time.deltaTime;
        if (environmentTimer >= 0.25f)
        {
            environmentTimer = 0f;
            DynamicGI.UpdateEnvironment();
        }

        if (t >= 1f)
        {
            finished = true;
            nightReached = true;
            DynamicGI.UpdateEnvironment();
        }
    }

    // k = 0 -> día completo, k = 1 -> noche completa
    void Apply(float k)
    {
        // Rotación del sol
        transform.rotation = Quaternion.Euler(Mathf.Lerp(startAngle, endAngle, k), baseYRotation, 0f);

        // Luz del sol
        sun.intensity = Mathf.Lerp(dayIntensity, nightIntensity, k);
        sun.color = Color.Lerp(dayColor, nightColor, k);

        // Luz ambiental y reflejos
        RenderSettings.ambientIntensity = Mathf.Lerp(dayAmbient, nightAmbient, k);
        RenderSettings.reflectionIntensity = Mathf.Lerp(dayAmbient, nightAmbient, k);

        // Brillo del cielo
        if (skyboxInstance != null && skyboxInstance.HasProperty("_Exposure"))
            skyboxInstance.SetFloat("_Exposure", Mathf.Lerp(dayExposure, nightExposure, k));

        // Niebla
        if (changeFog)
        {
            RenderSettings.fog = true;
            RenderSettings.fogColor = Color.Lerp(dayFogColor, nightFogColor, k);
        }
    }
}


