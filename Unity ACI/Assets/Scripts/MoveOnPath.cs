using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MoveOnPath : MonoBehaviour {
    public static MoveOnPath Instance;

    public EditPathScript PathToFollow;

    public int CurrentWayPointID = 0;
    public float speed = 2;
    public bool DestroyOnEnd;

    [SerializeField]private float rotationSpeed = 5.0f;
    //[SerializeField]private string pathName;

    [SerializeField]private float reachDistance = 0.1f;

    public bool waypointDone = false;
   
    //Vector3 curr_position;

    //Vector3 last_position;
    //public GameObject[] allPaths;


    void Awake()
    {
        if (!Instance)
            Instance = null;
    }

    void Start () {

        
        //int num = Random.Range(0, allPaths.Length);
        //transform.position = allPaths[num].transform.position;
        //PathToFollow = GameObject.Find(pathName).GetComponent<EditPathScript>();
        //last_position = transform.position;

    }

    void Update()
    {
        if (PathToFollow != null)
        {
            if (PathToFollow.path_objs[CurrentWayPointID] != null)
            {

                float distance = Vector3.Distance(PathToFollow.path_objs[CurrentWayPointID].position,
                                                    transform.position);
                transform.position = Vector3.MoveTowards(transform.position,
                                                    PathToFollow.path_objs[CurrentWayPointID].position,
                                                    Time.deltaTime * speed);

                var rotation = Quaternion.LookRotation(PathToFollow.path_objs[CurrentWayPointID].position - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation,
                                                    rotation,
                                                    Time.deltaTime * rotationSpeed);

                if (distance <= reachDistance)
                    CurrentWayPointID++;
                if (CurrentWayPointID >= PathToFollow.path_objs.Count)
                {
                    waypointDone = true;
                    CurrentWayPointID = 0;

                    if (DestroyOnEnd)
                    {
                        Destroy(gameObject);
                        SpawnScript.maxEnemy--;
                    }

                }
            }
        }
    }
}
