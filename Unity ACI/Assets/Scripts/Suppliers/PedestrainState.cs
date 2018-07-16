using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedestrainState : MonoBehaviour {

    bool orientationRight = false;
    float speed;
    public bool idle;
    float defaultRotation;
    public float idleTimer;
    bool stopped;
    Animator pedesAnimator;
	// Use this for initialization
    //States for pedestrain in shops scene
	void Start () {
        stopped = false;
        defaultRotation = 90;

        pedesAnimator = transform.GetComponent<Animator>();
        if (transform.position.x > 0)
        {
            orientationRight = true;
            transform.localRotation = Quaternion.Euler(0,-90,0);
            defaultRotation = -90;
        }
        speed = Random.Range(1, 5);
        if (speed >= 3)
            pedesAnimator.speed = 1;
        else
            pedesAnimator.speed = 0.5f;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (idle)
        {
            pedesAnimator.SetBool("Leave", false);
            pedesAnimator.SetBool("Sit", true);

            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * 10);
            idleTimer -= Time.deltaTime;

            if (idleTimer <= 0)
            {
                idle = false;
            }
        }
        else
        {
            pedesAnimator.SetBool("Leave", true);
            pedesAnimator.SetBool("Sit", false);
            if (orientationRight)
                transform.position -= Vector3.right * Time.deltaTime * speed;
            else
                transform.position += Vector3.right * Time.deltaTime * speed;
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(0, defaultRotation, 0), Time.deltaTime * 10);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (stopped && other.transform.tag == "Shops")
            stopped = false;
    }
    void OnTriggerEnter(Collider collision)
    {
        if (collision.transform.name == "Bound")
            Destroy(gameObject);

        if (collision.transform.tag == "Shops" && !stopped)
        {
            float random = Random.Range(0, 30);
            if (random <= 10)
            {
                idle = true;
                idleTimer = Random.Range(1f, 5f);
            }
            stopped = true;
        }
    }
}
