using UnityEngine;
using System.Collections;

public class SpawnScript : MonoBehaviour {

    public GameObject[] spawnObjects;
    public Vector3 spawnValues;
    public EditPathScript GlobalPath;

    public float spawnDelay;
    public float spawnStartDelay;
    public float spawnMaxWait;
    public float spawnMinWait;

    public GameObject spawnParent;

    public bool loopSwitch;

    public static int maxEnemy;
    int randObjType;
    
	// Use this for initialization
	void Start () {

        loopSwitch = true;
        maxEnemy = 0;
        //StartCoroutine(waitSpawner());
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyUp(KeyCode.X))
        {
            //PopUpBarNotifAnim.ispopup = true;
            waitSpawner();
            //loopSwitch = false;
            Debug.Log("KeyHit");
        }
        //spawnDelay = Random.Range(spawnMinWait, spawnMaxWait);
        
	}

    public void waitSpawner()
    {
        //yield return new WaitForSeconds(spawnStartDelay);


       // while (!loopSwitch)
       // {
            //if (Input.GetKeyUp(KeyCode.X))
            //{

            //if (maxEnemy <= 10)
            //{
                //Types of enemy to spawn
                randObjType = Random.Range(0, spawnObjects.Length);

                spawnObjects[randObjType].GetComponent<MoveOnPath>().PathToFollow = GlobalPath;
                Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), 0,
                                                    Random.Range(-spawnValues.z, spawnValues.z));

                GameObject tempSpawn = (GameObject)Instantiate(spawnObjects[randObjType],
                                                               spawnPosition + transform.TransformPoint(0, 0, 0),
                                                               gameObject.transform.rotation);

                spawnObjects[randObjType].GetComponent<MoveOnPath>().speed = Random.Range(5, 5);
                tempSpawn.transform.parent = spawnParent.transform;

                //maxEnemy++;
                //loopSwitch = true;
            //}
            //yield return new WaitForSeconds(spawnDelay);
        //}
       // }
    }
}
