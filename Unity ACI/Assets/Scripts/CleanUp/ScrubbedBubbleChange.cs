using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrubbedBubbleChange : MonoBehaviour {

    [SerializeField]
    Material[] greenTick;

    public bool accomplished = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (gameObject.transform.localScale.x >= 8)
        {
            GetComponent<Renderer>().materials = greenTick;
            accomplished = true;
        }
	}
}
