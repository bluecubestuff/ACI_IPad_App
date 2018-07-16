using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    /// Private Variable
    private float CurrentPositionY;

    /// Public Variable
    public bool isOpen;    
    public float factor;
    public GameObject MenuCover; //Arrow for sliding open the menu 

    private void Start()
    {
        CurrentPositionY = transform.GetComponent<RectTransform>().anchoredPosition.y;
    }
    
    public void ToggleMenu()
    {
        StopAllCoroutines();
        if (!isOpen)
        {
            StartCoroutine(OpenMenu());
            ARCleanDataStore.HideRotationalArrows = true;
            isOpen = true;
        }
        else
        {
            StartCoroutine(CloseMenu());
            ARCleanDataStore.HideRotationalArrows = false;
            isOpen = false;
        }
    }

    private IEnumerator OpenMenu()
    {
        RectTransform rectTrans = transform.GetComponent<RectTransform>();
        while (factor < 1f)
        {
            factor += 0.05f;
            rectTrans.anchoredPosition = new Vector2(rectTrans.anchoredPosition.x, Mathf.Lerp(rectTrans.anchoredPosition.y, CurrentPositionY - rectTrans.sizeDelta.x, factor));
            MenuCover.transform.localRotation = Quaternion.Euler(0, 0, 0);
            yield return null;
        }
    }

    private IEnumerator CloseMenu()
    {
        RectTransform rectTrans = transform.GetComponent<RectTransform>();
        while (factor > 0f)
        {
            factor -= 0.05f;
            rectTrans.anchoredPosition = new Vector2(rectTrans.anchoredPosition.x, Mathf.Lerp(rectTrans.anchoredPosition.y, CurrentPositionY, 1f - factor));
            MenuCover.transform.localRotation = Quaternion.Euler(0, 0, 180);
            yield return null;
        }
    }
}
