using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TestManager : MonoBehaviour {

    public Camera MainCam;
    public Camera FirstCam;
    public Camera MAINMAIN;

    public Camera BeefCam;
    public Camera ChickenCam;
    public Camera FishCam;

    public Button FirstButton;
    public Button BackToOverview;
    public Button LeftButton;
    public Button RightButton;

    int currentCamera = 2;
    bool indoor = false;

	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {

        if (currentCamera == 1)
            LeftButton.interactable = false;
        else
            LeftButton.interactable = true;

        if (currentCamera == 3)
            RightButton.interactable = false;
        else
            RightButton.interactable = true;

        if (indoor)
        {
            FirstButton.gameObject.SetActive(false);
            BackToOverview.gameObject.SetActive(true);
        }
        else
        {
            FirstButton.gameObject.SetActive(true);
            BackToOverview.gameObject.SetActive(false);
        }
            
	}

    public void toFirst()
    {
        MainCam.transform.position = FirstCam.transform.position;
        MainCam.transform.rotation = FirstCam.transform.rotation;
        indoor = true;
    }

    public void toOverview()
    {
        MainCam.transform.position = MAINMAIN.transform.position;
        MainCam.transform.rotation = MAINMAIN.transform.rotation;
        indoor = false;
        currentCamera = 2;
    }

    public void toLeft()
    {
        //left         middle    right
        //1 - chicken, 2 - beef, 3 - fish
        if (currentCamera == 2)
        {
            currentCamera = 1;
            MainCam.transform.position = ChickenCam.transform.position;
            MainCam.transform.rotation = ChickenCam.transform.rotation;
        }

        else if(currentCamera == 3)
        {
            currentCamera = 2;
            MainCam.transform.position = BeefCam.transform.position;
            MainCam.transform.rotation = BeefCam.transform.rotation;
        }
            
    }

    public void toRight()
    {
        if(currentCamera == 1)
        {
            currentCamera = 2;
            MainCam.transform.position = BeefCam.transform.position;
            MainCam.transform.rotation = BeefCam.transform.rotation;
        }

        else if(currentCamera == 2)
        {
            currentCamera = 3;
            MainCam.transform.position = FishCam.transform.position;
            MainCam.transform.rotation = FishCam.transform.rotation;
        }
    }
}
