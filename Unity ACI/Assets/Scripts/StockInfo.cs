using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class StockInfo : MonoBehaviour
{

    /* StockInfo.cs holds data for each food. 
     * All food models or gameobjects that wants to hold food data MUST have StockInfo.cs!
         This Script automatically changes its own model to food model based on containing data*/


    [SerializeField]
    bool isActive;

    public Food food;
    public int index;
    

    [HideInInspector] public Vector3 InitialPosition;

    private Vector3 originalScale;

    // Use this for initialization
    void Start()
    {
        isActive = false;
        originalScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.GetComponent<MeshRenderer>().material = food.foodARImage;
        if(food.foodName.Contains("Carrot") || food.foodName.Contains("Chilli") || food.foodName.Contains("Tomato"))
        {
            transform.localScale = new Vector3(0.1f, transform.localScale.y, 0.1f);
        }
        else
        {
            transform.localScale = originalScale;
        }
    }
    
    //To call from other scripts
    public void SetActiveSelected(bool active)
    {
        isActive = active;
    }

    public IEnumerator ReturnToPosition(System.Action isDone)
    {
        while (transform.position != InitialPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, InitialPosition, 4f * Time.deltaTime);
            yield return null;
        }
        isDone();
    }
}
