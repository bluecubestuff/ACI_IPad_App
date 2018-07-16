using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class FoodDatabase : MonoBehaviour
{
    public List<Food> food = new List<Food>();
    public Food[] foodTypes;
    string jsonString;
    void Start()
    {
        //Getting food data items from json file
        string jsonFile = (Application.streamingAssetsPath + "/fooddata.json");
        if (Application.platform == RuntimePlatform.Android)
        {
            WWW reader = new WWW(jsonFile);
            while (!reader.isDone) { }

            jsonString = reader.text;
        }
        else
        {
            jsonString = File.ReadAllText(jsonFile);
        }
        foodTypes = JsonHelper.FromJson<Food>(jsonString);

        for (int i = 0; i < foodTypes.Length; i++)
        {
            if (foodTypes[i] != null)
            {
                Food newFood = foodTypes[i];
                food.Add(newFood);
                food[i].init();
            }
        }
    }
}
