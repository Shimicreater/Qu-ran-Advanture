using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// --- helper koordinat ---
public class MapLocation
{
    public int x;
    public int z;

    public MapLocation(int _x, int _z)
    {
        x = _x;
        z = _z;
    }
}

public class MazeLogig : MonoBehaviour
{
    [Header("Maze Size")]
    public int width = 30;
    public int depth = 30;
    public int scale = 6;

    public float groundY = 0f;

    [Header("Prefabs")]
    public GameObject Cube;          // dinding / building
    public Transform player;         // karakter player
    public GameObject ExitPadPrefab; // ExitPad_L2

    [Header("Enemy")]
    public GameObject enemyPrefab;   // prefab jin / ghost
    public int enemyCount = 2;       // <<< JUMLAH MUSUH

    [Header("Letters In Maze")]
    public GameObject[] letterPrefabs;   // prefab huruf (ha, kho, dal, dzal, ra, dst)
    public int lettersToSpawn = 5;       // berapa huruf mau di-spawn
    public float letterHeightOffset = 0.5f; // tinggi huruf di atas ground

    [HideInInspector] public byte[,] map;        // 1 = wall, 0 = corridor
    [HideInInspector] public MapLocation entrance;
    [HideInInspector] public MapLocation exit;

    void Start()
    {
        InitialiseMap();
        GenerateMaps();   // dioverride di RecursiveDFS
        DrawMaps();
        PlaceCharacter(); // spawn player di pintu masuk
        PlaceExitPad();   // spawn exit pad di pintu keluar
        PlaceEnemies();   // <<< spawn BANYAK enemy
        PlaceLetters();   // spawn huruf hijaiyah
    }

    void InitialiseMap()
    {
        map = new byte[width, depth];
        for (int z = 0; z < depth; z++)
        {
            for (int x = 0; x < width; x++)
            {
                map[x, z] = 1;    // default = wall
            }
        }
    }

    // default random; akan dioverride oleh RecursiveDFS
    public virtual void GenerateMaps()
    {
        for (int z = 0; z < depth; z++)
        {
            for (int x = 0; x < width; x++)
            {
                if (Random.Range(0, 100) < 50)
                    map[x, z] = 0;
            }
        }
    }

    // gambar semua dinding
    void DrawMaps()
    {
        for (int z = 0; z < depth; z++)
        {
            for (int x = 0; x < width; x++)
            {
                if (map[x, z] == 1)
                {
                    float halfWall = scale * 0.5f;

                    Vector3 pos = new Vector3(
                        x * scale,
                        groundY + halfWall,   // tembok duduk di atas ground
                        z * scale
                    );

                    GameObject wall = Instantiate(Cube, pos, Quaternion.identity);
                    wall.transform.localScale = new Vector3(scale, scale, scale);
                }
            }
        }
    }

    // dipakai algoritma untuk cek tetangga lorong (atas/bawah/kiri/kanan)
    public int CountSquareNeighbours(int x, int z)
    {
        if (x <= 0 || z <= 0 || x >= width - 1 || z >= depth - 1)
            return 5;

        int count = 0;

        if (map[x + 1, z] == 0) count++;
        if (map[x - 1, z] == 0) count++;
        if (map[x, z + 1] == 0) count++;
        if (map[x, z - 1] == 0) count++;

        return count;
    }

    // --- PLAYER DI PINTU MASUK ---
    void PlaceCharacter()
    {
        if (player == null) return;

        // kalau entrance sudah di-set oleh RecursiveDFS, pakai itu
        if (entrance != null)
        {
            Vector3 pos = new Vector3(
                entrance.x * scale,
                player.position.y,
                entrance.z * scale
            );
            player.position = pos;
            return;
        }

        // fallback lama: random corridor
        List<Vector3> corridors = new List<Vector3>();

        for (int z = 0; z < depth; z++)
        {
            for (int x = 0; x < width; x++)
            {
                if (map[x, z] == 0)
                {
                    Vector3 p = new Vector3(x * scale, player.position.y, z * scale);
                    corridors.Add(p);
                }
            }
        }

        if (corridors.Count == 0) return;

        Vector3 spawnPos = corridors[Random.Range(0, corridors.Count)];
        player.position = spawnPos;
    }

    // --- EXIT PAD DI PINTU KELUAR ---
    void PlaceExitPad()
    {
        if (ExitPadPrefab == null || exit == null) return;

        float padHalfHeight = ExitPadPrefab.transform.localScale.y * 0.5f;

        Vector3 pos = new Vector3(
            exit.x * scale,
            groundY + padHalfHeight,
            exit.z * scale
        );

        Instantiate(ExitPadPrefab, pos, ExitPadPrefab.transform.rotation);
    }

    // --- MULTI ENEMY DI MAZE ---
    void PlaceEnemies()
    {
        if (enemyPrefab == null) return;
        if (enemyCount <= 0) return;

        // kumpulkan semua koridor
        List<MapLocation> corridors = new List<MapLocation>();
        for (int z = 0; z < depth; z++)
        {
            for (int x = 0; x < width; x++)
            {
                if (map[x, z] == 0)
                {
                    // jangan spawn di entrance / exit
                    if (entrance != null && x == entrance.x && z == entrance.z) continue;
                    if (exit != null && x == exit.x && z == exit.z) continue;

                    corridors.Add(new MapLocation(x, z));
                }
            }
        }
        if (corridors.Count == 0) return;

        int spawnCount = Mathf.Min(enemyCount, corridors.Count);

        for (int i = 0; i < spawnCount; i++)
        {
            // ambil cell random dan buang dari list supaya tidak dobel
            int idx = Random.Range(0, corridors.Count);
            MapLocation cell = corridors[idx];
            corridors.RemoveAt(idx);

            // kalau terlalu dekat dengan exit, pilih ulang sekali lagi
            if (exit != null)
            {
                float dx = cell.x - exit.x;
                float dz = cell.z - exit.z;
                float d2 = dx * dx + dz * dz;
                if (d2 <= 3 * 3 && corridors.Count > 0)
                {
                    int newIdx = Random.Range(0, corridors.Count);
                    cell = corridors[newIdx];
                    corridors.RemoveAt(newIdx);
                }
            }

            Vector3 enemyPos = new Vector3(
                cell.x * scale,
                groundY + 1f,
                cell.z * scale
            );

            Debug.Log($"SPAWN ENEMY #{i + 1} di grid: {cell.x},{cell.z}");

            GameObject enemy = Instantiate(enemyPrefab, enemyPos, Quaternion.identity);

            // ====== SET TARGET OTOMATIS (jin / Ghost) ======
            if (player != null)
            {
                var jinLogic = enemy.GetComponent<jin>();
                if (jinLogic != null)
                    jinLogic.target = player;

                var ghostLogic = enemy.GetComponent<GhostController>();
                if (ghostLogic != null)
                    ghostLogic.target = player;
            }
        }
    }

    // --- HURUF HIJAIYAH DI MAZE ---
    void PlaceLetters()
    {
        if (letterPrefabs == null || letterPrefabs.Length == 0) return;
        if (lettersToSpawn <= 0) return;

        // kumpulkan semua cell koridor kecuali entrance & exit
        List<MapLocation> corridors = new List<MapLocation>();
        for (int z = 0; z < depth; z++)
        {
            for (int x = 0; x < width; x++)
            {
                if (map[x, z] == 0)
                {
                    if (entrance != null && x == entrance.x && z == entrance.z) continue;
                    if (exit != null && x == exit.x && z == exit.z) continue;

                    corridors.Add(new MapLocation(x, z));
                }
            }
        }
        if (corridors.Count == 0) return;

        int spawnCount = Mathf.Min(lettersToSpawn,
                                   corridors.Count,
                                   letterPrefabs.Length);

        // daftar index huruf yang masih belum dipakai (0..N-1)
        List<int> availableLetters = new List<int>();
        for (int i = 0; i < letterPrefabs.Length; i++)
            availableLetters.Add(i);

        for (int i = 0; i < spawnCount; i++)
        {
            // pilih cell koridor random (unik)
            int cellIdx = Random.Range(0, corridors.Count);
            MapLocation cell = corridors[cellIdx];
            corridors.RemoveAt(cellIdx);

            // pilih index huruf random dari yang BELUM dipakai
            int listIdx = Random.Range(0, availableLetters.Count);
            int prefabIndex = availableLetters[listIdx];
            availableLetters.RemoveAt(listIdx);   // buang supaya gak bisa kepilih lagi

            GameObject prefab = letterPrefabs[prefabIndex];

            Vector3 pos = new Vector3(
                cell.x * scale,
                groundY + letterHeightOffset,
                cell.z * scale
            );

            Instantiate(prefab, pos, prefab.transform.rotation);

            Debug.Log($"[MazeLogig] Spawn letter {prefab.name} (idx {prefabIndex}) di ({cell.x},{cell.z})");
        }
    }
}
