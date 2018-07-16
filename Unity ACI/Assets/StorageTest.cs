using UnityEngine;
using System.Collections;

public class StorageTest : MonoBehaviour {

    public GameObject firstImg;
    public GameObject secondImg;

    public GameObject FButton;
    public GameObject DButton;
    public GameObject WButton;

    public GameObject toStorage;
    public GameObject toFront;

	// Use this for initialization
	void Start () {
        toStorage.SetActive(true);
        toFront.SetActive(false);

        firstImg.SetActive(true);
        secondImg.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void enterRoom()
    {
        FButton.SetActive(false);
        DButton.SetActive(false);
        WButton.SetActive(false);

        firstImg.SetActive(false);
        secondImg.SetActive(true);

        toStorage.SetActive(false);
        toFront.SetActive(true);
    }

    public void exitRoom()
    {
        FButton.SetActive(true);
        DButton.SetActive(true);
        WButton.SetActive(true);

        firstImg.SetActive(true);
        secondImg.SetActive(false);

        toStorage.SetActive(true);
        toFront.SetActive(false);
    }
}
