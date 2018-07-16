using UnityEngine;
using System.Collections;

public class AndyTouchManager : MonoBehaviour
{

    //public GameObject particleStuff;

    public bool isDestroyFeature;

    public GameObject Grime;
    public GameObject Clean;
    //public WaiterAI_States Waiter;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {




        if (Input.GetMouseButton(0))
        {
            Vector3 mousePosFar = new Vector3(Input.mousePosition.x,
                                               Input.mousePosition.y,
                                               Camera.main.farClipPlane);
            Vector3 mousePosNear = new Vector3(Input.mousePosition.x,
                                               Input.mousePosition.y,
                                               Camera.main.nearClipPlane);
            Vector3 mousePosF = Camera.main.ScreenToWorldPoint(mousePosFar);
            Vector3 mousePosN = Camera.main.ScreenToWorldPoint(mousePosNear);

            Debug.DrawRay(mousePosN, mousePosF - mousePosN, Color.green);

            //Debug.Log("clicked");

            RaycastHit hit;
            if ((Physics.Raycast(mousePosN, mousePosF - mousePosN, out hit)))
            {
                if (hit.collider.gameObject.tag == "CleanUp")
                {
                    Clean.SetActive(true);
                    Clean.GetComponent<CleanUp>().LastTouch = hit.collider.gameObject;
                    Clean.GetComponent<CleanUp>().WalkToGrime();
                }

                #region DinerDash_Game Mechanics
                /*if (hit.collider.gameObject.tag == "Table")
                {
                    StartCoroutine(WaiterAI_States.Instance.StartWalking(hit.transform));
                    WaiterAI_States.Instance.LatestTouch = hit.collider.transform;
                }
                if (hit.collider.gameObject.tag == "Counter")
                {
                    StartCoroutine(WaiterAI_States.Instance.StartWalking(hit.transform));
                    WaiterAI_States.Instance.LatestTouch = hit.collider.transform;
                }*/
                #endregion
            }
        }
    }
}
