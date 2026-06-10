using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class MovementLogic : MonoBehaviour
{
    public Transform PlayerOrientation;
    public Animator anim;
    public CameraLogic camlogic;
    public float walkspeed, runspeed;   // ← tetap sama

    private Rigidbody rb;
    float horizontalInput;
    float verticalInput;
    Vector3 moveDirection;
    bool grounded = true;

    // =========================
    //  SFX (TAMBAHAN)
    // =========================
    [Header("SFX")]
    public AudioClip StepAudio;     // drag clip langkah ke sini
    AudioSource PlayerAudio;        // AudioSource di Player

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        PlayerOrientation = GetComponent<Transform>();

        // ambil AudioSource di Player (harus ada komponen AudioSource)
        PlayerAudio = GetComponent<AudioSource>();

    }

    void Update()
    {
        if (Time.timeScale == 0f)
        {
            anim.SetBool("Walk", false);
            anim.SetBool("Run", false);
            return;
        }

        Movement();
    }

    public void Movement()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        moveDirection = PlayerOrientation.forward * verticalInput +
                        PlayerOrientation.right * horizontalInput;

        if (grounded && moveDirection != Vector3.zero)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                anim.SetBool("Run", true);
                anim.SetBool("Walk", false);
                rb.AddForce(moveDirection.normalized * runspeed * 10f, ForceMode.Force);
            }
            else
            {
                anim.SetBool("Walk", true);
                anim.SetBool("Run", false);
                rb.AddForce(moveDirection.normalized * walkspeed * 10f, ForceMode.Force);
            }
        }
        else
        {
            anim.SetBool("Walk", false);
            anim.SetBool("Run", false);
        }
    }

    // Masih bisa dipakai dari script lain / collision untuk set grounded
    public void groundedchanger()
    {
        grounded = true;
    }

    // =========================
    //  DIPANGGIL DARI ANIMATION EVENT (TAMBAHAN)
    // =========================
    public void step()
    {
        Debug.Log("step");

        if (PlayerAudio == null || StepAudio == null) return;

        PlayerAudio.clip = StepAudio;
        PlayerAudio.Play();
    }
}
