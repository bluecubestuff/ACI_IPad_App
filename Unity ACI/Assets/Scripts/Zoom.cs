using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class Zoom : MonoBehaviour {

    public DragCamera DragReference;
    public Slider ZoomSlider;
    public Camera ZoomCamera;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonUp(0))
        {
            EnableDrag(true);
        }

    }

    public void EnableDrag(bool isEnabled)
    {
        DragReference.enabled = isEnabled;
    }

    public void SetZoom()
    {
        //Get the Fill Amount of the Slider
        //Set the Y based on the Fill Amount
        Vector3 NewZoomPos = DragReference.MainCamera.transform.position;
        NewZoomPos.y = ZoomSlider.value;
        NewZoomPos.z = -ZoomSlider.value;
        DragReference.MainCamera.transform.position = NewZoomPos;
    }

}
