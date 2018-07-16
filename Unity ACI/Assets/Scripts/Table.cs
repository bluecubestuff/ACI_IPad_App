using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Table : MonoBehaviour {

    public List<Transform> ChairList = new List<Transform>();

    public int EmptySlots = 0;
    public bool TableOccupy;

    bool hasFood;

    public Transform GroupRef;
    public float SetPopPoints = 0.1f;
    float PopPoints = 0f;
    int RNGChance = 0;
    public StocknPopularityManager StockManager;
    public GameObject FOOD;


    // Use this for initialization
    void Start()
    {
        hasFood = false;
        TableOccupy = false;
        SetPopPoints = 0.1f;
        Transform ChairParent = transform.Find("Chairs");

        if (ChairParent == null)
        {
            //Debug.Log(transform.name + " has no Chairs");
        }
           
        //find child(Chair)
        foreach (Transform ChairChild in ChairParent)
        {
            if (ChairChild.GetComponent<Chair>())
            {
                ChairList.Add(ChairChild);
            }
        }
    }
	// Update is called once per frame
	void Update () {
        //if (WaiterAI_States.Instance.served)
        //{
        //    Debug.Log("IS EATING!");
        //    StartCoroutine(StartEating(2)); // Wait for some time for eating, then leave
        //    WaiterAI_States.Instance.served = false; 
        //}

        if (hasFood)
        {
            hasFood = false;
           // Debug.Log("Starts eating food.");
            StartCoroutine(StartEating(2));
        }
    }

    //True if no seats left
    public bool SeatsIsFull()
    {
        foreach (Transform chair in ChairList)
        {
            if (chair.GetComponent<Chair>().IsOccupied() == false)
            {
                return false;
            }
        }
        return true;
    }

    //The number of Empty chairs there
    public int GetEmptySlots()
    {
        EmptySlots = 0;
        foreach (Transform chair in ChairList)
        {
            if (chair.GetComponent<Chair>().IsOccupied() == false)
            {
                EmptySlots++;
            }
        }
        return EmptySlots;
    }

    //Get ONE transform of an EmptySeat
    public Transform GetEmptySeat()
    {
        foreach (Transform chair in ChairList)
        {
            if (chair.GetComponent<Chair>().IsOccupied() == false)
            {
                return chair;
            }
        }
        return null;
    }

    public void OccupyTable(Transform Group)
    {
        TableOccupy = true;
        GroupRef = Group;
        //Start the PRoccess
    }

    public void UnoccupyTable()
    {
        TableOccupy = false;

        foreach (Transform chair in ChairList)
        {
            chair.GetComponent<Chair>().UnOccupy();
        }
    }

    //Process what needs to be done(4)
    public void ProcessState()
    {
       // Debug.Log("test");

        //Take Order
        //if (TableState == 0)
        //{
        //    WaiterAI_States.Instance.TakeOrder(transform);
        //    WaiterAI_States.Instance.GoCounter();
        //    TableState++;
        //}
        //else if (TableState == 1)//Food Served
        //{
        //    Debug.Log("test");

        //    //We can serve food
        //    if (WaiterAI_States.Instance.ServingCount > 0)
        //    {
        //        //active food here
        //        StartCoroutine(StartEating(2f));
        //        WaiterAI_States.Instance.ServeFood(transform);
        //        FOOD.gameObject.SetActive(true);

        //    }
        //}
    }

    public void PlaceFood()
    {
        hasFood = true;
        FOOD.gameObject.SetActive(true);
    }

    public IEnumerator StartEating(float a)
    {
        yield return new WaitForSeconds(a);
        ChanceToGetPopPoints();
        FOOD.gameObject.SetActive(false);
        GroupRef.GetComponent<AI_Group>().CustomerLeft();
    }

    public void ChanceToGetPopPoints()
    {
        RNGChance = /*Random.Range(1, 3)*/2;
        if (StocknPopularityManager.starRating <= 2)
        {
            if (RNGChance == 1)
            {
                AddPopPoints();
            }
            RNGChance = 0;
        }
        else if (StocknPopularityManager.starRating >= 3)
        {
            AddPopPoints();
        }
    }

    public void AddPopPoints()
    {

        //Debug.Log(StockManager.popularityBar.fillAmount + "HEEE");
        //PopPoints = SetPopPoints * (StockManager.popularityBar.fillAmount / 1f);
       // Debug.Log("SetPopPoints = " + SetPopPoints);
       // Debug.Log("PopPoints = " + PopPoints);
        StockManager.AddPopularityPoints(SetPopPoints);
    }

}
