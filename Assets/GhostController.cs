using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class GhostController : MonoBehaviour
{
    [Header("Target & Jarak")]
    public Transform target;              // akan diisi otomatis dari tag "Player"
    public float chaseRange = 10f;        // jarak mulai mengejar
    public float wanderRadius = 5f;       // radius patroli di sekitar posisi awal

    [Header("Floating Effect")]
    public float floatAmplitude = 0.2f;   // tinggi naik-turun
    public float floatSpeed = 2f;         // kecepatan naik-turun

    [Header("Music")]
    public BGM_Level2Controller bgmController; // akan dicari otomatis
    public float dangerMusicRange = 8f;        // jarak trigger musik danger

    [Header("Game Over")]
    public string gameOverSceneName = "GameOverL2";

    private NavMeshAgent agent;
    private Vector3 startPos;
    private float baseOffset;
    private bool isInDangerRange = false;
    private float distanceToTarget;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        startPos = transform.position;

        if (agent != null)
        {
            baseOffset = agent.baseOffset;   // tinggi dasar ghost di atas NavMesh
            agent.updateRotation = false;    // rotasi kita yang atur
        }

        // ==== cari Player otomatis ====
        if (target == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                target = playerObj.transform;
            else
                Debug.LogWarning("[Ghost] Player dengan tag 'Player' tidak ditemukan!");
        }

        // ==== cari BGM controller otomatis ====
        if (bgmController == null)
        {
            bgmController = FindObjectOfType<BGM_Level2Controller>();
            if (bgmController == null)
                Debug.LogWarning("[Ghost] BGM_Level2Controller tidak ditemukan di scene!");
        }
    }

    void Update()
    {
        if (agent == null || target == null) return;

        distanceToTarget = Vector3.Distance(transform.position, target.position);

        // ---------- MUSIK ----------
        if (bgmController != null)
        {
            bool nowDanger = distanceToTarget <= dangerMusicRange;

            if (nowDanger && !isInDangerRange)
                bgmController.PlayDanger();
            else if (!nowDanger && isInDangerRange)
                bgmController.PlayNormal();

            isInDangerRange = nowDanger;
        }
        // ---------------------------

        // ---------- AI KEJAR / PATROLI ----------
        if (distanceToTarget <= chaseRange)
        {
            // kejar player
            agent.isStopped = false;
            agent.SetDestination(target.position);

            Vector3 dir = target.position - transform.position;
            dir.y = 0f;
            if (dir.sqrMagnitude > 0.01f)
            {
                Quaternion rot = Quaternion.LookRotation(dir);
                transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * 5f);
            }
        }
        else
        {
            // patroli sekitar startPos
            if (!agent.hasPath || agent.remainingDistance < 0.5f)
            {
                Vector2 rnd = Random.insideUnitCircle * wanderRadius;
                Vector3 dest = startPos + new Vector3(rnd.x, 0f, rnd.y);
                agent.isStopped = false;
                agent.SetDestination(dest);
            }
        }

        // ---------- EFEK MELAYANG (pakai baseOffset, bukan posisi Y) ----------
        if (agent != null)
        {
            agent.baseOffset = baseOffset +
                               Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CatchPlayer();
        }
    }

    void CatchPlayer()
    {
        Debug.Log("[Ghost] Player tertangkap!");

        if (!string.IsNullOrEmpty(gameOverSceneName))
        {
            SceneManager.LoadScene(gameOverSceneName);
        }
        else
        {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
    }
}
