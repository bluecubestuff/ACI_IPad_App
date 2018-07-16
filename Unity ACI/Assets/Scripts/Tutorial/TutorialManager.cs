using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager
{
    public enum Page
    {
        //Prompt
        Prompt,
        //Game Introduction
        Restaurant_Intro1,
        Restaurant_Intro2,
        //Go to Suppliers
        Restaurant_ToSuppliers,

        //Enter Shop
        Suppliers_EnterShop,
        //How to Buy Food
        Suppliers_Instructions1,
        Suppliers_Instructions2,
        //Buy Food
        Suppliers_BuyFood,
        //How Food are Sent
        Suppliers_InfoStorage,
        //Go to Storage
        Suppliers_GoToStorage,

        //Check Delivery
        Storage_CheckDelivery,
        //About Stored Food
        Storage_StoredFood,
        //Return to Diner
        Storage_ReturnToDiner,

        //Gameplay Mechanics
        Restaurant_Info1,
        Restaurant_Info2,
        Restaurant_Info3,
        //End of Tutorial
        Restaurant_End,

        //Complete
        Complete,
    }

    public static Page page = Page.Prompt;
}
