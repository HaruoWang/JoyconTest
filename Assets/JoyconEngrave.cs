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


    public float[] stick;
    public Vector3 gyro;
    public Vector3 accel;
    public int jc_ind = 0;
    public Quaternion orientation;

    public Vector3 rot;
    public float xx, yy, zz, stepCount;
    public float ggyro;
    public float aaccel;
    public bool go;

    public Rigidbody p;
    public float _speed;


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
        if (ggyro >= 20 && go == false)
        {
            stepCount += 1;
            if (p)
            {
                p.AddRelativeForce(Vector3.forward * _speed);
            }

            go = true;
        }

        if (ggyro < 20 && go == true)
        {
            //stepCount += 1;
            go = false;
        }

        // Debug.Log(new Vector2(joycons[jc_ind].GetStick()[0], joycons[jc_ind].GetStick()[1]));

        if (joycons.Count == 0) return;

        Joycon j = joycons[jcIndex];

        if (j.GetButtonDown(Joycon.Button.SHOULDER_2))
        {
            Debug.Log("Shoulder button 2 pressed");
            Debug.Log(string.Format("Stick x: {0:N} Stick y: {1:N}", j.GetStick()[0], j.GetStick()[1]));
            j.Recenter();
        }

        float[] stickArr = j.GetStick();
        Vector2 stick = new Vector2(stickArr[0], stickArr[1]);
        int x = Mathf.Clamp(Mathf.RoundToInt((stick.x + 1f) / 2f * (gridSize - 1)), 0, gridSize - 1);
        int y = Mathf.Clamp(Mathf.RoundToInt((stick.y + 1f) / 2f * (gridSize - 1)), 0, gridSize - 1);
        int z = gridSize / 2;

        if (j.GetButtonDown(Joycon.Button.SHOULDER_2))
        {
            CarveVoxel(x, y, z);
        }
        
        // stick = j.GetStick();

            gyro = j.GetGyro();
            accel = j.GetAccel();
            orientation = j.GetVector();

            rot = orientation.eulerAngles;
            ggyro = (int)gyro.sqrMagnitude;
            aaccel = (int)accel.sqrMagnitude;

            if (j.GetButton(Joycon.Button.DPAD_UP))
            {
                gameObject.GetComponent<Renderer>().material.color = Color.red;
            }
            else
            {
                gameObject.GetComponent<Renderer>().material.color = Color.blue;
            }
            gameObject.transform.rotation = orientation;
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
