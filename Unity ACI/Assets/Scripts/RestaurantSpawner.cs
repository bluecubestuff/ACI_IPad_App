using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RestaurantSpawner : MonoBehaviour {
    public static RestaurantSpawner Instance;
    public int AI_Count = 0;
    public int MaximumSpawnCount = 5;
    public ObjectPool CustomerList;
    public ObjectPool AI_Parent;
    public GameObject ai_end;
    public float SpawnTime;
    private float TimeToSpawn;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }

    // Use this for initialization
    void Start () {
    }

    void Update()
    {
        if (NewTutorials.tutDone)
        {
            //Time has passed, Time to spawn
            if (TimeToSpawn > SpawnTime)
            {
                //Spawn here
                if (AI_Count < MaximumSpawnCount)
                    SpawnObj();

                //Reset Spawn Timer
                TimeToSpawn = 0f;
            }
            else
                TimeToSpawn += Time.deltaTime;
        }
    }
    public void SpawnObj()
    {
        //Set the Size of the Group
        int GroupSize = Random.Range(1, 5);

        //Find a Table with sufficient slots for that Size
        Transform GroupTable = TableManager.Instance.FindTablesWithSlot(GroupSize);

        //True if no table with slots
        if (GroupTable == null)
            return;

        //---Found a Table----

        //Parent Prefab from ObjectPool(Group)
        GameObject PooledParent = AI_Parent.GetObject();
        PooledParent.transform.SetParent(transform);

        //Allow Grouping for the Individuals
        AI_Group PooledGroup = PooledParent.GetComponent<AI_Group>();
        PooledGroup.GroupSize = GroupSize;
        PooledGroup.GroupTable = GroupTable.GetComponent<Table>();

        //Occupy the Table with the created Group
        GroupTable.GetComponent<Table>().OccupyTable(PooledParent.transform);

        //Set the group's Position to the SpawnPoint
        PooledParent.transform.position = transform.position;

        float offset = 0;

        //Spawn each Individual Customer to add to the Group
        for (int i = 0; i < PooledGroup.GroupSize; ++i)
        {
            //Child Prefab from ObjectPool(Individual Customer)
            GameObject PooledChild = CustomerList.GetObject();

            //Put the Individual inside the Group
            PooledChild.transform.SetParent(PooledParent.transform);
            PooledChild.transform.position = new Vector3(PooledParent.transform.position.x + offset, PooledParent.transform.position.y + offset/2, PooledParent.transform.position.z);

            //AI of the Individual
            AI_State PooledAI = PooledChild.GetComponent<AI_State>();

            offset += 20;

            //Make AI start walking to Table
            StartCoroutine(PooledAI.WalkToTable(GroupTable.transform));
        }

        //Start checking for AI to reach the Table
        StartCoroutine(PooledGroup.WalkingToTable());
        AI_Count++;
    }

    public void ReturnPooledObjects()
    {
        CustomerList.ReturnObject(gameObject);
    }
}
