using UnityEngine;
using UnityEngine.SceneManagement;

// Patrulla por waypoints: el esqueleto se mueve en línea recta de un punto a otro.
public class SkeletonPatrol : MonoBehaviour
{

    [Header("Patrulla")]
    [Tooltip("Puntos de la ruta, en orden. Al llegar al último vuelve al primero.")]
    public Transform[] waypoints;
    public float patrolSpeed = 2f;      // Velocidad al caminar
    public float rotationSpeed = 200f;  // Velocidad de giro (grados por segundo)
    public float alignAngle = 5f;
    public float waitTime = 1.5f;       // Segundos que espera en cada waypoint


    [Tooltip("Distancia a la que se considera que ha llegado al waypoint.")]
    public float arrivalDistance = 0.2f;

    [Header("Visión")]
    [Tooltip("Si se deja vacío, se busca automáticamente el objeto con tag 'Player'.")]


    public Transform player;
    public float viewDistance = 8f;                  // alcance máximo de la vista
    [Range(1f, 360f)] public float viewAngle = 90f;  // amplitud del cono de visión (grados)


    [Tooltip("Distancia a la que te detecta aunque estés fuera del cono (cercanía/oído).")]
    public float closeDetectionRadius = 1.5f;
    public float eyeHeight = 1.5f;                   // altura de los "ojos" del esqueleto
    [Tooltip("Capas que bloquean la visión (lápidas, muros, árboles). NO incluyas la capa del Player.")]
    public LayerMask obstacleMask;


    [Header("Game Over")]
    public string gameOverSceneName = "GameOver";


    private int currentIndex;        // índice del waypoint al que se dirige
    private float waitTimer;         // cuenta atrás de la espera
    private bool waiting;            // true mientras está parado en un waypoint
    private static bool restarting;  // compartida por todos los esqueletos: evita recargar dos veces




    void Awake()
    {
        restarting = false;

        // Si no arrastraste el Player a mano, lo busca por su tag.
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }
    }

    // BUCLE PRINCIPAL

    void Update()
    {
        Patrol();

        if (CanSeePlayer())
            RestartLevel();
    }

    // PATRULLA

    void Patrol()
    {
        if (waypoints == null || waypoints.Length == 0) return;

        // Fase de espera: cuenta atrás y, al terminar, pasa al siguiente waypoint.
        if (waiting)
        {
            waitTimer -= Time.deltaTime;
            if (waitTimer <= 0f)
            {
                waiting = false;
                currentIndex = (currentIndex + 1) % waypoints.Length; // el % hace que vuelva al 0
            }
            return;
        }

        // Destino: la posición del waypoint, pero manteniendo la altura (Y) del esqueleto.
        Vector3 target = waypoints[currentIndex].position;
        target.y = transform.position.y;

        // ¿Ha llegado?
        if (Vector3.Distance(transform.position, target) <= arrivalDistance)
        {
            waiting = true;
            waitTimer = waitTime;
            return;
        }

        // Girar hacia el destino.
        Vector3 direction = (target - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // Si todavía no está mirando al destino, solo gira pero no camina
        float angleToTarget = Quaternion.Angle(transform.rotation, targetRotation);
        if (angleToTarget > alignAngle) return;

        // Avanzar hacia el destino.
        transform.position = Vector3.MoveTowards(
            transform.position, target, patrolSpeed * Time.deltaTime);
    }

    // ============ DETECCIÓN DEL PLAYER ============

    bool CanSeePlayer()
    {
        if (player == null) return false;

        Vector3 eyePos = transform.position + Vector3.up * eyeHeight;  // origen de la vista
        Vector3 targetPos = player.position + Vector3.up * 1f;         // apunta al pecho del player
        Vector3 toPlayer = targetPos - eyePos;
        float distance = toPlayer.magnitude;

        // 1) Demasiado lejos: no lo ve.
        if (distance > viewDistance) return false;

        // 2) Está muy cerca, o dentro del cono de visión.
        bool inCloseRange = distance <= closeDetectionRadius;
        float angle = Vector3.Angle(transform.forward, new Vector3(toPlayer.x, 0f, toPlayer.z));
        bool inCone = angle <= viewAngle * 0.5f;
        if (!inCone && !inCloseRange) return false;

        // 3) ¿Hay una lápida/muro en medio? Si lo hay, no lo ve.
        if (Physics.Linecast(eyePos, targetPos, obstacleMask)) return false;

        return true;
    }

    // ============ REINICIO DEL NIVEL ============


    void RestartLevel()
    {
        if (restarting) return;   // si el otro esqueleto ya lo hizo, no repetir
        restarting = true;
        Cursor.lockState = CursorLockMode.None;      // libera el ratón para los botones
        Cursor.visible = true;                       // hace visible el cursor
        SceneManager.LoadScene(gameOverSceneName);   // carga la escena de Game Over
    }

    // ============ AYUDA VISUAL (solo en el editor) ============

    void OnDrawGizmosSelected()
    {
        Vector3 eyePos = transform.position + Vector3.up * eyeHeight;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(eyePos, viewDistance);          // alcance de visión

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(eyePos, closeDetectionRadius);  // radio de detección cercana

        Vector3 left = Quaternion.Euler(0, -viewAngle * 0.5f, 0) * transform.forward;
        Vector3 right = Quaternion.Euler(0, viewAngle * 0.5f, 0) * transform.forward;
        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(eyePos, left * viewDistance);          // bordes del cono
        Gizmos.DrawRay(eyePos, right * viewDistance);

        if (waypoints == null) return;
        Gizmos.color = Color.green;
        for (int i = 0; i < waypoints.Length; i++)
        {
            if (waypoints[i] == null) continue;
            Gizmos.DrawSphere(waypoints[i].position, 0.2f);
            Transform next = waypoints[(i + 1) % waypoints.Length];
            if (next != null) Gizmos.DrawLine(waypoints[i].position, next.position);
        }
    }
}
