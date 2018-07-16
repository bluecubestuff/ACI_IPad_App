using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WaiterScript2 : MonoBehaviour {

    public static WaiterScript2 Instance;

    enum ActionState
    {
        Idle,
        PlaceOrders,
        GiveOrders,
        TakeFood,
        Serve,
    }
    ActionState actionState;
    float routineDelay;
    
    NavMeshAgent navMesh;
    Animator animator;
    TransformAlignment alignment;

    public Chef_AI linkedChef;


    Queue<Table>
        preorders,
        capturedPreorders,
        orderQueue,
        servingQueue;

    public Transform
        giveOrders,
        takeFood;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance == this)
        {
            Destroy(gameObject);
        }
    }

	// Use this for initialization
	void Start () {
        preorders = new Queue<Table>();
        capturedPreorders = new Queue<Table>();
        orderQueue = new Queue<Table>();
        servingQueue = new Queue<Table>();

        alignment = GetComponent<TransformAlignment>();
        alignment.forcedRotationEnabled = false;

        navMesh = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        actionState = ActionState.Idle;
        routineDelay = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
        WaiterAction();
	}

    void WaiterAction()
    {
        if (!PathComplete())
        {
            animator.SetBool("Walking", true);
            return;
        }
        else
        {
            animator.SetBool("Walking", false);

            switch (actionState)
            {
                case ActionState.Idle:
                    alignment.forcedRotationEnabled = false;

                    break;

                case ActionState.GiveOrders:
                    alignment.forcedRotationEnabled = true;
                    alignment.forcedRotationQuat = giveOrders.transform.localRotation;

                    break;

                case ActionState.TakeFood:
                    alignment.forcedRotationEnabled = true;
                    alignment.forcedRotationQuat = takeFood.transform.localRotation;
                    break;

                case ActionState.PlaceOrders:
                case ActionState.Serve:
                    alignment.forcedRotationEnabled = false;

                    break;
            }
        }

        if (routineDelay > 0.0f)
        {
            routineDelay -= Time.deltaTime;

            animator.SetBool("Ordering", actionState == ActionState.PlaceOrders);
            animator.SetBool("GivingOrders", actionState == ActionState.GiveOrders);
            animator.SetBool("TakeFood", actionState == ActionState.TakeFood);
            animator.SetBool("Serving", actionState == ActionState.Serve);

            return;
        }

        switch (actionState)
        {
            case ActionState.Idle:
                if (servingQueue.Count > 0 && Chef_AI.Instance.ServingIsAvailable())
                {
                    //Debug.Log("Taking food...");

                    TakeFood();
                }
                else if (preorders.Count > 0)
                {
                   // Debug.Log("Preordering...");
                    while (preorders.Count > 0)
                    {
                        capturedPreorders.Enqueue(preorders.Peek());
                        preorders.Dequeue();
                    }

                    Preorder(capturedPreorders.Peek());
                }
                break;

            case ActionState.PlaceOrders:
                orderQueue.Enqueue(capturedPreorders.Peek());
                //Debug.Log("Order placed.");
                capturedPreorders.Dequeue();

                if (capturedPreorders.Count == 0)
                {
                    //Debug.Log("Giving orders...");

                    GiveOrders();
                }
                else
                {
                    //Debug.Log("Preordering for next group...");

                    Preorder(capturedPreorders.Peek());
                }

                animator.SetBool("Ordering", false);
                animator.SetBool("Walking", true);

                break;

            case ActionState.GiveOrders:
                linkedChef.ReceiveOrder();
                servingQueue.Enqueue(orderQueue.Peek());
                orderQueue.Dequeue();

                if (orderQueue.Count > 0)
                {
                    routineDelay = 1.0f;
                }
                else
                {
                    //Debug.Log("Giving orders is done.");
                    actionState = ActionState.Idle;
                }

                if (linkedChef.ServingIsAvailable())
                {
                   // Debug.Log("Taking food...");

                    TakeFood();
                }

                break;

            case ActionState.TakeFood:
                linkedChef.TakeServedFood();
                animator.SetBool("HasFood", true);

                //Debug.Log("Bringing food to customer...");

                ServeCustomer(servingQueue.Peek());

                break;

            case ActionState.Serve:
                servingQueue.Peek().PlaceFood();
                servingQueue.Dequeue();

                //Debug.Log("Food is served.");

                animator.SetBool("HasFood", false);

                actionState = ActionState.Idle;

                break;
        }
    }

    void Preorder(Table table)
    {
        animator.SetBool("Walking", true);
        navMesh.SetDestination(table.transform.position);
        routineDelay = 1.5f;

        actionState = ActionState.PlaceOrders;
    }
    void GiveOrders()
    {
        animator.SetBool("Walking", true);
        navMesh.SetDestination(giveOrders.position);
        routineDelay = 1.0f;

        actionState = ActionState.GiveOrders;
    }
    void TakeFood()
    {
        animator.SetBool("Walking", true);
        navMesh.SetDestination(takeFood.position);
        routineDelay = 0.8f;

        actionState = ActionState.TakeFood;
    }
    void ServeCustomer(Table table)
    {
        animator.SetBool("Walking", true);
        navMesh.SetDestination(table.transform.position);
        routineDelay = 1.2f;

        actionState = ActionState.Serve;
    }

    public void AddPreorder(Table obj)
    {
        preorders.Enqueue(obj);
    }

    bool PathComplete()
    {
        return !navMesh.pathPending && navMesh.velocity == Vector3.zero;
    }
}
