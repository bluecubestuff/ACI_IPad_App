using UnityEngine;
using System.Collections;

public class BillBoard : MonoBehaviour {

	// Use this for initialization
	void Start () {
        transform.rotation = Camera.main.transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {
        //transform.rotation = Camera.main.transform.rotation;
    }
}
