using UnityEngine;
using System.Collections;

public class AI_State : MonoBehaviour {

    public Animator Walk;


    public UnityEngine.AI.NavMeshAgent AINavMesh;
    public GameObject Ai_Path_Ends;
    public StocknPopularityManager StockManager;
    public GameObject WIN;
    public GameObject blackBackground;
    public float SetPopPoints = 0.1f;
    float PopPoints = 0f;
    int RNGChance = 0;
    public bool HasBeenServed;
    public bool Seated = false;

    // Use this for initialization
    void Start()
    {
        Seated = false;
        SetPopPoints = 0.05f;

        AINavMesh.speed = Random.Range(0.4f, 1.5f);
    }
	
	// Update is called once per frame
	void Update () {

        if (StocknPopularityManager.starRating >= 1)
        {
            WIN.SetActive(true);
            blackBackground.SetActive(true);
            Time.timeScale = 0; // Pauses the game
        }

        //if (Input.GetKey(KeyCode.D))
        //    Time.timeScale = 5f;
        //else
        //    Time.timeScale = 1f;
        //return;
    }
    public void ChanceToGetPopPoints()
    {
        RNGChance = Random.Range(1, 3);
        if (StocknPopularityManager.starRating <= 0.4f)
        {
            if (RNGChance == 1)
            {
                //AddPopPoints();
                StockManager.AddPopularityPoints(1f);
                //StockManager.AddPopularityPoints(0.5f);
            }
            else
                StockManager.AddPopularityPoints(0.5f);
            //StockManager.AddPopularityPoints(0.3f);
        }
        else if (StocknPopularityManager.starRating > 0.4f)
        {
            //AddPopPoints();
            StockManager.AddPopularityPoints(0.35f);
        }
    }

    public void AddPopPoints()
    {
        //Debug.Log("ADDED!!!!");
        //Debug.Log("SetPopPoints(BEFORE) = " + SetPopPoints);
        //Debug.Log(StockManager.popularityBar.fillAmount + "HEEE");
        //PopPoints = SetPopPoints * (StockManager.popularityBar.fillAmount / 1f);
        //Debug.Log("SetPopPoints = " + SetPopPoints);
        //Debug.Log("PopPoints = " + PopPoints);
        //StockManager.AddPopularityPoints(PopPoints);
    }

    public IEnumerator WalkToTable(Transform TargetTable)
    {

        //Walk.SetBool("CustomerWalk", true);

        //Find a Chair at the Table
        Table Target = TargetTable.GetComponent<Table>();

        //Find a Seat at the Table
        Transform AvailableChair = Target.GetEmptySeat();

        //Reserve the Chair
        Chair ChairRef = AvailableChair.GetComponent<Chair>();
        ChairRef.Occupy();

        //Go to our Reserved Chair
        AINavMesh.SetDestination(AvailableChair.position);
        
        //True if we reached Destination
        while (!PathComplete())
        {
            yield return null;
        }

        //--Customer Reached Destination--
        Seated = true;
        AINavMesh.enabled = false;
        Walk.SetBool("Sit", true);
        transform.eulerAngles = -AvailableChair.eulerAngles + AvailableChair.parent.parent.eulerAngles;
        if (AvailableChair.parent.parent.eulerAngles.y <= 10f || AvailableChair.parent.parent.eulerAngles.y >= 350f)
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y - 90f, 0);

        //SittingPosition OFFSET
        //Corner Table

        if (AvailableChair.tag == "LeftChairGroup") // Chairs belonging to the tables on the left side of the screen
        {
            if (AvailableChair.position.z < -1.4f && AvailableChair.position.x < -3.9f)
            {
                transform.position = new Vector3(AvailableChair.position.x -0.1f, transform.position.y, AvailableChair.position.z);
                if (AvailableChair.rotation.y > 0)
                {
                    transform.position = new Vector3(AvailableChair.position.x + 0.04f, transform.position.y, AvailableChair.position.z);
                }
            }
        }
        else if (AvailableChair.tag == "RightChairGroup") // Chairs belonging to the tables on the left side of the screen
        {
            //transform.position = new Vector3(AvailableChair.position.x, transform.position.y, AvailableChair.position.z - 0.1f);
            transform.position = new Vector3(AvailableChair.position.x, transform.position.y, AvailableChair.position.z + 0.05f);

            if (AvailableChair.localPosition.x < -3.9f)
            {
                transform.position = new Vector3(AvailableChair.position.x, transform.position.y, AvailableChair.position.z - 0.1f);
            }
            
        }
        ////Bottom Right
        //else if (AvailableChair.position.z < 1f && AvailableChair.position.x > 1f && AvailableChair.position.x < 1.3f)
        //    transform.position = new Vector3(AvailableChair.position.x - 0.15f, transform.position.y, AvailableChair.position.z);
        //else if (AvailableChair.position.z < 1f && AvailableChair.position.x > 0.3f && AvailableChair.position.x < 1f)
        //    transform.position = new Vector3(AvailableChair.position.x + 0.15f, transform.position.y, AvailableChair.position.z);
        ////Bottom Left
        //else if (AvailableChair.position.z < 1f && AvailableChair.position.x > -0.8f && AvailableChair.position.x < 0f)
        //    transform.position = new Vector3(AvailableChair.position.x - 0.15f, transform.position.y, AvailableChair.position.z);
        //else if (AvailableChair.position.z < 1f && AvailableChair.position.x < -0.8f)
        //    transform.position = new Vector3(AvailableChair.position.x + 0.15f, transform.position.y, AvailableChair.position.z);
        ////Top Tables
        //else if (AvailableChair.position.z > 2f && AvailableChair.position.z < 2.3f)
        //    transform.position = new Vector3(AvailableChair.position.x, transform.position.y, AvailableChair.position.z + 0.15f);
        //else if (AvailableChair.position.z > 2.3f)
        //    transform.position = new Vector3(AvailableChair.position.x, transform.position.y, AvailableChair.position.z - 0.15f);
    }

    public bool PathComplete()
    {
        //Debug.Log("PATH COMPLETE");
        //Walk.SetBool("CustomerWalk", false);
        if (!AINavMesh.pathPending)
        {
            if (AINavMesh.remainingDistance <= AINavMesh.stoppingDistance)
            {
                if (!AINavMesh.hasPath || AINavMesh.velocity.sqrMagnitude == 0f)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
