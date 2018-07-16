/* 
 * Author: Lim Rui An Ryan
 * Filename: ARCleanInstructionsScreen.cs
 * Description: A script that handles the showing of the instruction pages within instructions scene
 */
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ARCleanInstructionsScreen : MonoBehaviour, IPointerClickHandler
{
    // Public Variables
    public float RateOfIncrement = 5f;
    public float InterfaceMovementSpeed = 5f;
    public float WaitTime = 1f;

    // Private Variables
    private const int TutorialPageIndex = 1;
    [SerializeField] private GameObject Tutorial_Page;
    [SerializeField] private GameObject Hints_Page;
    [SerializeField] private TransitionSystem LinkedTransit;
    [SerializeField] private List<GameObject> InstructionPages;
    private int PagePhase = 0;
    private Vector3 HintOrigin;
    private Vector3 TutorialOrigin;
    private Vector3 HintTarget = new Vector3(0, 0, Screen.height * 3f);
    private Vector3 TutorialTarget = new Vector3(0, 0, Screen.height * 3f);
    private float Timer = 0f;
	
    void Start(){
        TutorialOrigin = Tutorial_Page.GetComponent<RectTransform>().position;
        HintOrigin = Hints_Page.GetComponent<RectTransform>().position;
        Hints_Page.GetComponent<RectTransform>().position = HintTarget;
        Tutorial_Page.GetComponent<RectTransform>().position = TutorialTarget;
    }

	// Update is called once per frame
	void Update () {
        Timer += Time.deltaTime;
        DisplayInterface(InstructionPages[0], PagePhase == 0);
        MoveDownInterface(Tutorial_Page, PagePhase == TutorialPageIndex, HintOrigin, HintTarget);
        for (int i = 1; i < InstructionPages.Count; ++i)
            DisplayInterface(InstructionPages[i], PagePhase - TutorialPageIndex == i);
        MoveDownInterface(Hints_Page, PagePhase== InstructionPages.Count + TutorialPageIndex, HintOrigin, HintTarget);
    }

    public void MoveDownInterface(GameObject Page, bool MoveDown, Vector3 Origin, Vector3 Target)
    {
        Vector3 NewTarget = Vector3.zero;
        if (MoveDown)
            NewTarget = Origin;
        else NewTarget = Target;
        Vector3 Direction = Page.GetComponent<RectTransform>().position - NewTarget;
        if (Direction.sqrMagnitude > 1)
            Page.GetComponent<RectTransform>().position -= Direction * Time.deltaTime * InterfaceMovementSpeed;
    }

    public void SetTutorialSetting(bool Enable)
    {
        if (!Enable)
            PagePhase = InstructionPages.Count + TutorialPageIndex;
        else if (PagePhase <= TutorialPageIndex)
            PagePhase++;
    }

    public void SetHintSetting(bool Enable){
        ARCleanDataStore.ShowHints = Enable;
        LinkedTransit.IncrementScene();
    }

    private void DisplayInterface(GameObject InterfaceElement, bool Display){
        Image InterfaceImage = InterfaceElement.GetComponent<Image>();
        if (InterfaceImage == null)
            return;
            if (Display && InterfaceImage.color.a < 1)
                InterfaceImage.color = new Color(InterfaceImage.color.r, InterfaceImage.color.g, InterfaceImage.color.b, InterfaceImage.color.a + RateOfIncrement * Time.deltaTime);
        else if (!Display && InterfaceImage.color.a > 0)
            InterfaceImage.color = new Color(InterfaceImage.color.r, InterfaceImage.color.g, InterfaceImage.color.b, InterfaceImage.color.a - RateOfIncrement * Time.deltaTime);
    }

    // Input Handling
    public void OnPointerClick(PointerEventData eventData)
    {
        if (PagePhase != TutorialPageIndex)
            if (PagePhase <= InstructionPages.Count && Timer > WaitTime)
            {
                Timer = 0;
                PagePhase++;
            }
    }
}
