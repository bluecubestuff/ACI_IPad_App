using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GrimDirtManager : MonoBehaviour {
    public static GrimDirtManager Instance;
    private List<Transform> GrimList = new List<Transform>();

    //Spawning Variables
    public float MinimumSpawnTime;
    public float MaximumSpawnTime;
    public float SpawnTime; 

    public int MinimumSpawnAmount;
    public int MaximumSpawnAmount;
    public int GetAiRefSeated;

    public int SpawnCount = 0;

    //Checking Current Grime
    public GrimDirt CurrentGrime = null;

    void Awake()
    {
        if (!Instance)
            Instance = null;
    }

    // Use this for initialization
    void Start () {

        //Iterate through all your Children
        foreach (Transform child in transform)
        {
            //It is a GrimDirt
            if (child.GetComponent<GrimDirt>())
            {
                ////Hide all the GrimDirts
                //child.gameObject.SetActive(false);

                //Add to the List for use later
                GrimList.Add(child);
            }
        }

        //Set the MaximumSpawnAmount to the amount of Childs
        //MaximumSpawnAmount = GrimList.Count;

        //Set the Timer to start counting down
        SetTimer();
	}
	
	// Update is called once per frame
	void Update () {
        //GetAiRefSeated = gameObject.GetComponent<AI_Group>().GrpSeatedCount;

        //Spawn Grim, it is ready
        if (ReadyToSpawn())
        {
            SetTimer();
            SpawnGrim(Random.Range(MinimumSpawnAmount,MaximumSpawnAmount));
        }
	}

    //Set the Random Spawn Time
    void SetTimer()
    {
        MaximumSpawnTime -= GetAiRefSeated;
        SpawnTime = Random.Range(MinimumSpawnTime, MaximumSpawnTime);
    }

    //True if SpawnTime has passed
    bool ReadyToSpawn()
    {
       //Count down Spawn Time
        SpawnTime -= Time.deltaTime;

        //True if Spawn time is over then Spawn
        if (SpawnTime <= 0)
            return true;
        else
            return false;
    }

    //Spawn AmountToSpawn Grims
    void SpawnGrim(int AmountToSpawn)
    {
        
        //Grims that are NOT spawned/active/shown yet
        List<Transform> HiddenGrims = new List<Transform>();

        //Loop through List to find the NOT Spawned/Active/Shown Grims
        foreach (Transform Grim in GrimList)
        {
            //True if Grim is hidden
            if (Grim.gameObject.activeSelf == false)
            {
                //add to GrimSelection
                HiddenGrims.Add(Grim);
            }
        }

       //Create a TempList to Random Grim
        List<Transform> RNGGrims = new List<Transform>();
        
        //Loop through all the child in HiddenGrim
        for(int i = 0; i<HiddenGrims.Count;++i)
        {
            //Randomly pick out an Index from HiddenGrims
            int IndexToRemove = Random.Range(0, HiddenGrims.Count);
            //Add to Temp List
            RNGGrims.Add(HiddenGrims[IndexToRemove]);
            //Remove from the list
            HiddenGrims.RemoveAt(IndexToRemove);
        }
        //put RandomList into HiddenGrim 
        HiddenGrims = RNGGrims;
      /********************************************************************/

        //Spawn the Amount from the HiddenGrims
        foreach (Transform child in HiddenGrims)
        {
            //Spawn
            child.gameObject.SetActive(true);
            AmountToSpawn--;
            SpawnCount++;
            //We have spawned enough
            if (AmountToSpawn <= 0)
                break;
        }
    }

    public void DestroyGrime()
    {

    }
}
