using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Chef_Meat : MonoBehaviour
{
    public static Chef_Meat Instance;

    NavMeshAgent navMesh;
    Animator animator;

    //public KitStocks KitStocks;
    public Transform
        getOrder,
        getStocks,
        giveStocks,
        getFood,
        prepareFood,
        serveFood;

    int
        ordersInQueue,
        foodInQueue;

    bool hasNoStock;

    TransformAlignment alignment;

    enum ActionState
    {
        Idle,
        GetOrder,
        GetStocks,
        GiveStocks,
        GetFood,
        PrepareFood,
        ServeFood,
    };

    ActionState actionState;
    float routineDelay = 0.0f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Use this for initialization
    void Start()
    {
        actionState = ActionState.Idle;

        ordersInQueue = 0;
        foodInQueue = 0;
        hasNoStock = false;

        navMesh = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        alignment = GetComponent<TransformAlignment>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (CheckGetStocks)
        //{
        //    if (PathComplete())
        //    { 
        //        Debug.Log("Going to prepare food");
        //        KitStocks.ReduceStock();
        //        PrepareFood();
        //        CheckGetStocks = false;
        //    }
        //}

        ChefAction();
    }



    void ChefAction()
    {
        bool p = PathComplete();

        animator.SetBool("IsWalking", !p);

        if (!p)
        {
            switch (actionState)
            {
                case ActionState.Idle:
                    //Reset walk animation.
                    alignment.forcedRotationEnabled = false;

                    break;

                case ActionState.GetOrder:
                    //Reset walk animation.
                    alignment.forcedRotationEnabled = true;
                    alignment.forcedRotationQuat = getOrder.localRotation;

                    break;

                case ActionState.GetStocks:
                    //Reset walk animation.
                    alignment.forcedRotationEnabled = true;
                    alignment.forcedRotationQuat = getStocks.localRotation;

                    break;

                case ActionState.GiveStocks:
                    //Reset walk animation.
                    alignment.forcedRotationEnabled = true;
                    alignment.forcedRotationQuat = giveStocks.localRotation;

                    break;

                case ActionState.GetFood:
                    //Reset walk animation.
                    alignment.forcedRotationEnabled = true;
                    alignment.forcedRotationQuat = getFood.localRotation;

                    break;

                case ActionState.PrepareFood:
                    //Reset walk animation.
                    alignment.forcedRotationEnabled = true;
                    alignment.forcedRotationQuat = prepareFood.localRotation;

                    break;

                case ActionState.ServeFood:
                    //Reset walk animation.
                    alignment.forcedRotationEnabled = true;
                    alignment.forcedRotationQuat = serveFood.localRotation;

                    break;
            }

            return;
        }
        if (routineDelay > 0)
        {
            routineDelay -= Time.deltaTime;
            return;
        }

        switch (actionState)
        {
            case ActionState.Idle:
                if (hasNoStock)
                {
                    if (StocknPopularityManager.stockValue > 0.0f)
                        hasNoStock = false;
                }

                if (foodInQueue > 0)
                {
                    Debug.Log("Getting food...");
                    GetFood();
                }
                else if (ordersInQueue > 0 && !hasNoStock)
                {
                    Debug.Log("Getting an order...");
                    GetOrder();
                }

                break;

            case ActionState.GetOrder:
                Debug.Log("Getting stocks...");
                GetStocks();

                break;

            case ActionState.GetStocks:
                if (StockIsAvailable())
                {
                    UseStocks();

                    Debug.Log("Giving stocks...");
                    GiveStocks();
                }
                else
                {
                    Debug.Log("No stock available.");
                    hasNoStock = true;
                    actionState = ActionState.Idle;
                }

                break;

            case ActionState.GiveStocks:
                Chef_AI.Instance.ReceiveStocks();
                Debug.Log("Giving stocks is done.");

                actionState = ActionState.Idle;
                break;

            case ActionState.GetFood:
                --foodInQueue;

                Debug.Log("Preparing food...");
                PrepareFood();

                break;

            case ActionState.PrepareFood:
                Debug.Log("Serving food...");
                ServeFood();

                break;

            case ActionState.ServeFood:
                Chef_AI.Instance.AddServedFood();
                Debug.Log("Serving food is complete.");

                actionState = ActionState.Idle;

                break;
        }
    }

    void GetOrder()
    {
        navMesh.SetDestination(getOrder.position);
        routineDelay = 0.6f;

        actionState = ActionState.GetOrder;
    }
    void GetStocks()
    {
        navMesh.SetDestination(getStocks.position);
        routineDelay = 2.0f;

        actionState = ActionState.GetStocks;
    }
    void GiveStocks()
    {
        navMesh.SetDestination(giveStocks.position);
        routineDelay = 1.5f;

        actionState = ActionState.GiveStocks;
    }
    void GetFood()
    {
        navMesh.SetDestination(getFood.position);
        routineDelay = 1.25f;

        actionState = ActionState.GetFood;
    }
    void PrepareFood()
    {
        navMesh.SetDestination(prepareFood.position);
        routineDelay = 3.5f;

        actionState = ActionState.PrepareFood;
    }
    void ServeFood()
    {
        navMesh.SetDestination(serveFood.position);
        routineDelay = 0.8f;

        actionState = ActionState.ServeFood;
    }

    bool StockIsAvailable()
    {
        return (StocknPopularityManager.stockValue > 0.0f);
    }
    void UseStocks()
    {
        StocknPopularityManager.stockValue -= 0.1f;
    }

    public void AddOrder()
    {
        ++ordersInQueue;
    }
    public void AddFood()
    {
        ++foodInQueue;
    }

    public bool PathComplete()
    {
        //Debug.Log(MeatNavMesh.remainingDistance);

        
        if (!navMesh.pathPending)
        {
          
            if (Vector3.Distance(navMesh.destination, navMesh.transform.position) <= navMesh.stoppingDistance + 0.4f)
            {
                if (!navMesh.hasPath || navMesh.velocity.sqrMagnitude == 0f)
                {
                    //Debug.Log("PathComplete");
                    navMesh.updateRotation = false;
                    return true;
                }
            }
        }
        return false;
    }


}
