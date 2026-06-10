using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBGM : MonoBehaviour
{
    [SerializeField] private AudioSource bgmSource;
    private const string VolumeKey = "MasterVolume";

    private void Awake()
    {
        float v = PlayerPrefs.GetFloat(VolumeKey, 1f);

        // set volume global (opsional)
        AudioListener.volume = v;

        // set volume khusus BGM level
        if (bgmSource != null)
        {
            bgmSource.volume = v;
        }
    }
}
