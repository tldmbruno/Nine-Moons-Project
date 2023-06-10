using System.Collections.Generic;
using UnityEngine;

public class ChunkBehavior : MonoBehaviour
{
    private const int CHUNK_SIZE = 16;
    private const int SURFACE_HEIGHT = 2;

    private List<GameObject> _voxels = new List<GameObject>();
    private GameObject[] _voxelPrefabs;

    void Start()
    {
        _voxelPrefabs = Resources.LoadAll<GameObject>("Voxels/");

        for (int yAxis = 0; yAxis < SURFACE_HEIGHT; yAxis++)
        {
            for (int zAxis = 0; zAxis < CHUNK_SIZE; zAxis++)
            {
                for (int xAxis = 0; xAxis < CHUNK_SIZE; xAxis++)
                {
                    _voxels.Add(Instantiate(_voxelPrefabs[0], new Vector3(xAxis, yAxis, zAxis), Quaternion.identity, transform));
                }
            }
        }
    }
}
