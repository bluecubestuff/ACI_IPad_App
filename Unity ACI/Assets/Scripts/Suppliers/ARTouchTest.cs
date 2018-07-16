using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ARTouchTest : MonoBehaviour {
    RaycastHit hit;

    Text text;
    // Use this for initialization
    void Start()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Vector3 mousePosFar = new Vector3(Input.mousePosition.x,
                                               Input.mousePosition.y,
                                               Camera.allCameras[0].farClipPlane);
            Vector3 mousePosNear = new Vector3(Input.mousePosition.x,
                                               Input.mousePosition.y,
                                               Camera.allCameras[0].nearClipPlane);
            Vector3 mousePosF = Camera.allCameras[0].ScreenToWorldPoint(mousePosFar);
            Vector3 mousePosN = Camera.allCameras[0].ScreenToWorldPoint(mousePosNear);

            Debug.DrawRay(mousePosN, mousePosF - mousePosN, Color.green);




            if (Physics.Raycast(mousePosN, mousePosF - mousePosN, out hit))
            {
                text.text = hit.transform.name;

            }
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
