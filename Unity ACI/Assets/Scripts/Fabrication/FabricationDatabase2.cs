using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct FabricationStep
{
    public string name;
    public List<Vector2> points;
    public bool reversable;
    [Header("Text & Image Data")]
    public Sprite startingImage;
    public bool
        mirrored,
        flipped;
    public Sprite resultImage;
    [TextArea]
    public string resultText;
    public Sprite hintImage;
    [TextArea]
    public string hintText;

}

[Serializable]
public struct FabricationMeatType
{
    public string name;

    [TextArea]
    public string
        completeText,
        incorrectText;
    public List<FabricationStep> steps;
}

public class FabricationDatabase2 : MonoBehaviour
{
    public List<FabricationMeatType> meatTypes;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
