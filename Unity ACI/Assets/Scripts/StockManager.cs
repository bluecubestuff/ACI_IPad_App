using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class StockManager : MonoBehaviour
{
    public static StockManager StockInstance;


    public StockInfo CurrentStock = null;
    public int tempSizeCheck;
    public int tempFoodRand;
    public FoodDatabase database;
    public float[] StarRatingList = new float[4];

   

    public Image Ratings;

    public GameObject VeggieSelectionParent;
    public GameObject MeatSelectionParent;
    public GameObject CheeseSelectionParent;
    public GameObject CannedSelectionParent;

    public GameObject particlegood;
    public GameObject particlebad;
    public Text FoodTitle;

    public GameObject SelectionModel;
    public GameObject FinalSelectionModel;

    void Awake()
    {
        if (StockInstance == null)
        {
            StockInstance = this;
        }
    }

    // Use this for initialization
    void Start()
    {  
        for (int i = 0; i < SelectionModel.transform.childCount; i++)
        {
            //Generate Stock/Food data for all Models During Selection Scene
            SelectionModel.transform.GetChild(i).GetComponent<StockInfo>().index = i;
            if(VeggieSelectionParent != null)
            SelectionModel.transform.GetChild(i).GetComponent<Transform>().transform.position = VeggieSelectionParent.transform.GetChild(i).transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ReturnCurrentModelSize()
    {
        if (CurrentStock != null)
        {
            CurrentStock.SetActiveSelected(false);
        }
    }

    //Set Food Quality
    public void SetStockRatings()
    {
        //Food Quality is determined by min and max quality of Selected Supplier
        //float minR = SupplierSceneManager.SupplierInstance.CurrentSupplier.minRating;
        //float maxR = SupplierSceneManager.SupplierInstance.CurrentSupplier.maxRating;

        //for (int i = 0; i < StarRatingList.Length; ++i)
        //{
        //    float temp = Mathf.Round(Random.Range(minR, maxR));
        //    StarRatingList[i] = temp;
        //}
    }

    public void SetRatingFill()
    {
        /* 1 = 100% of Image
         * 0.2 = 1/5 of Image
         * 1/5 image = 1 star
         */
        Ratings.fillAmount = (float)CurrentStock.transform.GetComponent<StockInfo>().food.foodRarity * 0.2f;
    }

    public void RandomizeFoodType(int choosenfoodorder)
    {

        //Randomize through FoodDatabase
        tempFoodRand = Random.Range(0, database.food.Count);
        int newRangeCheck;
        //The "tempFoodRand/5" is because all food within Database is in multiples of 5.
        //Thus, it is easy to check based on a single number divisible by 5.
        newRangeCheck = tempFoodRand / 5;

        //choosenfoodorder : See TouchManager.cs Find: decidefoodnumber
        switch (choosenfoodorder)
        {
            //tomato
            case 0:
                {
                    //Keep randomizing until reach Veg Food
                    while (newRangeCheck != 0 && newRangeCheck != 3 && newRangeCheck != 7)
                    {
                        tempFoodRand = Random.Range(0, database.food.Count);
                        newRangeCheck = tempFoodRand / 5;
                    }
                    //tempFoodRand = 0;
                    //newRangeCheck = tempFoodRand;
                }
                break;
            //canned food
            case 1:
                {
                    //Fixed Canned Food
                    tempFoodRand = 5;
                    newRangeCheck = tempFoodRand / 5;
                }
                break;
            //steak
            case 2:
                {
                    //Keep randomizing until reached Meat type Food
                    while (newRangeCheck != 2 && newRangeCheck != 4)
                    {
                        tempFoodRand = Random.Range(0, database.food.Count);
                        newRangeCheck = tempFoodRand / 5;
                    }
                    //tempFoodRand = 10;
                    //newRangeCheck = tempFoodRand / 5;
                }
                break;
            //seafood
            case 3:
                {
                    //Fixed Dairy Food (Cheese)
                    while (newRangeCheck != 5 && newRangeCheck != 6)
                    {
                        tempFoodRand = Random.Range(0, database.food.Count);
                        newRangeCheck = tempFoodRand / 5;
                    }
                }
                break;
            default:
                {//Inital Gameplay (UNUSED)
                    //Change TouchManager's "decidefoodnumber" = 99 to access Random
                    Debug.Log("Entered Random");
                    newRangeCheck = tempFoodRand / 5;
                }
                break;
        }
        
        //Give the food model food data
        bool hasGradeA = false;
        for (int i = 0; i < SelectionModel.transform.childCount; i++)
        {
            int randFood = -1;
            switch (newRangeCheck)
            {
                case 0: //Tomato
                    randFood = Random.Range(0, 5);
                    break;
                case 1: //Canned Food
                    randFood = Random.Range(5, 10);
                    break;
                case 2: //Steak
                    randFood = Random.Range(10, 15);
                    break;
                case 3: //Mushroom            
                    randFood = Random.Range(15, 20);
                    break;
                case 4: //Chicken
                    randFood = Random.Range(20, 25);
                    break;
                case 5: //Fish
                    randFood = Random.Range(25, 30);
                    break;
                case 6: //Crab
                    randFood = Random.Range(30, 35);
                    break;
                case 7: //Carrot
                    randFood = Random.Range(35, 40);
                    break;
            }

            if (!hasGradeA && Random.value < 0.2f)
                randFood = newRangeCheck * 5;

            if (randFood % 5 == 0)
                hasGradeA = true;

            if (i == SelectionModel.transform.childCount - 1 && !hasGradeA)
                SelectionModel.transform.GetChild(i).GetComponent<StockInfo>().food = database.food[newRangeCheck * 5];
            else
                SelectionModel.transform.GetChild(i).GetComponent<StockInfo>().food = database.food[randFood];
        }

    }
    }

