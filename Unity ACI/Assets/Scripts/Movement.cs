using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {


    [SerializeField]private float moveSpeed = 10.0f;
    [SerializeField]private float turnSpeed = 50.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if(Input.GetKey(KeyCode.W))
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);

        if (Input.GetKey(KeyCode.S))
            transform.Translate(-Vector3.forward * moveSpeed * Time.deltaTime);

        if (Input.GetKey(KeyCode.A))
            transform.Rotate(Vector3.up * -turnSpeed * Time.deltaTime);

        if (Input.GetKey(KeyCode.D))
            transform.Rotate(Vector3.up * turnSpeed * Time.deltaTime);

    }
}
