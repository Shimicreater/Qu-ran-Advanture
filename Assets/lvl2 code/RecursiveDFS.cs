using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// turunan dari MazeLogig
public class RecursiveDFS : MazeLogig
{
    // 4 arah gerak: kanan, atas, kiri, bawah
    public List<MapLocation> directions = new List<MapLocation>()
    {
        new MapLocation(1, 0),
        new MapLocation(0, 1),
        new MapLocation(-1, 0),
        new MapLocation(0, -1)
    };

    // fungsi rekursif untuk membentuk maze
    void Generate(int x, int z)
    {
        if (CountSquareNeighbours(x, z) >= 2)
            return;

        map[x, z] = 0;

        // acak urutan arah tiap kali
        directions.Shuffle();

        Generate(x + directions[0].x, z + directions[0].z);
        Generate(x + directions[1].x, z + directions[1].z);
        Generate(x + directions[2].x, z + directions[2].z);
        Generate(x + directions[3].x, z + directions[3].z);
    }

    public override void GenerateMaps()
    {
        int startX = width / 2;
        int startZ = depth / 2;

        Generate(startX, startZ);

        // setelah maze jadi, buat pintu masuk & keluar
        CreateEntranceAndExit();
    }

    // bikin lubang di sisi kiri & kanan maze
    void CreateEntranceAndExit()
    {
        // ENTRANCE di sisi kiri (x = 0)
        for (int z = 1; z < depth - 1; z++)
        {
            if (map[1, z] == 0)
            {
                map[0, z] = 0;             // lubangi border
                entrance = new MapLocation(0, z);
                break;
            }
        }

        // EXIT di sisi kanan (x = width - 1)
        for (int z = depth - 2; z > 0; z--)
        {
            if (map[width - 2, z] == 0)
            {
                map[width - 1, z] = 0;     // lubangi border kanan
                exit = new MapLocation(width - 1, z);
                break;
            }
        }
    }
}
