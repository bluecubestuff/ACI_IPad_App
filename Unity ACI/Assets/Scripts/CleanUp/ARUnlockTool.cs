using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
public class ARUnlockTool : MonoBehaviour
{

    [SerializeField]
    string typeToUnlock;

    // Use this for initialization
    void Start()
    {
        Debug.Log(typeToUnlock);


    }

    // Update is called once per frame
    void Update()
    {
        //When scanning, it will take the string of what to unlock and unlock the button corresponding to the tool (This is the tag of the button)
        if (GameObject.FindGameObjectsWithTag(typeToUnlock) != null)
        {
            if (GameObject.FindGameObjectWithTag(typeToUnlock).GetComponent<ToolInfo>() != null)
                GameObject.FindGameObjectWithTag(typeToUnlock).GetComponent<ToolInfo>().UnlockThisTool();
        }
    }

}
