/* 
 * Author: Lee Kwan Liang
 * Filename: ARMenuSelection.cs
 * Description: A script that is used hide or show the user interface.
 */
using System.Collections;
using UnityEngine;

public class ARMenuSelection : MonoBehaviour
{
    // Public Variables
    public float RotationalArrowSpeed = 5f;
    public TransitionSystem LinkedTransit;

    /// Private Variable
    private float CurrentPositionY;
    private bool IsOpen = false;
    // Rotational Origins
    private Vector3 Position_RotationUp, Position_RotationDown, Position_RotationLeft, Position_RotationRight;

    private void Start()
    {
        //ARCleanDataStore.Menu = this;
        CurrentPositionY = transform.GetComponent<RectTransform>().anchoredPosition.y;
        Position_RotationUp = ARCleanDataStore.ModelAccess.Button_Up.transform.position;
        Position_RotationDown = ARCleanDataStore.ModelAccess.Button_Down.transform.position;
        Position_RotationLeft = ARCleanDataStore.ModelAccess.Button_Left.transform.position;
        Position_RotationRight = ARCleanDataStore.ModelAccess.Button_Right.transform.position;
    }

    private void Update()
    {
        if (IsOpen || ARCleanDataStore.HideRotationalArrows)
            HideRotationalInterface();
        else DisplayRotationalInterface();
    }

    public void ReturnToScanMode()
    {
        ARCleanDataStore.HideRotationalArrows = false;
        LinkedTransit.DecrementScene();
    }

    public void ToggleMenu()
    {
        StopAllCoroutines();
        if (!IsOpen)
        {
            StartCoroutine(OpenMenu());
            ARCleanDataStore.HideRotationalArrows = true;
            IsOpen = true;
        }
        else
        {
            StartCoroutine(CloseMenu());
            ARCleanDataStore.HideRotationalArrows = false;
            IsOpen = false;
        }
    }

    public void ToggleMenu(bool TurnOn)
    {
        StopAllCoroutines();
        if (TurnOn)
        {
            StartCoroutine(OpenMenu());
            ARCleanDataStore.HideRotationalArrows = true;
            IsOpen = true;
        }
        else
        {
            StartCoroutine(CloseMenu());
            ARCleanDataStore.HideRotationalArrows = false;
            IsOpen = false;
        }
    }

    private IEnumerator OpenMenu()
    {
        RectTransform rectTrans = transform.GetComponent<RectTransform>();
        float factor = 0f;
        while (factor < 1f)
        {
            factor += 0.05f;
            rectTrans.anchoredPosition = new Vector2(rectTrans.anchoredPosition.x, Mathf.Lerp(rectTrans.anchoredPosition.y, CurrentPositionY - rectTrans.sizeDelta.y * 0.55f, factor));
            yield return null;
        }
    }

    private IEnumerator CloseMenu()
    {
        RectTransform rectTrans = transform.GetComponent<RectTransform>();
        float factor = 0f;
        while (factor < 1f)
        {
            factor += 0.05f;
            rectTrans.anchoredPosition = new Vector2(rectTrans.anchoredPosition.x, Mathf.Lerp(rectTrans.anchoredPosition.y, CurrentPositionY, factor));
            yield return null;
        }
    }

    private void DisplayRotationalInterface()
    {
        Vector3 Direction;
        if ((Direction = (ARCleanDataStore.ModelAccess.Button_Up.transform.position - Position_RotationUp)).sqrMagnitude > 1)
            ARCleanDataStore.ModelAccess.Button_Up.transform.position -= Direction * Time.deltaTime * RotationalArrowSpeed;
        if ((Direction = (ARCleanDataStore.ModelAccess.Button_Down.transform.position - Position_RotationDown)).sqrMagnitude > 1)
            ARCleanDataStore.ModelAccess.Button_Down.transform.position -= Direction * Time.deltaTime * RotationalArrowSpeed;
        if ((Direction = (ARCleanDataStore.ModelAccess.Button_Left.transform.position - Position_RotationLeft)).sqrMagnitude > 1)
            ARCleanDataStore.ModelAccess.Button_Left.transform.position -= Direction * Time.deltaTime * RotationalArrowSpeed;
        if ((Direction = (ARCleanDataStore.ModelAccess.Button_Right.transform.position - Position_RotationRight)).sqrMagnitude > 1)
            ARCleanDataStore.ModelAccess.Button_Right.transform.position -= Direction * Time.deltaTime * RotationalArrowSpeed;
    }

    private void HideRotationalInterface()
    {
        Vector3 Direction;
        if ((Direction = (ARCleanDataStore.ModelAccess.Button_Up.transform.position - new Vector3(Screen.width * 0.5f, Screen.height * 1.05f))).sqrMagnitude > 1)
            ARCleanDataStore.ModelAccess.Button_Up.transform.position -= Direction * Time.deltaTime * RotationalArrowSpeed;
        if ((Direction = (ARCleanDataStore.ModelAccess.Button_Down.transform.position - new Vector3(Screen.width * 0.5f, Screen.height * -0.05f))).sqrMagnitude > 1)
            ARCleanDataStore.ModelAccess.Button_Down.transform.position -= Direction * Time.deltaTime * RotationalArrowSpeed;
        if ((Direction = (ARCleanDataStore.ModelAccess.Button_Left.transform.position - new Vector3(-Screen.width * 0.05f, Screen.height * 0.5f))).sqrMagnitude > 1)
            ARCleanDataStore.ModelAccess.Button_Left.transform.position -= Direction * Time.deltaTime * RotationalArrowSpeed;
        if ((Direction = (ARCleanDataStore.ModelAccess.Button_Right.transform.position - new Vector3(Screen.width * 1.05f, Screen.height * 0.5f))).sqrMagnitude > 1)
            ARCleanDataStore.ModelAccess.Button_Right.transform.position -= Direction * Time.deltaTime * RotationalArrowSpeed;
    }
}
