using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLogic : MonoBehaviour
{
    public Transform Player;        // drag Player
    public Transform ViewPoint;     // empty object di atas Player (pivot kamera)
    public float RotationSpeed = 5f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 viewDir = Player.position -
                          new Vector3(transform.position.x,
                                      Player.position.y,
                                      transform.position.z);

        ViewPoint.forward = viewDir.normalized;

        Vector3 inputDir = ViewPoint.forward * verticalInput +
                           ViewPoint.right * horizontalInput;

        if (inputDir != Vector3.zero)
        {
            Player.forward = Vector3.Slerp(
                Player.forward,
                inputDir.normalized,
                Time.deltaTime * RotationSpeed
            );
        }
    }
}




