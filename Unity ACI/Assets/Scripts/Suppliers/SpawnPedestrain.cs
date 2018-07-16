using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPedestrain : MonoBehaviour {

    public int spawnRate;
    public float timer;

    [SerializeField]
    string[] pedestrains;
    [SerializeField]
    GameObject[] spawners;

    BoxCollider collider;
    //Spawning pedestrains for shop scene (Just to spice it up)
	// Use this for initialization
	void Start () {
        timer = 5f;
        collider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update () {
        if (timer > 0)
            timer -= Time.deltaTime;


        else
        {
            spawnRate = Random.Range(0, 100);
            if (spawnRate <= 50)
            {
                Instantiate(Resources.Load(pedestrains[Random.Range(0,pedestrains.Length)]) as GameObject, new Vector3(spawners[Random.Range(0, spawners.Length)].transform.localPosition.x, spawners[Random.Range(0, spawners.Length)].transform.localPosition.y, spawners[Random.Range(0, spawners.Length)].transform.localPosition.z + (Random.Range(-2f, 2f))), transform.rotation);
            }
                timer = 5;
        }
    }
 
}
