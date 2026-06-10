using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterRandomSpawner : MonoBehaviour
{
    [Header("Prefab huruf (lif, Ba, dll)")]
    public GameObject[] letterPrefabs;      // drag prefab lif, Ba, dst

    [Header("Titik spawn di map")]
    public Transform[] spawnPoints;         // drag Point_1, Point_2, dst

    [Header("Jumlah huruf yang mau dimunculkan")]
    public int lettersToSpawn = 5;

    private void Start()
    {
        SpawnLetters();
    }

    public void SpawnLetters()
    {
        if (letterPrefabs == null || letterPrefabs.Length == 0) return;
        if (spawnPoints == null || spawnPoints.Length == 0) return;

        int maxSpawn = Mathf.Min(lettersToSpawn,
                                 spawnPoints.Length,
                                 letterPrefabs.Length);

        List<int> usedPoints = new List<int>();
        List<int> usedPrefabs = new List<int>();

        for (int i = 0; i < maxSpawn; i++)
        {
            // pilih point yang belum dipakai
            int pointIndex;
            do
            {
                pointIndex = Random.Range(0, spawnPoints.Length);
            } while (usedPoints.Contains(pointIndex));
            usedPoints.Add(pointIndex);

            // pilih prefab yang belum dipakai
            int prefabIndex;
            do
            {
                prefabIndex = Random.Range(0, letterPrefabs.Length);
            } while (usedPrefabs.Contains(prefabIndex));
            usedPrefabs.Add(prefabIndex);

            Transform point = spawnPoints[pointIndex];
            GameObject prefab = letterPrefabs[prefabIndex];

            Instantiate(prefab, point.position, point.rotation);
        }
    }
}
