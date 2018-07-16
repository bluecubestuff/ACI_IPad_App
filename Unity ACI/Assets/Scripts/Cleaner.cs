using UnityEngine;
using System.Collections;

public class Cleaner : MonoBehaviour {
    public static Cleaner Instance;
    public UnityEngine.AI.NavMeshAgent CleanerAgent;
    public Animator CleanAnimation;
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        }
    public void WalkTo(Transform t)
    {
        CleanAnimation.SetInteger("CleanerState", 1);
        CleanerAgent.SetDestination(t.position);
        if (PathComplete())
        {
            Mop();
        }
        
    }
    public bool Mop()
    {
        CleanAnimation.SetInteger("CleanerState",2);
        StartCoroutine(AnimationEnds());
        return true;
    }
    public bool PathComplete()
    {
        if (!CleanerAgent.pathPending)
        {
            if (CleanerAgent.remainingDistance <= CleanerAgent.stoppingDistance)
            {
                if (!CleanerAgent.hasPath || CleanerAgent.velocity.sqrMagnitude == 0f)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public IEnumerator AnimationEnds()
    {
        yield return new WaitForSeconds(CleanAnimation.GetCurrentAnimatorStateInfo(0).length);
        CleanAnimation.SetInteger("CleanerState", 0);
}
}
