using UnityEngine;
using System.Collections;

//For storing classes of the different cuts of meat
public class MeatFabricationData {

    //public string FabName;
    //public float startCutPointX, endCutPointX, startCutPointY, endCutPointY;
    //public int FabNumOfCuts;

    //public MeatFabricationData(string name, float startPosX, float endPosX, float startPosY, float endPosY, int numOfCuts)
    //{
    //    FabName = name;
    //    startCutPointX = startPosX;
    //    endCutPointX = endPosX;
    //    startCutPointY = startPosY;
    //    endCutPointY = endPosY;
    //    FabNumOfCuts = numOfCuts;
    //}

    //public MeatFabricationData()
    //{

    //}
}

//Class for the different cuts of chicken
public class ChickenCuts
{
    public string ChickenName;
    public float startCutPointX, endCutPointX, startCutPointY, endCutPointY;
    public string defaultImage,correctImage, wrongImage;
    public string correctText,wrongText;
    public bool imageRotate;

    public ChickenCuts(string name, float startPosX, float startPosY, float endPosX, float endPosY, string defaultImageA, string correctImageA, string correctTextA, string wrongImageA, string wrongTextA, bool needToRotateImage = false)
    {
        ChickenName = name;
        startCutPointX = startPosX;
        startCutPointY = startPosY;
        endCutPointX = endPosX;
        endCutPointY = endPosY;
        wrongImage = wrongImageA;
        wrongText = wrongTextA;
        correctImage = correctImageA;
        correctText = correctTextA;
        defaultImage = defaultImageA;
        imageRotate = needToRotateImage;
    }

    public ChickenCuts()
    {

    }
}
//Class for the different cuts of shellfish
public class ShellfishCuts
{
    public string ShellfishName;
    public float startCutPointX, endCutPointX, startCutPointY, endCutPointY;
    public string defaultImage, correctImage, wrongImage;
    public string correctText, wrongText;
    public bool imageRotate;

    public ShellfishCuts(string name, float startPosX, float startPosY, float endPosX, float endPosY, string defaultImageA, string correctImageA, string correctTextA, string wrongImageA, string wrongTextA, bool needToRotateImage = false)
    {
        ShellfishName = name;
        startCutPointX = startPosX;
        startCutPointY = startPosY;
        endCutPointX = endPosX;
        endCutPointY = endPosY;
        wrongImage = wrongImageA;
        wrongText = wrongTextA;
        correctImage = correctImageA;
        correctText = correctTextA;
        defaultImage = defaultImageA;
        imageRotate = needToRotateImage;
    }

    public ShellfishCuts()
    {

    }
}
//Class for the different cuts of fish
public class FishCuts
{
    public string FishName;
    public float startCutPointX, endCutPointX, startCutPointY, endCutPointY;

    public FishCuts(string name, float startPosX, float startPosY, float endPosX, float endPosY)
    {
        FishName = name;
        startCutPointX = startPosX;
        startCutPointY = startPosY;
        endCutPointX = endPosX;
        endCutPointY = endPosY;
    }

    public FishCuts()
    {

    }
}
