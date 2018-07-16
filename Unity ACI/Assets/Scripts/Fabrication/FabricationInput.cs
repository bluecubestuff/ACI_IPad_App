using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FabricationInput : MonoBehaviour {
    
    public EventTrigger clickArea;
    public FabricationHandler2 fabricationHandler;

	// Use this for initialization
	void Start ()
    {
        //fabricationManager.method = FabricationManager2.Method.Tap;
        
        EventTrigger.Entry e = new EventTrigger.Entry();
        e.eventID = EventTriggerType.PointerDown;
        e.callback.AddListener(data => OnPointerDown((PointerEventData)data));
        clickArea.triggers.Add(e);

        e = new EventTrigger.Entry();
        e.eventID = EventTriggerType.PointerUp;
        e.callback.AddListener(data => OnPointerUp((PointerEventData)data));
        clickArea.triggers.Add(e);
    }
	
	// Update is called once per frame
	void Update ()
    {
        fabricationHandler.UpdateMethod(Input.mousePosition);
	}

    void OnPointerDown(PointerEventData e)
    {
        fabricationHandler.StartMethod(e.position);
    }
    void OnPointerUp(PointerEventData e)
    {
        fabricationHandler.EndMethod(e.position);
    }
}
