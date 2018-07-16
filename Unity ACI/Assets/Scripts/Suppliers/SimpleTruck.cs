using UnityEngine;

public class SimpleTruck : MonoBehaviour {

    [SerializeField]
    GameObject[] resetPosition;
    Animator truckAnimator;
    [SerializeField]
    float maxSpeed;
    [SerializeField]
    float minSpeed;
    float speed;
	// Use this for initialization
	void Start () {
        speed = Random.Range(minSpeed, maxSpeed);
        truckAnimator = transform.GetComponent<Animator>();
        truckAnimator.speed = speed/maxSpeed;
    }
	
	// Update is called once per frame
	void Update () {
        transform.position += Vector3.right * Time.deltaTime * speed;

    }
    void OnTriggerEnter(Collider collision)
    {
        if (collision.transform.name == "Bound")
        {
            int x = Random.Range(0, resetPosition.Length);
            transform.position = new Vector3(resetPosition[x].transform.position.x, transform.position.y, resetPosition[x].transform.position.z);
            speed = Random.Range(minSpeed, maxSpeed);
            truckAnimator.speed = speed / maxSpeed;
        }


    }
}
