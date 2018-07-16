using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TableManager : MonoBehaviour {
    public static TableManager Instance;
    public Table[] TableList;

    // Use this for initialization
    void Start () {
        if (Instance == null)
            Instance = this;
        else if (!Instance)
            Destroy(gameObject);
        TableList = FindObjectsOfType(typeof(Table)) as Table[];
	}
	
	// Update is called once per frame
	void Update () {

    }

    //Return the Table's Transform with (Slots) amount of Available Chairs
    public Transform FindTablesWithSlot(int Slots)
    {
        foreach (Table table in TableList)
        {
            if (table.GetEmptySlots() >= Slots && !table.TableOccupy)
                return table.transform;//Got table with slots
        }
        return null;//No table with slots
    }
    public void reshuffle()
    {
        // Knuth shuffle algorithm
        for (int t = 0; t < TableList.Length; t++)
        {
            Table tmp = TableList[t];
            int r = Random.Range(t, TableList.Length);
            TableList[t] = TableList[r];
            TableList[r] = tmp;
        }
    }
}
