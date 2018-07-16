using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class MeatFabricationScript2 : MonoBehaviour
{
    enum State
    {
        Idle,
        Ready,
        Ongoing,
    }
    public static bool fabricatedMeat = false;

    public TMP_Dropdown stepsDropdown;
    public SpriteRenderer meatImage;

    public GameObject
        successWindow,
        failWindow,
        completeWindow;

    public GameObject
        instructionWindow;

    public Sprite tryAgainSprite;

    public Image
        successImage,
        hintImage;

    public TextMeshProUGUI
        successText,
        failText,
        hintText,
        completeText;

    public GameObject hintPoint;
    public EventTrigger clickArea;

    List<GameObject> hintPoints = new List<GameObject>();

    public AudioManager audioManager;

    public AudioClip
        applauseSound,
        sliceSound,
        failSound;

    State state;
    int pointerID;

    FabricationHandler2 handler;
    FabricationDatabase2 database;
    
    int meatTypeIndex;
    int stepIndex;

    int incorrectCount;

	// Use this for initialization
	void Start ()
    {
        fabricatedMeat = true;

        handler = GetComponent<FabricationHandler2>();
        database = GetComponent<FabricationDatabase2>();
        stepsDropdown.onValueChanged.AddListener(data => OnDropdownChange(data));
        meatTypeIndex = -1;

        EventTrigger.Entry e = new EventTrigger.Entry();
        e.eventID = EventTriggerType.PointerDown;
        e.callback.AddListener(data => OnClickedArea((PointerEventData)data));
        clickArea.triggers.Add(e);

        e = new EventTrigger.Entry();
        e.eventID = EventTriggerType.Drag;
        e.callback.AddListener(data => OnDrag((PointerEventData)data));
        clickArea.triggers.Add(e);

        e = new EventTrigger.Entry();
        e.eventID = EventTriggerType.PointerUp;
        e.callback.AddListener(data => OnRelease((PointerEventData)data));
        clickArea.triggers.Add(e);

        ResetLayout();

        instructionWindow.SetActive(true);
	}
	
	// Update is called once per frame
	void Update ()
    {
        
	}

    public void SetMeatType(string name)
    {
        stepsDropdown.gameObject.SetActive(true);

        int index = database.meatTypes.FindIndex(q => (q.name == name));

        if (index != -1)
        {
            meatTypeIndex = index;
            
            ResetLayout();
        }
    }
    public void SuccessCloseButton()
    {
        if (meatTypeIndex == -1) return;

        if (stepIndex < database.meatTypes[meatTypeIndex].steps.Count - 1)
            NextStep();
        else
            FabricationComplete();
    }
    public void FailCloseButton()
    {
        if (meatTypeIndex == -1) return;

        if (incorrectCount >= 3)
        {
            foreach (var p in hintPoints)
                p.SetActive(true);
        }

        state = State.Ready;
    }

    void OnDropdownChange(int n)
    {
        //Debug.Log(n);
        stepIndex = n;
        ResetFabrication();
    }
    void OnClickedArea(PointerEventData e)
    {
        //Debug.Log("Click: " + e.position + ", ID: " + e.pointerId);
        e.useDragThreshold = false;

        if (state == State.Ready)
        {
            pointerID = e.pointerId;
            state = State.Ongoing;

            handler.StartMethod(e.position);
        }
    }
    void OnDrag(PointerEventData e)
    {
        //Debug.Log("Drag: " + e.position);

        if (state == State.Ongoing)
        {
            if (e.pointerId == pointerID)
                handler.UpdateMethod(e.position);
        }
    }
    void OnRelease(PointerEventData e)
    {
        //Debug.Log("Release: " + e.position);

        if (meatTypeIndex != -1 && state == State.Ongoing)
        {
            if (e.pointerId == pointerID)
            {
                state = State.Idle;
                handler.EndMethod(e.position);
                
                switch (handler.state)
                {
                    case FabricationHandler2.State.Pass:
                        StepSuccess();
                        break;

                    case FabricationHandler2.State.Fail:
                        StepFail();
                        break;

                    default:
                        state = State.Ready;
                        break;
                }
            }
        }
    }

    void ResetLayout()
    {
        stepsDropdown.ClearOptions();

        if (meatTypeIndex == -1 || meatTypeIndex >= database.meatTypes.Count)
        {
            meatTypeIndex = -1;
            stepsDropdown.options.Add(new TMP_Dropdown.OptionData("(Choose fabrication step)"));
        }
        
        if (meatTypeIndex != -1)
        {
            foreach (var s in database.meatTypes[meatTypeIndex].steps)
                stepsDropdown.options.Add(new TMP_Dropdown.OptionData(s.name));
        }

        SetStep(0);
    }
    void ResetFabrication()
    {
        stepsDropdown.value = stepIndex;
        stepsDropdown.RefreshShownValue();

        RefreshMeatImage();

        incorrectCount = 0;
        
        ResetSlicePoints();
        if (meatTypeIndex != -1)
        {
            CreatePointsToHandler();
            state = State.Ready;
        }
        else
            state = State.Idle;

        handler.state = FabricationHandler2.State.Idle;
    }
    void ResetSlicePoints()
    {
        foreach (var p in hintPoints)
            Destroy(p);
        hintPoints.Clear();

        handler.initPoints.Clear();
    }
    void RefreshMeatImage()
    {
        Sprite sp = null;

        meatImage.transform.localRotation = Quaternion.identity;

        if (meatTypeIndex != -1)
        {
            var st = database.meatTypes[meatTypeIndex].steps[stepIndex];
            sp = st.startingImage;

            if (st.mirrored)
                meatImage.transform.localRotation *= new Quaternion(0.0f, 1.0f, 0.0f, 0.0f);
            if (st.flipped)
                meatImage.transform.localRotation *= new Quaternion(1.0f, 0.0f, 0.0f, 0.0f);

            //if (sp) Debug.Log(sp.name);
        }

        meatImage.sprite = sp;
    }

    void StepSuccess()
    {
        audioManager.PlaySFX(sliceSound);

        var s = database.meatTypes[meatTypeIndex].steps[stepIndex];

        successImage.sprite = s.resultImage;
        successImage.transform.localRotation = Quaternion.identity;
        if (s.mirrored)
            successImage.transform.localRotation *= new Quaternion(0.0f, 1.0f, 0.0f, 0.0f);
        if (s.flipped)
            successImage.transform.localRotation *= new Quaternion(1.0f, 0.0f, 0.0f, 0.0f);
        successText.text = database.meatTypes[meatTypeIndex].steps[stepIndex].resultText;

        successWindow.SetActive(true);
        
        if (database.meatTypes[meatTypeIndex].steps.Count - 1 == stepIndex)
            stepsDropdown.gameObject.SetActive(false);
    }
    void StepFail()
    {
        audioManager.PlaySFX(failSound);

        var m = database.meatTypes[meatTypeIndex];

        ++incorrectCount;
        failText.text = m.incorrectText;
        hintText.text = m.steps[stepIndex].hintText;
        hintImage.transform.localRotation = Quaternion.identity;

        //Debug.Log("Steps: " + incorrectCount + ", Image: " + m.steps[stepIndex].hintImage);
        if (incorrectCount >= 3 && m.steps[stepIndex].hintImage != null)
        {
            var s = m.steps[stepIndex];

            hintImage.sprite = s.hintImage;
            if (s.mirrored)
                hintImage.transform.localRotation = new Quaternion(0.0f, 1.0f, 0.0f, 0.0f);
            if (s.flipped)
                hintImage.transform.localRotation = new Quaternion(1.0f, 0.0f, 0.0f, 0.0f);
        }
        else
            hintImage.sprite = tryAgainSprite;

        failWindow.SetActive(true);
    }

    void SetStep(int n)
    {
        stepIndex = n;

        ResetFabrication();
    }
    void NextStep()
    {
        SetStep(stepIndex + 1);
    }
    void FabricationComplete()
    {
        audioManager.PlaySFX(applauseSound);

        completeText.text = database.meatTypes[meatTypeIndex].completeText;
        completeWindow.SetActive(true);

        meatTypeIndex = -1;
        ResetLayout();
    }

    void CreatePointsToHandler()
    {
        handler.sliceReversable = database.meatTypes[meatTypeIndex].steps[stepIndex].reversable;

        var pa = database.meatTypes[meatTypeIndex].steps[stepIndex].points;

        for (int i = 0; i < pa.Count; ++i)
        {
            GameObject o;
            if (hintPoint == null)
                o = new GameObject();
            else
                o = Instantiate(hintPoint);

            o.transform.position = (Vector3)pa[i] + new Vector3(0.0f, 0.0f, hintPoint.transform.localPosition.z);
            o.transform.SetParent(transform);
            hintPoints.Add(o);
            handler.initPoints.Add(o.transform);
        }
    }
}
