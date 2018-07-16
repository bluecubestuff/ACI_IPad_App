using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameCache : Singleton<GameCache>
{
    public List<GameObject> OrderList;

    // Use this for initialization
    void Start () {
        if (OrderList == null)
            OrderList = new List<GameObject>();

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
