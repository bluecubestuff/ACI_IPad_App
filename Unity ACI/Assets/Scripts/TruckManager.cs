using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TruckManager : MonoBehaviour
{

    public static List<GameObject> truckList = new List<GameObject>();
    public static List<Food> foodList = new List<Food>();
    public GameObject truckPrefab;
    float x = -2.5f;
    float y = 0;

    public FoodDatabase database;

    // Use this for initialization
    void Start()
    {

        int truckAmount = 0;

        for (int i = 1; i <= 5; i++)
        {
            //spawn truck prefab and give then a ID
            GameObject truck = Instantiate(truckPrefab);
            truck.GetComponent<Truck>().index = truckAmount;
            truck.AddComponent<CheckForStorageDoor>();
            //add them to list
            truckList.Add(truck);
            foodList.Add(new Food());

            //set the prefab as child of the truckmanager
            truck.transform.SetParent(this.gameObject.transform);
            truck.name = "Truck " + i;
            truck.GetComponent<Transform>().localPosition = new Vector3(x, y, 1.5f);

            x += 1;
            truckAmount++;
        }
        //AddTruck(Random.Range(0, 3));
        //AddTruck(Random.Range(0, 3));
        //AddTruck(Random.Range(0, 3));
    }

    void Update()
    {
        /*if (Input.GetKeyUp("space"))
        {
            Debug.Log("gg");
            AddTruck(Random.Range(0, 3));
        }*/
    }

    public void AddTruck(int id)
    {
        //check from database
        for (int i = 0; i < database.food.Count; i++)
        {
            if (database.food[i].foodID == id)
            {
                //add truck from database
                Food food = database.food[i];
                AddFoodToTruck(food);
                break;
            }
        }
    }

    public void AddTruck(string name)
    {
        //check from database
        for (int i = 0; i < database.food.Count; i++)
        {
            if (database.food[i].foodName == name)
            {
                //add truck from database
                Food food = database.food[i];
                AddFoodToTruck(food);
                break;
            }
        }
    }

    public void AddFoodToTruck(Food food)
    {
        //check the list for empty slot
        for (int i = 0; i < foodList.Count; i++)
        {
            if (foodList[i].foodName == null)
            {
                //add them to list
                foodList[i] = food;
                break;
            }
        }
    }

    public void ChangeTruckFood(Food food, int truckIndex)
    {
        transform.GetChild(truckIndex).GetComponent<Truck>().food = food;
        foodList[truckIndex] = food;
        AddFoodToTruck(food);
        for (int i = 0; i < foodList.Count; i++)
        {
            //Debug.Log(foodList[i].foodName);
        }

    }
}
