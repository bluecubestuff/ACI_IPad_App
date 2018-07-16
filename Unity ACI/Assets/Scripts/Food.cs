using UnityEngine;
using System.Collections;

[System.Serializable]
public class Food {

    public int foodID;
    public string foodName;
    public string prefabLocation;
    public FoodType foodType;
    public double foodRarity;
    public GameObject foodPrefab;
    public Sprite foodIconType;
    public Material foodARImage;
    public FoodPlacement foodPlacement;

    public enum FoodType
    {
        DRY,
        COLD,
        FREEZE,
    }

    public enum FoodPlacement
    {
        TOP,
        BOTTOM,
    }  

    public Food(int id, string name, FoodType type, double rarity, string prefab, FoodPlacement placement)
    {
        foodID = id;
        foodName = name;
        foodType = type;
        foodRarity = rarity;
        foodPrefab = Resources.Load<GameObject>("Model Prefab/" + prefab);
        foodIconType = Resources.Load<Sprite>("Storage Icon/" + foodType.ToString());
        foodARImage = Resources.Load<Material>("Food/" + prefab);
        foodPlacement = placement;
    }
    public Food()
    {

    }
    public void init()
    {
        //foodPrefab = Resources.Load<GameObject>("ModelPrefab/" + prefabLocation);
        foodARImage = Resources.Load<Material>("Food/" + prefabLocation);
        foodIconType = Resources.Load<Sprite>("Storage Icon/" + foodType.ToString());
    }
}
