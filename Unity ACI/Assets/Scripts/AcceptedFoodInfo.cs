using UnityEngine;

public class AcceptedFoodInfo : MonoBehaviour
{
    public Food food;

    // Update is called once per frame
    void Update()
    {
        if (gameObject.GetComponent<AcceptedFoodInfo>().food.foodName != null && gameObject.GetComponent<AcceptedFoodInfo>().food != null)
        {

            gameObject.GetComponent<MeshFilter>().mesh = gameObject.GetComponent<AcceptedFoodInfo>().food.foodPrefab.GetComponent<MeshFilter>().sharedMesh;
            gameObject.GetComponent<MeshRenderer>().materials = gameObject.GetComponent<AcceptedFoodInfo>().food.foodPrefab.GetComponent<MeshRenderer>().sharedMaterials;
        }
    }
}
