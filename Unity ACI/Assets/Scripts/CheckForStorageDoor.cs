using UnityEngine;
using System.Collections;

public class CheckForStorageDoor : MonoBehaviour {

    public  int num = -1;
	void OnCollisionEnter(Collision col)
    {
        Debug.Log("collllllllll");
        switch (col.gameObject.tag)
        {
            case "dryDoor":
                num = 0;
                break;
            case "wetDoor":
                num = 1;
                break;
            case "freezeDoor":
                num = 2;
                break;
        }
    }
}
