using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;  

public class jin : MonoBehaviour
{
    [Header("Target & Jarak")]
    public Transform target;          // drag Player di Inspector
    public float ChaseRange = 10f;    // jarak mulai ngejar
    public float turnSpeed = 15f;     // kecepatan noleh ke player

    private NavMeshAgent agent;
    private Animator anim;
    private float DistanceToTarget;

    // =========================
    //  SFX
    // =========================
    [Header("SFX")]
    public AudioClip WalkAudio;       // suara langkah jin
    public AudioClip TangkapAudio;    // suara saat berhasil menangkap
    private AudioSource jinAudio;     // AudioSource di jin

    // =========================
    //  MUSIC (BGM)
    // =========================
    [Header("Music")]
    public BGM_Level2Controller bgmController; // drag BGM_Level2 di Inspector
    public float dangerMusicRange = 8f;        // jarak mulai lagu tegang
    private bool isInDangerRange = false;

    private void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        anim = GetComponent<Animator>();     // pastikan Animator ada di gameobject yg sama

        // ambil AudioSource (harus ada component AudioSource di jin)
        jinAudio = GetComponent<AudioSource>();

        // ====== CARI PLAYER OTOMATIS ======
        // kalau Target di Inspector belum diisi (prefab tidak bisa isi Player)
        if (target == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                target = playerObj.transform;
            }
            else
            {
                Debug.LogWarning("[Jin] Player dengan tag 'Player' tidak ditemukan!");
            }
        }

        // ====== CARI BGM CONTROLLER OTOMATIS ======
        if (bgmController == null)
        {
            bgmController = FindObjectOfType<BGM_Level2Controller>();
            if (bgmController == null)
            {
                Debug.LogWarning("[Jin] BGM_Level2Controller tidak ditemukan di scene!");
            }
        }
    }


    private void Update()
    {
        if (target == null) return;

        // hitung jarak ke player
        DistanceToTarget = Vector3.Distance(target.position, transform.position);

        // ---------- LOGIKA MUSIK ----------
        if (bgmController != null)
        {
            bool nowDanger = DistanceToTarget <= dangerMusicRange;

            if (nowDanger && !isInDangerRange)
            {
                Debug.Log($"[Jin] MASUK danger, jarak = {DistanceToTarget:F2}");
                bgmController.PlayDanger();
            }
            else if (!nowDanger && isInDangerRange)
            {
                Debug.Log($"[Jin] KELUAR danger, jarak = {DistanceToTarget:F2}");
                bgmController.PlayNormal();
            }

            isInDangerRange = nowDanger;
        }
        // ----------------------------------

        // kalau di luar ChaseRange → idle
        if (DistanceToTarget > ChaseRange)
        {
            SetIdle();
            return;
        }

        FaceTarget(target.position);

        if (DistanceToTarget >= agent.stoppingDistance)
        {
            ChaseTarget();
        }
        else
        {
            Tangkap();
        }
    }

    // ==================== STATE: IDLE ====================
    void SetIdle()
    {
        agent.isStopped = true;

        anim.SetBool("Walk", false);
        anim.SetBool("Tangkap", false);
    }

    // ==================== STATE: WALK (KEJAR) ====================
    void ChaseTarget()
    {
        agent.isStopped = false;
        agent.SetDestination(target.position);

        anim.SetBool("Walk", true);
        anim.SetBool("Tangkap", false);
    }

    // ==================== STATE: TANGKAP ====================
    void Tangkap()
    {
        agent.isStopped = true;

        anim.SetBool("Walk", false);
        anim.SetBool("Tangkap", true);
        // belum game over di sini, cuma anim dulu
    }

    // ==================== FACE TARGET ====================
    void FaceTarget(Vector3 destination)
    {
        Vector3 direction = (destination - transform.position).normalized;
        Vector3 flatDir = new Vector3(direction.x, 0f, direction.z);

        if (flatDir.sqrMagnitude > 0.001f)
        {
            Quaternion lookRotation = Quaternion.LookRotation(flatDir);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                lookRotation,
                Time.deltaTime * turnSpeed
            );
        }
    }

    // ==================== DIPANGGIL DARI ANIMATION EVENT ====================
    // event di anim jalan (footstep jin)
    public void StepJin()
    {
        if (jinAudio == null || WalkAudio == null) return;

        jinAudio.PlayOneShot(WalkAudio);
    }

    // event di anim tangkap, di frame ketika kena
    public void HitConnect()
    {
        if (DistanceToTarget <= agent.stoppingDistance)
        {
            // mainkan suara tangkap dulu
            if (jinAudio != null && TangkapAudio != null)
            {
                jinAudio.PlayOneShot(TangkapAudio);
            }

            Debug.Log("PLAYER KETANGKAP! Pindah ke GameOverL2");
            SceneManager.LoadScene("GameOverL2");   // nama harus sama persis dengan scene
        }
    }

    // ==================== GIZMOS ====================
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, ChaseRange);
    }
}
