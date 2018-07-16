using UnityEngine;
using System.Collections;

public class Tester : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.RightArrow))
            Time.timeScale += 1f;
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
            Time.timeScale = 1f;
	}
}
