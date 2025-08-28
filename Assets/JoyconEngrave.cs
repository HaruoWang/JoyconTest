using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
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

    // public RectTransform cursorUI;

    public GameObject cursor;


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
        // if (ggyro >= 20 && go == false)
        // {
        //     stepCount += 1;
        //     if (p)
        //     {
        //         p.AddRelativeForce(Vector3.forward * _speed);
        //     }

        //     go = true;
        // }

        // if (ggyro < 20 && go == true)
        // {
        //     go = false;
        // }

        if (joycons.Count == 0) return;

        Joycon j = joycons[jcIndex];

        gyro = j.GetGyro();

        float GetGyroX = gyro[0];
        float GetGyroY = gyro[1] * -1;
        float GetGyroZ = gyro[2];

        if (j.GetButtonDown(Joycon.Button.SHOULDER_2))
        {
            Debug.Log("Shoulder button 2 pressed");
            Debug.Log(string.Format("Stick x: {0:N} Stick y: {1:N}", j.GetStick()[0], j.GetStick()[1]));
            j.Recenter();
        }

        float[] stickArr = j.GetStick();
        float stickX = stickArr[0];
        float stickY = stickArr[1];
        float cursorX = (stickX + 1f) / 2f * Screen.width;
        float cursorY = (stickY + 1f) / 2f * Screen.height;
        cursorY = Screen.height - cursorY;

        //orientation = j.GetVector();
        //rot = orientation.eulerAngles;
        //float X = (float)Math.Round(orientation[0], 1);
        //float Y = (float)Math.Round(orientation[1], 1);
        //float Z = (float)Math.Round(orientation[2], 1);
        //float X = rot[0] / 360;
        //float Y = rot[1] / 360;
        //float Z = rot[2] / 360;

        //Debug.Log($"{rot}");

        //Debug.Log($"陀螺儀： 0, 0, 0");
        //Debug.Log($"{orientation}");
        // Debug.Log($"Cursor Position: {cursorX}, {cursorY}");
        // 目前按著b2 cursor，座標有反應，等等改
        // Vector2 stick = new Vector2(stickArr[0], stickArr[1]);
        // int x = Mathf.Clamp(Mathf.RoundToInt((stick.x + 1f) / 2f * (gridSize - 1)), 0, gridSize - 1);
        // int y = Mathf.Clamp(Mathf.RoundToInt((stick.y + 1f) / 2f * (gridSize - 1)), 0, gridSize - 1);
        // int z = gridSize / 2;

        // cursorUI.position = new Vector2(cursorX, cursorY);
        //cursor.transform.position = new Vector3(X, Y, Z);

        Quaternion q = j.GetVector();

        // 取得三軸方向
        Vector3 forward = q * Vector3.forward;
        Vector3 up = q * Vector3.up;
        Vector3 right = q * Vector3.right * -1;

        float x = stickX * 3.5f - 0.3f;
        float y = stickY;
        float z = forward.z * 5f + 2f;

        //Vector3 p = (forward + up + right) / 3;

        // 假設游標基準點在 (0,0,0)，讓它隨著 forward 移動
        cursor.transform.position = new Vector3(x, y, z);

        // Mouse.current.WarpCursorPosition(new Vector2(cursorX, cursorY));

        // Debug.Log(cursor.transform.position);

        if (j.GetButtonDown(Joycon.Button.SHOULDER_2))
        {
            // CarveVoxel(x, y, z);
        }

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

    void OnTriggerEnter(Collider wall)
    {
        Debug.Log("Hit the wall");
        GameObject.Destroy(wall.gameObject);
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