using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeTest : MonoBehaviour {

    public ARSwipe swipeControls;

    public ARCleanCamera CameraOBJ;

    public bool OBJInHand = false;
	// Use this for initialization
	void Start () {
		
	}
	

    public bool SetOBJInHand(bool OBJIH)
    {
        OBJInHand = OBJIH;
        return OBJInHand;
    }

	// Update is called once per frame
	void Update () {
       
        if (swipeControls.SwipeUp && !OBJInHand)
        {            
            CameraOBJ.SetToRotation(0);
        }
        if (swipeControls.SwipeDown && !OBJInHand)
        {           
            CameraOBJ.SetToRotation(1);
        }
        //if (swipeControls.SwipeLeft && !OBJInHand)
        //{

        //    CameraOBJ.SetToRotation(5);
        //}
        //if (swipeControls.SwipeRight && !OBJInHand )
        //{

        //    CameraOBJ.SetToRotation(3);
        //}
    }
}
