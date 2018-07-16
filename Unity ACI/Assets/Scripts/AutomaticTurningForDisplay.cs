using UnityEngine;
using System.Collections;

public class AutomaticTurningForDisplay : MonoBehaviour {

    [SerializeField]private float turnspeed = 10f;
	
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(Vector3.up, turnspeed * Time.deltaTime);
	}
}
