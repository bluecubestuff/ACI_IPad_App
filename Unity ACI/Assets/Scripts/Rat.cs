using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Rat : MonoBehaviour {

    public UnityEngine.AI.NavMeshAgent RatNav;
    public GameObject go;
    // Use this for initialization
    void Start()
    {
        RatNav.SetDestination(go.transform.position);
    }
	// Update is called once per frame
	void Update () {
	}

    public bool PathComplete()
    {
        if (!RatNav.pathPending)
        {
            if (RatNav.remainingDistance <= RatNav.stoppingDistance)
            {
                if (!RatNav.hasPath || RatNav.velocity.sqrMagnitude == 0f)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
