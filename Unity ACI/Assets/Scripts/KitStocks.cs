using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class KitStocks : MonoBehaviour {
    public static KitStocks Instance;

    public List<Transform> K_Stocks = new List<Transform>();

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }

	// Use this for initialization
	void Start () {

        foreach (Transform child in transform)
        {
            //It is a Stocks
            if (child.GetComponent<Stocks>())
            {
                //Add to the List for use later
                K_Stocks.Add(child);

                //Hide all the Stocks
                child.gameObject.SetActive(false);
            }
        }
        SpawnStocksBasedOnStock();

    }
	
	// Update is called once per frame
	void Update () {
        //
        if (Input.GetKeyDown(KeyCode.DownArrow))
            ReduceStock();
	}

    public void SpawnStocksBasedOnStock()
    {
        int ToSpawn = Mathf.FloorToInt(K_Stocks.Count * (StocknPopularityManager.stockValue / 1f));
        //SpawnStocks(1);
        SpawnStocks(ToSpawn);
    }


    void SpawnStocks(int AmountToSpawn)
    {

        foreach (Transform child in K_Stocks)
        {
            //We have spawned enough
            if (AmountToSpawn <= 0)
                return;
            AmountToSpawn--;

            //True if already spawned
            if (child.gameObject.activeSelf)
                continue;

            //Spawn
            child.gameObject.SetActive(true);
        }
    }

    public void ReduceStock()
    {
        foreach (Transform child in transform)
        {
            //True if already spawned
            if (child.gameObject.activeSelf)
            {
                child.gameObject.SetActive(false);
                StocknPopularityManager.stockValue -= 0.1f;
                break;
            }
        }
    }
}
