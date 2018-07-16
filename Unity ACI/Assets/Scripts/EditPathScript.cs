using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EditPathScript : MonoBehaviour {


    public Color rayColor = Color.white;                        //color of path of line
    public List<Transform> path_objs = new List<Transform>();
    Transform[] theArray;


    //Draw Gizmos in editor
	void OnDrawGizmos()
    {
        Gizmos.color = rayColor;

        theArray = GetComponentsInChildren<Transform>();    //Store list of arrays
        path_objs.Clear();      // Clear the path whenever we run in editor

        //Creates new Transform Objs to use from theArray
        foreach(Transform path_singleObj in theArray)
        {
            if(path_singleObj != transform)
            {
                //Add new paths after CLearing the list 
                path_objs.Add(path_singleObj);
            }
        }

        for(int i = 0; i< path_objs.Count; i++)
        {
            Vector3 position = path_objs[i].position;
            if(i>0)//if list is not empty
            {
                Vector3 previousPath = path_objs[i - 1].position;
                Gizmos.DrawLine(previousPath, position);    //show line where path is 
                Gizmos.DrawWireSphere(position, 0.3f);
            }
        }


    }
}
