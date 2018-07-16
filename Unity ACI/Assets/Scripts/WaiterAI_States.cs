using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaiterAI_States : MonoBehaviour
{
    public static WaiterAI_States Instance;

    Chef_AI CHEF;

    public List<Transform> OrderPriorityList = new List<Transform>();
    public List<Transform> ServePriorityList = new List<Transform>();
    public UnityEngine.AI.NavMeshAgent WaiterNavMesh;
    public GameObject WaiterIdle;
    //Indexes
    private int CurrentOrder = 0;
    private int CurrentServe = 0;
    public int Orders = 0;
    public int ServingCount = 0; 
    public Transform LatestTouch;
    public float timer = 20;
    public bool served;
    bool a_takeorder, a_serve;
    public Animator WaiterAnim;

    public List<Transform> ToProcess = new List<Transform>();

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance == this)
            Destroy(gameObject);
    }

    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        WaiterProcess();
    }

    public void TakeOrder(Transform ToTake)
    {
        ServePriorityList.Add(ToTake);
        Orders++;
    }

    public void ServeFood(Transform ToServe)
    {
        ServePriorityList.Remove(ToServe);
        ServingCount--;
        a_serve = false;
        WaiterAnim.SetBool("Serve", false);
    }

    public void GoCounter()
    {
        Transform theCounter = GameObject.Find("FoodCounter").transform;
        AddToProcess_Immediate(theCounter);
    }

    public bool ReadyToServe()
    {
        return (ServePriorityList.Count > 0 && ServingCount > 0);
    }

    public bool PathComplete()
    {
        if (!WaiterNavMesh.pathPending)
        {
            if (WaiterNavMesh.remainingDistance <= WaiterNavMesh.stoppingDistance + 0.4f)
            {
                if (!WaiterNavMesh.hasPath || WaiterNavMesh.velocity.sqrMagnitude == 0f)
                {
                    return true;
                }
            }
        }
        return false;
    }

    //Processes the List one by one (2)
    public void WaiterProcess()
    {
        //There is someone to process
        if (ToProcess.Count > 0)
        {
            //True if not going anywhere(No current process)
            if (PathComplete())
            {
                
                //True if we are ready to serve
                if (ReadyToServe())
                    AddToProcess_Immediate(ServePriorityList[0]);  
                StartCoroutine(StartWalking(ToProcess[0]));//Do the first Process on the List

            }
        }
    }

    //Adds to the List of Processes (1.5)
    public void AddToProcess(Transform pr)
    {
       // Debug.Log("Added to process.");
        ToProcess.Add(pr);
    }

    //Adds to the first Element (So that this Process will be done first)
    public void AddToProcess_Immediate(Transform pr)
    {
        ToProcess.Insert(0, pr);
    }

    //Go to the Process to begin Processing (3)
    public IEnumerator StartWalking(Transform Destination)
    {
        if (StocknPopularityManager.stockValue > 0)
        {
          
            WaiterAnim.SetBool("TakeOrder", true);

            if (a_serve == true)
            {
                WaiterAnim.SetBool("Serve", true);
            }
                


            WaiterNavMesh.SetDestination(Destination.position);

            //True if Path not Completed Yet
            while (!PathComplete())
                yield return null;
            //--Path Is Complete--

            //Process job that Waiter walked to
            if (Destination.GetComponent<Table>())//Reached Table
            {
                Table TableRef = Destination.GetComponent<Table>();
                TableRef.ProcessState();
            }
            else if (Destination.name == "FoodCounter")//Reached Counter
            {
                //ServingCount++;
                //Chef_AI.Instance.placedOrders += Orders;
                //Orders = 0;
                //ServingCount = Chef_AI.Instance.CookedFood;
                //a_serve = true;
            }

            //Remove this Completed Process
            ToProcess.Remove(Destination);
        }
    }
}
