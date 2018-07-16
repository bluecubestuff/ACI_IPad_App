using UnityEngine;
using System.Collections;

public class CleanUp : MonoBehaviour {
    public static CleanUp Instance;

    public bool Sweeep;
    public GameObject LastTouch;
    public GameObject Correct, Wrong;
	// Use this for initialization
	void Start () {
        Sweeep = false;
	}
	
	// Update is called once per frame
	void Update () {
    }

    public void Sweep()
    {
        gameObject.SetActive(false);
       // if (Cleaner.Instance.Sweep())
        //{
            Wrong.SetActive(true);
        //}
        //do sth here
    }
    public void Wipe()
    {
        gameObject.SetActive(false);
        //if (Cleaner.Instance.Wipe())
        //{
            Wrong.SetActive(true);
        //}
        //do sth here
    }
    public void Mop()
    {
        gameObject.SetActive(false);
        if (Cleaner.Instance.Mop())
        {
            Correct.SetActive(true);
            LastTouch.transform.parent.gameObject.SetActive(false);
        }
        //Do Sth Here
    }
    public void WalkToGrime()
    {
        Debug.Log("Walking");
        Cleaner.Instance.WalkTo(LastTouch.transform);
    }
    public void Disable()
    {
        if(Correct.activeSelf == true)
        {
            Correct.SetActive(false);
        }
        if (Wrong.activeSelf == true)
        {
            Wrong.SetActive(false);
        }
    }
}
