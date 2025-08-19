using System.Collections.Generic;
using UnityEngine;

public class JoyconEngrave : MonoBehaviour
{
    private List<Joycon> joycons;
    public int jcIndex = 0;

    public GameObject voxelPrefab;
    private GameObject[,,] voxelGrid;
    public int gridSize = 10;
    public float voxelSpacing = 1f;

    void Start()
    {
        voxelGrid = new GameObject[gridSize, gridSize, gridSize];
        for (int x = 0; x < gridSize; x++)
            for (int y = 0; y < gridSize; y++)
                for (int z = 0; z < gridSize; z++)
                {
                    Vector3 pos = new Vector3(x, y, z) * voxelSpacing;
                    voxelGrid[x, y, z] = GameObject.Find("Cube");
                }

        joycons = JoyconManager.Instance.j;
        if (joycons.Count < jcIndex + 1)
            Debug.LogError("Joycon not found");
    }

    void Update()
    {
        if (joycons.Count == 0) return;

        Joycon j = joycons[jcIndex];

        float[] stickArr = j.GetStick();
        Vector2 stick = new Vector2(stickArr[0], stickArr[1]);
        int x = Mathf.Clamp(Mathf.RoundToInt((stick.x + 1f) / 2f * (gridSize - 1)), 0, gridSize - 1);
        int y = Mathf.Clamp(Mathf.RoundToInt((stick.y + 1f) / 2f * (gridSize - 1)), 0, gridSize - 1);
        int z = gridSize / 2;

        if (j.GetButtonDown(Joycon.Button.SHOULDER_2))
        {
            CarveVoxel(x, y, z);
        }
    }

    void CarveVoxel(int x, int y, int z)
    {
        if (x >= 0 && x < gridSize &&
            y >= 0 && y < gridSize &&
            z >= 0 && z < gridSize)
        {
            if (voxelGrid[x, y, z] != null)
            {
                Destroy(voxelGrid[x, y, z]);
                voxelGrid[x, y, z] = null;
            }
        }
    }
}
