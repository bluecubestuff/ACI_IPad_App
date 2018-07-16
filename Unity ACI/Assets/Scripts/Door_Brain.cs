using UnityEngine;
using System.Collections;

public class Door_Brain : MonoBehaviour {
    private Animator AnimRef;

	// Use this for initialization
	void Start () {
        //Get a reference to Animator attached to Object for use
        AnimRef = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
	}
    void OnTriggerEnter(Collider coll)
    {
        Open();
    }

    //Door Opens
    void Open()
    {
        //Play Animation
        AnimRef.SetTrigger("DoorOpen");
        //Play Sound
        //Do whatever
    }
}
