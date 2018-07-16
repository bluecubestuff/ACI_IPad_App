using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class OrderListManager : MonoBehaviour
{
    public static OrderListManager orderInstance;

    [SerializeField]
    ContentSizeFitter contenetSizeFitter;


    [SerializeField]
    Transform orderParentPanel;
    [SerializeField]
    GameObject newOrderPrefab;
    public static List<GameObject> messageList = new List<GameObject>();

    public FoodDatabase database;

    string message = "test";

    public static int orderLimit;
    public TruckManager truckManager;
    public TouchManager touchManager;

    public GameObject orderlistParent;
    public GameObject blackBackground;
    
    OrderListManager()
    {
        if (orderInstance == null)
        {
            orderInstance = this;
        }
    }

    void Start()
    {
        //truckManager = GameObject.FindGameObjectWithTag("TruckManager").GetComponent<TruckManager>();
        GetOrderList();
        //DontDestroyOnLoad(gameObject);
        //orderLimit = 0;
        if (transform.childCount >0)
        {
            for(int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).transform.localScale = new Vector3(1, 1, 1);

        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            SaveOrderList();
        }

     

        if (Input.GetKeyDown(KeyCode.Y))
            GetOrderList();

        if (Input.GetKeyUp("space"))
        {

            GameObject orderClone = (GameObject)Instantiate(newOrderPrefab);
            messageList.Add(orderClone);
            orderClone.transform.SetParent(orderParentPanel);
            orderClone.transform.SetSiblingIndex(orderParentPanel.childCount + 2);
            //orderClone.transform.position = new Vector2(470, 460 - (orderParentPanel.childCount * 45));

            orderClone.GetComponentInChildren<Order>().food = database.food[Random.Range(8, 13)];
            orderClone.GetComponentInChildren<Text>().text = orderClone.GetComponentInChildren<Order>().food.foodName;

            if (Random.Range(0, 11) < 5)
            {
                Debug.Log("sent correct items");
                truckManager.AddTruck(orderClone.GetComponentInChildren<Text>().text);
            }
            else
            {
                Debug.Log("sent wrong items");
                truckManager.AddTruck(Random.Range(0, 25));
            }


        }
    }


    public void SetMethod(string message)
    {
        this.message = message;
    }


    public void ShowOrder(string order)
    {

        if (gameObject.transform.childCount <= 5)
        {

            GameObject orderClone = (GameObject)Instantiate(newOrderPrefab);
            messageList.Add(orderClone);
            orderClone.transform.SetParent(orderParentPanel);
            
            orderClone.transform.SetSiblingIndex(orderParentPanel.childCount + 2);
            orderClone.GetComponentInChildren<Order>().food = touchManager.GetComponent<TouchManager>().selectedFood.GetComponent<StockInfo>().food;
            orderClone.GetComponentInChildren<Text>().text = orderClone.GetComponentInChildren<Order>().food.foodName;/*touchManager.GetComponent<TouchManager>().selectedFood.GetComponent<StockInfo>().food.foodName;*/
            truckManager.AddTruck(orderClone.GetComponentInChildren<Text>().text);
            //if (Random.Range(0, 11) < 8)
            //{
            //    Debug.Log("sent correct items");
            //    truckManager.AddTruck(orderClone.GetComponentInChildren<Text>().text);
            //}
            //else
            //{
            //    Debug.Log("sent wrong items");
            //    truckManager.AddTruck(Random.Range(0, 25));
            //}
        }
    }

    void OnDestroy()
    {

    }

    public void SaveOrderList()
    {
        //Transfer all the Children of this Transform to GameCache.Instance.Transform

        int Whatever = transform.childCount;
        for (int i = 0; i < Whatever; ++i)
        {
            foreach (Transform child in transform)
            {
                child.SetParent(null);
                child.SetParent(GameCache.Instance.transform);
                Debug.Log("SAVED");
            }
        }
    }

    public void GetOrderList()
    {
        if (GameCache.Instance.transform.childCount <= 0)
            return;

        //Transfer all the Children of GameCache.Instance.Transform to this Transform
        else
        {
            int Whatever = GameCache.Instance.transform.childCount;
            for (int i = 0; i < Whatever; ++i)
            {
                foreach (Transform child in GameCache.Instance.transform)
                {
                    child.SetParent(null);
                    child.SetParent(transform);
                    Debug.Log("LOADED");
                }
            }
        }
    }


    public void OpenOrderlist()
    {
       orderlistParent.transform.localPosition = new Vector3(0, 0, 0);
       blackBackground.SetActive(true);
    }
    public void CloseOrderlist()
    {
        orderlistParent.transform.localPosition = new Vector3(-2000, 0, 0);
        blackBackground.SetActive(false);
    }
}
