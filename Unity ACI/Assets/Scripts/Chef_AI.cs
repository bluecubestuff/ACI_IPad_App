using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Chef_AI : MonoBehaviour {
    public static Chef_AI Instance;

    public Transform
        getOrders,
        giveOrders,
        getStocks,
        cookFood,
        giveFood;
    public GameObject OrderUI;
    public Chef_Meat linkedHeadChef;

    NavMeshAgent navMesh;
    Animator animator;

    int
        placedOrders,
        ordersInHand,
        stocksInQueue,
        foodToServe;

    float routineDelay = 0.0f;

    TransformAlignment alignment;


    enum ActionState
    {
        Idle,
        GetOrders,
        GiveOrders,
        GetStocks,
        CookFood,
        GiveFood,
    }

    ActionState actionState;

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
    void Start () {
        placedOrders = 0;
        ordersInHand = 0;
        stocksInQueue = 0;
        foodToServe = 0;

        actionState = ActionState.Idle;

        navMesh = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        alignment = GetComponent<TransformAlignment>();
    }
	
	// Update is called once per frame
	void Update () {
        //ReceiveOrder();

        ChefAction();
    }

    void ChefAction()
    {
        bool p = PathComplete();

        animator.SetBool("IsWalking", !p);

        if (!p)
            return;
        else
        {
            switch (actionState)
            {
                case ActionState.Idle:
                    //ChefAnimation.SetBool("CookWalk", false);
                    alignment.forcedRotationEnabled = false;
                    break;

                case ActionState.GetOrders:
                    //ChefAnimation.SetBool("CookWalk", false);
                    alignment.forcedRotationEnabled = true;
                    alignment.forcedRotationQuat = getOrders.localRotation;
                    break;

                case ActionState.GiveOrders:
                    //ChefAnimation.SetBool("CookWalk", false);
                    alignment.forcedRotationEnabled = true;
                    alignment.forcedRotationQuat = giveOrders.localRotation;
                    break;

                case ActionState.GetStocks:
                    //ChefAnimation.SetBool("CookWalk", false);
                    alignment.forcedRotationEnabled = true;
                    alignment.forcedRotationQuat = getStocks.localRotation;
                    break;

                case ActionState.CookFood:
                    //ChefAnimation.SetBool("CookWalk", false);
                    alignment.forcedRotationEnabled = true;
                    alignment.forcedRotationQuat = cookFood.localRotation;
                    break;

                case ActionState.GiveFood:
                    //ChefAnimation.SetBool("CookWalk", false);
                    alignment.forcedRotationEnabled = true;
                    alignment.forcedRotationQuat = giveFood.localRotation;
                    break;
            }
        }

        if (routineDelay > 0.0f)
        {
            routineDelay -= Time.deltaTime;
            return;
        }

        switch (actionState)
        {
            case ActionState.Idle:

                if (stocksInQueue > 0)
                {
                    Debug.Log("Getting stocks...");

                    GetStocks();
                }
                else if (placedOrders > 0)
                {
                    Debug.Log("Getting orders...");

                    navMesh.SetDestination(getOrders.position);
                    //ChefAnimation.SetBool("CookWalk", true);
                    GetOrders();
                }

                break;

            case ActionState.GetOrders:
                ++ordersInHand;
                --placedOrders;

                if (placedOrders > 0)
                {
                    GetOrders();
                }
                else
                {
                    Debug.Log("Giving orders...");

                    navMesh.SetDestination(giveOrders.position);
                    //ChefAnimation.SetBool("CookWalk", true);
                    GiveOrders();
                }

                break;

            case ActionState.GiveOrders:
                Chef_Meat.Instance.AddOrder();
                --ordersInHand;

                if (ordersInHand > 0)
                {
                    GiveOrders();
                }
                else
                {
                    Debug.Log("Giving orders is complete.");
                    actionState = ActionState.Idle;
                }

                break;

            case ActionState.GetStocks:
                --stocksInQueue;
                Debug.Log("Cooking food...");

                CookFood();

                break;

            case ActionState.CookFood:
                Debug.Log("Giving food...");

                GiveFood();

                break;

            case ActionState.GiveFood:
                Chef_Meat.Instance.AddFood();
                Debug.Log("Giving food is complete.");

                actionState = ActionState.Idle;
                break;
        }
    }

    void GetOrders()
    {
        routineDelay = 0.6f;

        actionState = ActionState.GetOrders;
    }
    void GiveOrders()
    {
        routineDelay = 0.6f;

        actionState = ActionState.GiveOrders;
    }
    void GetStocks()
    {
        navMesh.SetDestination(getStocks.position);
        //ChefAnimation.SetBool("CookWalk", true);
        routineDelay = 1.5f;

        actionState = ActionState.GetStocks;
    }
    void CookFood()
    {
        navMesh.SetDestination(cookFood.position);
        //ChefAnimation.SetBool("CookWalk", true);
        routineDelay = 4.0f;

        actionState = ActionState.CookFood;
    }
    void GiveFood()
    {
        navMesh.SetDestination(giveFood.position);
        //ChefAnimation.SetBool("CookWalk", true);
        routineDelay = 1.25f;

        actionState = ActionState.GiveFood;
    }

    //Obsolete methods below.
    public IEnumerator OrderPopups(float a)
    {
        yield return new WaitForSeconds(a);
        OrderUI.SetActive(false);
    }
    //Obsolete methods above.

    public void ReceiveOrder()
    {
        ++placedOrders;
    }
    public void ReceiveStocks()
    {
        ++stocksInQueue;
    }
    public void AddServedFood()
    {
        ++foodToServe;
    }
    public void TakeServedFood()
    {
        --foodToServe;
    }

    public bool ServingIsAvailable()
    {
        return foodToServe > 0;
    }

    public bool PathComplete()
    {
        if (!navMesh.pathPending)
        {
            if (navMesh.remainingDistance <= navMesh.stoppingDistance + 0.1f)
                return true;
        }

        return false;
    }
}

