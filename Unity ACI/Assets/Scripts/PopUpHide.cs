using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpHide : MonoBehaviour {
    float popUpTime;
	// Use this for initialization
	void Start () {
        popUpTime = 3f;
	}
	
	// Update is called once per frame
	void Update () {
		if(gameObject.activeSelf && popUpTime >= 0)
        {
            popUpTime -= Time.deltaTime;
        }
        if (Input.GetMouseButtonDown(0))
        {
            popUpTime = 0.05f;
        }

        if (popUpTime <= 0)
        {
            gameObject.SetActive(false);
            popUpTime = 3f;
        }
	}
}
