/* 
 * Author: Lim Rui An Ryan
 * Filename: ARCleanObjectSpawner.cs
 * Description: A script that is used spawn objects used in the cleaning game mode.
 */
using System.Collections.Generic;
using UnityEngine;

public class ARCleanObjectSpawner : MonoBehaviour {
    /// Public Variables
    Vector3 RayStart = new Vector3(0, 10, 0);
    Vector3 RayTarget = Vector3.zero;
    Vector3 DirtSize = new Vector3(0.75f, 0.75f, 0.75f);
    Vector3 WaterSize = new Vector3(0.5f, 0.5f, 0.5f);
    /// Private Variables
    // Linked Instantiated Prefabs
    [SerializeField] GameObject StainPrefab;
    // Sprites for this prefab
    [SerializeField] List<Material> DirtVariants;
    [SerializeField] List<Material> WaterVariants;

    // Use this for initialization
    void Start () {
        // Assuming that I'm a standalone object that exists only to spawn dirt objects

	}

    public int GenerateDirt(List<GameObject> LinkedObjectList, Vector3 SpawningDimensions, int SpawnCount)
    {
        // Minor error handling
        if (StainPrefab == null || DirtVariants.Count == 0){
            Debug.Log("ARCleanObjectSpawner: Missing Stain Prefab / No Materials Attached");
            return 0;
        }
        int SpawnCounter = SpawnCount;
        int LoopBreaker = 0;
        while (SpawnCounter > 0){
            // Ascertain randomized target
            Vector3 RangedTarget = new Vector3(RayTarget.x + Random.Range(-SpawningDimensions.x, SpawningDimensions.x), RayTarget.y, RayTarget.z + Random.Range(-SpawningDimensions.z, SpawningDimensions.z));
            // Create the ray
            Ray SpawnRay = new Ray(RayStart, RangedTarget - RayStart);
            RaycastHit RCHit;
            Physics.Raycast(SpawnRay, out RCHit);
            if (RCHit.collider != null){
                if (RCHit.collider.gameObject.tag != "ARC_Stain"){
                    LoopBreaker = 0;
                    // Can spawn object, decrement counter
                    SpawnCounter--;
                    // Instantiate the prefab
                    GameObject NewStain = Instantiate(StainPrefab);
                    NewStain.GetComponent<ARCleanDirt>().StartingAlpha = 1;
                    // Deactivate Collider
                    foreach (Collider c in NewStain.GetComponents<Collider>())
                        c.enabled = false;
                    // Set the tag
                    NewStain.tag = "ARC_Stain";
                    // Set up the transform for the prefab
                    NewStain.transform.position = RCHit.point;
                    // Set up the scale for the prefab
                    NewStain.transform.localScale = DirtSize;
                    // Set up a random rotation angle
                    NewStain.transform.eulerAngles = new Vector3(90, 0, Random.Range(0, 360));
                    // Set up it's parameters
                    NewStain.GetComponent<Renderer>().material = DirtVariants[Random.Range(0, DirtVariants.Count)];
                    // Add it to the list
                    LinkedObjectList.Add(NewStain);
                }
                else{
                    LoopBreaker++;
                    if (LoopBreaker > 50)
                        break;
                }
            }
        }
        return SpawnCount - SpawnCounter;
    }

    public int GenerateWater(List<GameObject> LinkedObjectList, Vector3 SpawningDimensions, int SpawnCount, string Tag)
    {
        // Minor error handling
        if (StainPrefab == null || DirtVariants.Count == 0)
        {
            Debug.Log("ARCleanObjectSpawner: Missing Stain Prefab / No Materials Attached");
            return 0;
        }
        int SpawnCounter = SpawnCount;
        int LoopBreaker = 0;
        while (SpawnCounter > 0)
        {
            // Ascertain randomized target
            Vector3 RangedTarget = new Vector3(RayTarget.x + Random.Range(-SpawningDimensions.x, SpawningDimensions.x), RayTarget.y, RayTarget.z + Random.Range(-SpawningDimensions.z, SpawningDimensions.z));
            // Create the ray
            Ray SpawnRay = new Ray(RayStart, RangedTarget - RayStart);
            RaycastHit RCHit;
            Physics.Raycast(SpawnRay, out RCHit);
            if (RCHit.collider != null)
            {
                bool SpawnCheck = true;
                // If a tag is specified, check against the collider to determine if an object can be spawned
                if (!Tag.Equals("") && RCHit.collider.gameObject.tag != Tag)
                    SpawnCheck = false;
                if (SpawnCheck && RCHit.collider.gameObject.tag != "ARC_Water")
                {
                    LoopBreaker = 0;
                    // Can spawn object, decrement counter
                    SpawnCounter--;
                    SpawnWater(LinkedObjectList, RCHit.point);
                }
                else
                {
                    LoopBreaker++;
                    if (LoopBreaker > 50)
                        break;
                }
            }
        }
        return SpawnCount - SpawnCounter;
    }

    public void SpawnWater(List<GameObject> LinkedObjectList, Vector3 SpawningPosition)
    {
        Ray SpawnRay = new Ray(RayStart, SpawningPosition - RayStart);
        RaycastHit RCHit;
        Physics.Raycast(SpawnRay, out RCHit);
        if (RCHit.collider == null || RCHit.collider.gameObject.tag == "ARC_Water")
            return;

        // Instantiate the prefab
        GameObject NewWater = Instantiate(StainPrefab);
        NewWater.GetComponent<ARCleanDirt>().StartingAlpha = 0.8f;
        // Deactivate Collider
        foreach (Collider c in NewWater.GetComponents<Collider>())
            c.enabled = false;
        // Set the tag
        NewWater.tag = "ARC_Water";
        // Set up the transform for the prefab
        NewWater.transform.position = SpawningPosition;
        // Set up the scale for the prefab
        NewWater.transform.localScale = WaterSize;
        // Set up a random rotation angle
        NewWater.transform.eulerAngles = new Vector3(90, 0, Random.Range(0, 360));
        // Set up it's parameters
        NewWater.GetComponent<Renderer>().material = WaterVariants[Random.Range(0, WaterVariants.Count)];
        // Add it to the list
        LinkedObjectList.Add(NewWater);
    }

    public void ActivateInternalColliders(List<GameObject> LinkedObjectList)
    {
        foreach(GameObject G in LinkedObjectList)
            foreach (Collider C in G.GetComponents<Collider>())
                C.enabled = true;
    } 
}
