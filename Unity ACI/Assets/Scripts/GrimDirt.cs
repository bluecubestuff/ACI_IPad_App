using UnityEngine;
using System.Collections;

public class GrimDirt : MonoBehaviour {

    float Time_G;

	// Use this for initialization
	void Start () {
        //StartCoroutine(DecreasePop(3));
        Time_G = -1;
    }
	
	// Update is called once per frame
	void Update () {
        DecreasePop(3);
    }
    //public IEnumerator DecreasePop(float a)
    //{
    //    if (Time_G < 0)
    //        Time_G = a;
    //    yield return new WaitForSeconds(a);
    //    if(StocknPopularityManager.popValue > 0)
    //        StocknPopularityManager.popValue -= 0.05f;
    //}
    public bool DecreasePop(float a)
    {
        if (Time_G < 0)
            Time_G = a;
        if (Time_G > 0)
        {
            Time_G -= Time.deltaTime;
            if (Time_G < 0)
            {
                if (StocknPopularityManager.popValue > 0)
                {
                    Debug.Log("PopPoint Minus");
                    StocknPopularityManager.popValue -= 0.01f;
                    return true;
                }
                Time_G = -1;
            }
        }
        return false;
    }
}
