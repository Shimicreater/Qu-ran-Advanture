using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootstep : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] stepClips;
    [SerializeField][Range(0f, 1f)] private float stepVolume = 0.7f;

    // Dipanggil dari Animation Event
    public void Step()
    {
        if (audioSource == null) return;
        if (stepClips == null || stepClips.Length == 0) return;

        int index = Random.Range(0, stepClips.Length);
        audioSource.PlayOneShot(stepClips[index], stepVolume);
    }
}
