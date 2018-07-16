/* 
 * Author: Lim Rui An Ryan
 * Filename: ARCleanCamera.cs
 * Description: An object that contains logical codes involving the clean up scene's logic in accordance to the gamestate
 */
using UnityEngine;

public class ARCleanCamera : MonoBehaviour
{
    /// Public Variables
    public enum Directions
    {
        D_Up = 0,
        D_Down,
        D_Forward,
        D_Right,
        D_Back,
        D_Left,
        D_Max,
    }
    public Vector2 RotationAxisTime = new Vector2(3, 3);
    public GameObject ViewCube;
    public float RotationSpeed = 1;

    /// Private Variables
    private Vector2 RotationFactor;
    private Vector2 RotationAxisSpeed;
    private Vector2 RotationAxisCurrentTime;
    private Vector3 CameraDefault;
    private int OriginCheck = 0;
    private bool ShowUp;
    private bool ShowDown;
    private bool ShowLR;

    // Use this for initialization
    void Start()
    {
        // Default initiallization for rotation parameters
        RotationFactor = new Vector2();
        RotationAxisCurrentTime = RotationAxisTime;
        RotationAxisSpeed = new Vector2(90f / RotationAxisTime.x, 60f / RotationAxisTime.y);
        CameraDefault = gameObject.transform.position;
        ShowLR = ShowUp = true;
        ARCleanDataStore.CameraCurrentDirection = Directions.D_Forward;
        //SetToRotation(0);
        gameObject.transform.LookAt(new Vector3(0, CameraDefault.y-4, 0));
    }

    // Update is called once per frame
    void Update()
    {
        RotationAxisCurrentTime += new Vector2(Time.deltaTime, Time.deltaTime);
        if (RotationAxisCurrentTime.x <= RotationAxisTime.x)
        {
            Quaternion Rot = Quaternion.AngleAxis(RotationAxisSpeed.x * Time.deltaTime * RotationFactor.x, new Vector3(0, 1));
            gameObject.transform.position = Rot * (gameObject.transform.position - new Vector3(0, CameraDefault.y, 0));
            gameObject.transform.position += new Vector3(0, CameraDefault.y, 0);
        }
        else if (OriginCheck%4 == 0 && RotationAxisCurrentTime.y > RotationAxisTime.y && RotationFactor.y < 1)
        {
            gameObject.transform.position = CameraDefault;
            ShowUp = true;
            ShowDown = false;
            ShowLR = true;
        }
        if (RotationAxisCurrentTime.y <= RotationAxisTime.y)
        {
            Quaternion Rot = Quaternion.AngleAxis(RotationAxisSpeed.y * Time.deltaTime * RotationFactor.y, new Vector3(1, 0));
            gameObject.transform.position = Rot * (gameObject.transform.position - new Vector3(0, CameraDefault.y, 0));
            gameObject.transform.position += new Vector3(0, CameraDefault.y, 0);
        }
        gameObject.transform.LookAt(new Vector3(0, CameraDefault.y-4, 0));
        //gameObject.transform.LookAt(new Vector3(0, CameraDefault.y,  0));
        ViewCube.transform.eulerAngles = new Vector3(-gameObject.transform.eulerAngles.x, -gameObject.transform.eulerAngles.y, gameObject.transform.eulerAngles.z);
        SetUIBasedOnFlags();
    }

    // Public Functions
    public void SetToRotation(int Direction)
    {
        Directions DesiredDirection = (Directions)Direction;
        switch (DesiredDirection)
        {
            case Directions.D_Up:
                if (!ShowUp || RotationAxisCurrentTime.y < RotationAxisTime.y)
                    return;
                RotationFactor.y = 1;
                RotationAxisCurrentTime.y = 0;
                ShowDown = true;
                ShowUp = false;
                ShowLR = false;
                ARCleanDataStore.CameraCurrentDirection = Directions.D_Up;
                break;
            case Directions.D_Down:
                if (!ShowDown || RotationAxisCurrentTime.y < RotationAxisTime.y)
                    return;
                RotationFactor.y = -1;
                RotationAxisCurrentTime.y = 0;
                ARCleanDataStore.CameraCurrentDirection = Directions.D_Forward;
                break;
            case Directions.D_Left:
                if (!ShowLR ||RotationAxisCurrentTime.x < RotationAxisTime.x || RotationFactor.y == 1)
                    return;
                OriginCheck++;
                RotationFactor.x = 1;
                RotationAxisCurrentTime.x = 0;
                ARCleanDataStore.CameraCurrentDirection--;
                if (ARCleanDataStore.CameraCurrentDirection < Directions.D_Forward)
                    ARCleanDataStore.CameraCurrentDirection = Directions.D_Left;
                ShowUp = false;
                break;
            case Directions.D_Right:
                if (!ShowLR||RotationAxisCurrentTime.x < RotationAxisTime.x || RotationFactor.y == 1)
                    return;
                OriginCheck--;
                RotationFactor.x = -1;
                RotationAxisCurrentTime.x = 0;
                ARCleanDataStore.CameraCurrentDirection++;
                if (ARCleanDataStore.CameraCurrentDirection > Directions.D_Left)
                    ARCleanDataStore.CameraCurrentDirection = Directions.D_Forward;
                ShowUp = false;
                break;
        }
    }

    public void SetUIBasedOnFlags()
    {
        ARCleanDataStore.ModelAccess.Button_Up.SetActive(ShowUp);
        ARCleanDataStore.ModelAccess.Button_Down.SetActive(ShowDown);
        ARCleanDataStore.ModelAccess.Button_Left.SetActive(ShowLR);
        ARCleanDataStore.ModelAccess.Button_Right.SetActive(ShowLR);
    }
}
