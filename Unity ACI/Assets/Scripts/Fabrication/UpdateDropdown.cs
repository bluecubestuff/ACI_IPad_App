using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UpdateDropdown : MonoBehaviour
{
    //This script is to make sure the drop down is not reset to position 0
    private static float SCROLL_MARGIN = 0.3f; // how much to "overshoot" when scrolling, relative to the selected item's height

    private ScrollRect sr;
    [SerializeField]
    public RectTransform srTransform;
    MeatFabManager mfm;
    public void Awake()
    {
        sr = this.gameObject.GetComponent<ScrollRect>();
    }
    private void Start()
    {
        mfm = GameObject.FindGameObjectWithTag("MFM").GetComponent<MeatFabManager>();
        Debug.Log(mfm.locY);
        srTransform.transform.localPosition = new Vector3(srTransform.transform.localPosition.x,mfm.locY, srTransform.transform.localPosition.z);
        Debug.Log(srTransform.transform.localPosition);
    }
}