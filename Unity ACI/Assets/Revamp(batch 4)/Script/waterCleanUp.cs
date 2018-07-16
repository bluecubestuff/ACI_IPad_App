using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles both spawning and cleaning up of water spills
/// </summary>
public class waterCleanUp : MonoBehaviour {

    //Prefab of the water spill
    [SerializeField] private GameObject waterSpil;

    //GameObject of waiter
    [SerializeField] private GameObject waiter;

    //Layer for raycast
    [SerializeField] private LayerMask layerMask;

    public float timerSpeed = 1.0f;
    [Tooltip("Interval between each possible spawn")]
    public float spawnInterval = 5.0f;
    [Tooltip("Spawn chance of spill at each interval (0.0 - 1.0f). Closer to 1.0f = higher chance")]
    public float spawnRate = 0.5f;
    private float timer = 0.0f;
    private Ray ray;
    private RaycastHit hitInfo;

    // Update is called once per frame
    void Update()
    {
        //Creation of spill
        timer += Time.deltaTime * timerSpeed;
        if (timer > spawnInterval)
        {
            float spawnChance = Random.Range(0.0f, 1.0f);
            if (spawnChance > spawnRate)
            {
                Instantiate(waterSpil, waiter.transform.position, Quaternion.identity);
            }
            timer = 0.0f;
        }

        //Cleaning up of spill with raycast
        if (Input.GetMouseButtonDown(0))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray,out hitInfo,500.0f,layerMask))
            {
                //Input name into the string
                if(hitInfo.transform.name == "WaterSpill(Clone)")
                {
                    Destroy(hitInfo.transform.gameObject);
                }
            }
        }
    }
}
