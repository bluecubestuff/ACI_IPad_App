using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DragCamera : MonoBehaviour {

    private Vector3 touchPos;
    public Camera MainCamera;
    public bool IsDragging = false;

    private Vector2 MouseDownPos, MouseUpPos;
    private Vector2 OldPos, NewPos;
    public float HorizontalSensitivity;
    public float VerticalSensitivity;

    public float minX,maxX,minZ,maxZ;

    private float holdTimer;

	// Use this for initialization
	void Start () {
        OldPos = new Vector2();
        NewPos = new Vector2();
	}
	
	// Update is called once per frame
	void Update ()
    {

        //True if Holding
        if (Input.GetKey(KeyCode.Mouse0) )
        {
            holdTimer += Time.deltaTime;

            if (holdTimer > 0.05)
                IsDragging = true;
            //True if NewPos is empty(First Frame)
            if (NewPos == new Vector2(0, 0))
            {
                //Set it to Mouse Position and return since it's still the first frame
                NewPos = Input.mousePosition;
                return;
            }

            //We have a NewPos now
            OldPos = NewPos;

            //Update the NewPos with the New Mouse Position
            NewPos = Input.mousePosition;

            Vector2 MouseDiff = NewPos - OldPos;


            //Set the New Camera Position
            Vector3 NewCameraPosition = MainCamera.transform.position;

            //Convet the MouseDiff to the new distance the Camera moves by
            
            NewCameraPosition.x -= MouseDiff.x * HorizontalSensitivity * Time.deltaTime;
            NewCameraPosition.z -= MouseDiff.y * VerticalSensitivity * Time.deltaTime;


            if (NewCameraPosition.x <= minX)
                NewCameraPosition.x = minX;
            else if (NewCameraPosition.x >= maxX)
                NewCameraPosition.x = maxX;
            if (NewCameraPosition.z <= minZ)
                NewCameraPosition.z = minZ;
            if (NewCameraPosition.z >= maxZ)
                NewCameraPosition.z = maxZ;
            MainCamera.transform.position = NewCameraPosition;

        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            IsDragging = false;
            OldPos = new Vector2();
            NewPos = new Vector2();
            holdTimer = 0.0f;
        }
    }
}
