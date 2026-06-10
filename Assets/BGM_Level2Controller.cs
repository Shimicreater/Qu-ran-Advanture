using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM_Level2Controller : MonoBehaviour
{
    [Header("Clips")]
    public AudioClip normalClip;   // lagu biasa (explore maze)
    public AudioClip dangerClip;   // lagu tegang (dekat musuh)

    private AudioSource audioSource;
    private string volumeKey = "Volume_Level2";

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) return;

        float savedVolume = PlayerPrefs.GetFloat(volumeKey, 1f);
        audioSource.volume = savedVolume;

        audioSource.loop = true;   // penting: BGM loop

        if (normalClip == null)
        {
            normalClip = audioSource.clip;
        }

        if (audioSource.clip == null && normalClip != null)
        {
            audioSource.clip = normalClip;
        }

        if (!audioSource.isPlaying && audioSource.clip != null)
            audioSource.Play();
    }

    public void PlayNormal()
    {
        if (audioSource == null || normalClip == null) return;
        if (audioSource.clip == normalClip) return;

        Debug.Log("[BGM] PlayNormal");
        audioSource.clip = normalClip;
        audioSource.Play();
    }

    public void PlayDanger()
    {
        if (audioSource == null || dangerClip == null) return;
        if (audioSource.clip == dangerClip) return;

        Debug.Log("[BGM] PlayDanger");
        audioSource.clip = dangerClip;
        audioSource.Play();
    }
}
