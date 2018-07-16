using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class Truck : MonoBehaviour
{

    public Food food;
    public int index;
    TruckManager truckManager;
    [SerializeField]
    OrderListManager orderListManager;
    public GameObject foodObject;
    //EditPathScript toReject;
    public bool readyToRespawn;
    public bool foodIsStored;
    private Food temp;

    // Use this for initialization
    void Start()
    {
        truckManager = GameObject.FindGameObjectWithTag("TruckManager").GetComponent<TruckManager>();
        foodIsStored = false;

        readyToRespawn = false;
        //toReject = GameObject.FindGameObjectWithTag("Reject").GetComponent<EditPathScript>();
    }

    // Update is called once per frame
    void Update()
    {
        //if theres something in the truck
        if (TruckManager.foodList[index] != null && TruckManager.foodList[index].foodName != null)
        {
            food = TruckManager.foodList[index];
            foodObject.SetActive(true);
            if (readyToRespawn)
            {
                if(orderListManager == null)
                orderListManager = GameObject.FindGameObjectWithTag("OrderListManager").GetComponent<OrderListManager>();

                for (int i = 0; i < orderListManager.transform.childCount; i++)
                {
                    for (int j = 0; j < truckManager.transform.childCount; j++)
                    {
                        if (orderListManager.transform.GetChild(i).GetComponentInChildren<Order>().food != truckManager.transform.GetChild(j).GetComponent<Truck>().food)
                        {
                            temp = orderListManager.transform.GetChild(i).GetComponentInChildren<Order>().food;
                        }
                    }
                }
                truckManager.AddFoodToTruck(temp);
                readyToRespawn = false;
                TruckManager.foodList[index] = new Food();
                foodObject.gameObject.GetComponentInParent<Truck>().food = new Food();
                foodObject.SetActive(false);
            }
            if (foodIsStored)
            {
                TruckManager.foodList[index] = new Food();
                foodObject.gameObject.GetComponentInParent<Truck>().food = new Food();
                foodObject.SetActive(false);
            }
            
        }
        else
        {
            foodObject.SetActive(false);
        }
    }
}