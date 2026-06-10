using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    private MovementLogic movementLogic;

    private void Start()
    {
        movementLogic = GetComponentInParent<MovementLogic>();

        if (movementLogic == null)
        {
            Debug.LogError("GroundCheck: MovementLogic tidak ditemukan di parent!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // kalau mau lebih rapih, aktifkan tag Ground:
        // if (!other.CompareTag("Ground")) return;

        movementLogic.groundedchanger();
    }

    private void OnTriggerStay(Collider other)
    {
        // if (!other.CompareTag("Ground")) return;

        movementLogic.groundedchanger();
    }
}