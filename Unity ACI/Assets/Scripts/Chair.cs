using UnityEngine;
using System.Collections;

public class Chair : MonoBehaviour {
    private bool Occupied = false;
    public AI_State Customer;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Occupy()
    {
        Occupied = true;
    }

    public void UnOccupy()
    {
        Occupied = false;
    }

    public bool IsOccupied()
    {
        return Occupied;
    }
}
