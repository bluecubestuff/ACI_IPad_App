using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FabricationDatabase : MonoBehaviour{

    //Lists for different meat types 
    public List<ChickenCuts> ChickenParts = new List<ChickenCuts>();
    public List<ShellfishCuts> ShellfishParts = new List<ShellfishCuts>();
    public List<FishCuts> FishParts = new List<FishCuts>();

    // Use this for initialization / Initialising the parts of meat for each of the list declared above
    void Start() { 
        //Chicken
        ChickenParts.Add(new ChickenCuts("Cut Off Chicken Head", 3.2f, 2f, 3.5f, -2.5f, "MeatFabrication/Chicken/1_Full Chicken","MeatFabrication/Chicken/2_Headless Chicken"
            , "You have successfully fabricated the chicken by removing the head", "MeatFabrication/Chicken/1_ChickenHeadHint", "Hint : Start from the head of the chicken\n[Cut from top to down]"));
        ChickenParts.Add(new ChickenCuts("Cut Off Both Chicken Feet", -2.6f, 2.6f, -2.2f, -2.4f, "MeatFabrication/Chicken/2_Headless Chicken", "MeatFabrication/Chicken/3_Legless Chicken"
            ,"You have successfully fabricated the chicken by removing both of the feet", "MeatFabrication/Chicken/2_ChickenLegHint", "Hint : Start from the legs of the chicken\n[Cut from top to down]"));
        ChickenParts.Add(new ChickenCuts("Cut Left Chicken  Backbone", -1.5f, 0.8f, 2.4f, 0.6f, "MeatFabrication/Chicken/3_Legless Chicken", "MeatFabrication/Chicken/4_Back Cut 1 Chicken"
            , "You have successfully fabricated the chicken by cutting the left side of the back", "MeatFabrication/Chicken/3_Back Cut Chicken Hint", "Hint : Start from leftside of the back\n[Cut from left to right]"));
        ChickenParts.Add(new ChickenCuts("Cut Right Chicken  Backbone", -2.3f, -0.5f, 2.6f, -0.5f, "MeatFabrication/Chicken/4_Back Cut 1 Chicken","MeatFabrication/Chicken/5_Back Cut 2 Chicken"
            , "You have successfully fabricated the chicken by cutting the right side of the back", "MeatFabrication/Chicken/4_Back Cut 2 Chicken Hint", "Hint : Start from rightside of the back\n[Cut from left to right]"));
        ChickenParts.Add(new ChickenCuts("Cut Left Chicken Breast", -0.8f, 0.6f, 2.6f, 0.7f, "MeatFabrication/Chicken/6_Chicken Chest","MeatFabrication/Chicken/7_Chicken Chest Cut 1"
            , "You have successfully fabricated the chicken by cutting the left side of the breast", "MeatFabrication/Chicken/6_Chicken Chest Hint", "Hint : Start from leftside of the breast\n[Cut from left to right]"));
        ChickenParts.Add(new ChickenCuts("Cut Right Chicken Breast", -0.9f, -0.6f, 3.1f, -0.8f, "MeatFabrication/Chicken/7_Chicken Chest Cut 1","MeatFabrication/Chicken/8_Chicken Chest Cut 2"
            , "You have successfully fabricated the chicken by cutting the right side of the breast", "MeatFabrication/Chicken/7_Chicken Chest Cut Hint", "Hint : Start from rightside of the breast\n[Cut from left to right]"));
        ChickenParts.Add(new ChickenCuts("Cut Left Chicken Wing & Thigh", 3.1f, 1.8f, -2.5f, 1.3f, "MeatFabrication/Chicken/8_Chicken Chest Cut 2","MeatFabrication/Chicken/10_Chicken Half Scallop and_Fillet_intact"
            , "You have successfully fabricated the chicken by cutting out the wing & thigh", "MeatFabrication/Chicken/8_Chicken Chest Cut 2 Hint", "Hint : Start from chicken wing joint to the thigh joint\n[Cut from right to left]"));
        ChickenParts.Add(new ChickenCuts("Cut out Right Chicken Scallop", -0.1f, 2.2f, -2.01f, 2.7f, "MeatFabrication/Chicken/10_Chicken Half Scallop and_Fillet_intact","MeatFabrication/Chicken/11_Chicken_Scallop_extract"
            , "You have successfully fabricated the chicken by cutting out the scallop", "MeatFabrication/Chicken/10_Chicken Scallop Hint", "Hint : Cut out the small piece of scallop\n[Cut from right to left]"));
        ChickenParts.Add(new ChickenCuts("Cut out Right Chicken Fillet", 3.5f, -0.14f, -1.381f, -1.78f, "MeatFabrication/Chicken/10_Chicken Half Scallop and_Fillet_intact","MeatFabrication/Chicken/12_Chicken_Fillet"
            , "You have successfully fabricated the chicken by cutting out the fillet", "MeatFabrication/Chicken/10_Chicken Fillet Hint", "Hint : Cut out the piece of fillet\n[Cut from right to left]"));
        ChickenParts.Add(new ChickenCuts("Cut off Right Chicken Drumstick", 1.3f, 1.6f, -0.281f, -1.28f, "MeatFabrication/Chicken/9_Half Chicken","MeatFabrication/Chicken/13_Chicken_Drum_stick"
            , "You have successfully fabricated the chicken by cutting off the drumstick", "MeatFabrication/Chicken/9_Half Chicken Hint", "Hint : Cut between the wing and drum stick\n[Cut from top to bottom]"));
        ChickenParts.Add(new ChickenCuts("Separate Chicken wing & Breast", -0.122f, 3.5f, -1.081f, -0.12f, "MeatFabrication/Chicken/14_Chicken_Wing_with_Breast","MeatFabrication/Chicken/15_SeperateBreastnWing"
            , "You have successfully fabricated the chicken by separating the wing and breast", "MeatFabrication/Chicken/14_Chicken_Wing_with_Breast Hint", "Hint : Cut the joint between breast and wing\n[Cut from top to bottom]"));
        ChickenParts.Add(new ChickenCuts("Cut Right Thigh Tendon", 3.66f, 4.1f, 3.98f, 0.75f, "MeatFabrication/Chicken/13_Chicken_Drum_stick", "MeatFabrication/Chicken/13_Chicken_Drum_stick Bone", "You have successfully cut the thigh tendon", "MeatFabrication/Chicken/13_Chicken_Drum_stick bone hint", "Hint : Cut the base of the thigh bone\n[Cut from top to bottom]"));
        ChickenParts.Add(new ChickenCuts("Follow the bone and cut to skin area", 3.1f, 1.67f, 1.2f, -0.092f, "MeatFabrication/Chicken/13_Chicken_Drum_stick Bone","MeatFabrication/Chicken/13_Chicken_Drum_stick after", "Continue by cutting until the end of the bone", "MeatFabrication/Chicken/13_Chicken_Drum_stick Hint", "Hint : Cut from the bone area halfway until where the skin ends\n[Cut from right to left]"));
        ChickenParts.Add(new ChickenCuts("Continue cutting until the bone ends", 0.67f, -0.79f, -4.9f, 1.42f, "MeatFabrication/Chicken/13_Chicken_Drum_stick after","MeatFabrication/Chicken/17_Chicken_Drum_stick_debone", "You have successfully fabricated the chicken by cutting open the drumstick", "MeatFabrication/Chicken/13_Chicken_Drum_stick after Hint", "Hint : Continue from previous cut until end of the bone\n[Cut from right to left]"));
        ChickenParts.Add(new ChickenCuts("Debone Right Thigh", 3f, 1.2f, -0.36f, -1.28f, "MeatFabrication/Chicken/17_Chicken_Drum_stick_debone","MeatFabrication/Chicken/17_Chicken_Drum_stick_debone 1", "Continue to debone the drum stick", "MeatFabrication/Chicken/17_Chicken_Drum_stick_debone Hint", "Hint : Debone until the middle of the chicken\n[Cut from right to left]"));
        ChickenParts.Add(new ChickenCuts("Debone Right Thigh", -0.89f, -1.46f, -4.58f, 0.043f, "MeatFabrication/Chicken/17_Chicken_Drum_stick_debone 1","MeatFabrication/Chicken/18_Chicken_Drum_Stick_Boneless", "You have deboned the drum stick", "MeatFabrication/Chicken/17_Chicken_Drum_stick_debone 1 Hint", "Hint : Continue from previous cut until end of the bone\n[Cut from right to left]"));

        //Other side
        ChickenParts.Add(new ChickenCuts("Cut out Left Chicken Scallop", 2.18f, 2.64f, 0.55f,2.26f, "MeatFabrication/Chicken/10_Chicken Half Scallop and_Fillet_intact", "MeatFabrication/Chicken/11_Chicken_Scallop_extract", "You have successfully fabricated the chicken by cutting out the scallop", "MeatFabrication/Chicken/10_Chicken Scallop Hint", "Hint : Cut out the small piece of scallop\n[Cut from left to right]",true));
        ChickenParts.Add(new ChickenCuts("Cut out Left Chicken Fillet", -3.55f, 0.18f, 2.55f, -1.83f, "MeatFabrication/Chicken/10_Chicken Half Scallop and_Fillet_intact", "MeatFabrication/Chicken/12_Chicken_Fillet", "You have successfully fabricated the chicken by cutting out the fillet", "MeatFabrication/Chicken/10_Chicken Fillet Hint", "Hint : Cut out the piece of fillet\n[Cut from left to right]",true));
        ChickenParts.Add(new ChickenCuts("Cut off Left Chicken Drumstick", -1.49f, 3.97f, 1.068f, -1.87f, "MeatFabrication/Chicken/9_Half Chicken", "MeatFabrication/Chicken/13_Chicken_Drum_stick", "You have successfully fabricated the chicken by cutting off the drumstick", "MeatFabrication/Chicken/9_Half Chicken Hint", "Hint : Cut between the wing and drum stick\n[Cut from top to bottom]",true));
        ChickenParts.Add(new ChickenCuts("Separate Chicken Wing & Breast", -0.45f, 3.8f, 1.9f, -1.07f, "MeatFabrication/Chicken/14_Chicken_Wing_with_Breast", "MeatFabrication/Chicken/15_SeperateBreastnWing", "You have successfully fabricated the chicken by separating the wing and breast", "MeatFabrication/Chicken/14_Chicken_Wing_with_Breast Hint", "Hint : Cut the joint between breast and wing\n[Cut from top to bottom]",true));
        ChickenParts.Add(new ChickenCuts("Cut Left Thigh Tendon", -3.31f, 3.8f, -4.2f, 1.03f, "MeatFabrication/Chicken/13_Chicken_Drum_stick", "MeatFabrication/Chicken/13_Chicken_Drum_stick Bone", "You have successfully cut the thigh tendon", "MeatFabrication/Chicken/13_Chicken_Drum_stick bone hint", "Hint : Cut the base of the thigh bone\n[Cut from top to bottom]",true));

        ChickenParts.Add(new ChickenCuts("Follow the bone and cut to skin area", -2.71f, 1.89f, 0.865f, -0.445f, "MeatFabrication/Chicken/13_Chicken_Drum_stick Bone", "MeatFabrication/Chicken/13_Chicken_Drum_stick after", "Continue by cutting until the end of the bone", "MeatFabrication/Chicken/13_Chicken_Drum_stick Hint", "Hint : Cut from the bone area halfway until where the skin ends\n[Cut from left to right]",true));
        ChickenParts.Add(new ChickenCuts("Continue cutting until the bone ends", 0.13f, -0.09f, 5, 1.32f, "MeatFabrication/Chicken/13_Chicken_Drum_stick after", "MeatFabrication/Chicken/17_Chicken_Drum_stick_debone", "You have successfully fabricated the chicken by cutting open the drumstick", "MeatFabrication/Chicken/13_Chicken_Drum_stick after Hint", "Hint : Continue from previous cut until end of the bone\n[Cut from left to right]",true));
        ChickenParts.Add(new ChickenCuts("Debone Left Thigh", -2.43f, 0.89f, 0.8f, -1.6f, "MeatFabrication/Chicken/17_Chicken_Drum_stick_debone", "MeatFabrication/Chicken/17_Chicken_Drum_stick_debone 1", "Continue to debone the drum stick", "MeatFabrication/Chicken/17_Chicken_Drum_stick_debone Hint", "Hint : Debone until the middle of the chicken\n[Cut from left to right]",true));
        ChickenParts.Add(new ChickenCuts("Debone Left Thigh", 1.3f, -1.6f, 5.56f, 0.57f, "MeatFabrication/Chicken/17_Chicken_Drum_stick_debone 1", "MeatFabrication/Chicken/18_Chicken_Drum_Stick_Boneless", "You have deboned the drum stick", "MeatFabrication/Chicken/17_Chicken_Drum_stick_debone 1 Hint", "Hint : Continue from previous cut until end of the bone\n[Cut from left to right]",true));

        //Shellfish
        ShellfishParts.Add(new ShellfishCuts("Kill Crab", 0.09f, 1.49f, 0.15f, -1.87f, "1_Full Crab", "2_Dead Crab","You have successfully fabricated the crab by killing it", "1_Full Crab Hint", "Hint : Look for an inverted V on the crab\n[Cut from top to down]"));
        ShellfishParts.Add(new ShellfishCuts("Remove Crab Shell", 0.19f, -1.7f, 0.4f, 2.5f, "2.1_Full Crab Top", "2.2_Full Crab Top Removed", "You have successfully fabricated the crab by removing its shell", "2.1_Full Crab Top Hint", "Hint : Drag from bottom middle of the shell to pull out the shell\n[Drag from bottom to top]"));
        ShellfishParts.Add(new ShellfishCuts("Chop Crab in Half", -0.2f, 1.2f, 0.0f, -3.3f, "2.2_Full Crab Top Removed", "4_Crab Legs and Claw", "You have successfully fabricated the crab by cutting it in half", "2.2_Full Crab Top Removed Hint", "Hint : Cut the middle\n[Cut from top to down]"));
        ShellfishParts.Add(new ShellfishCuts("Get rid of shell on left side", -2.26f, 2.93f, -5.01f, -0.95f, "5_Crab Shell", "5_Crab Shell", "You have successfully fabricated the crab by removing left inner shell", "5_Crab Shell Left Hint", "Hint : Get rid of the inner shell on the left\n[Cut from right to left]"));
        ShellfishParts.Add(new ShellfishCuts("Get rid of shell on right side", 1.45f, 3.48f, 4.72f, -1.03f, "5_Crab Shell", "6_Crab Shell corner removed", "You have successfully fabricated the crab by removing right inner shell", "5_Crab Shell Left Hint", "Hint : Get rid of inner shell on the right\n[Cut from left to right]",true));
        ShellfishParts.Add(new ShellfishCuts("Cut off right crab pincers", -1.48f, 2f, -1.26f, -0.5f, "4_Crab Legs and Claw", "8_Crab Claw", "You have successfully fabricated the crab by cutting off the right pincers", "4_Pincer Hint", "Hint : Cut off the huge pincers\n[Cut from top of pincer's base to bottom]"));
        ShellfishParts.Add(new ShellfishCuts("Chop the right crab legs", 1.48f, 1.8f, -0.366f, -2.05f, "9_Crab Legs", "10_Crab Legs Chopped", "You have successfully fabricated the crab by cutting off the legs", "9_Crab Legs Hint", "Hint : Cut off the rest of the legs\n[Cut from top to bottom]"));
        ShellfishParts.Add(new ShellfishCuts("Break right Crab Claw", -0.4f, 2.4f, -0.4f, 2.4f, "11_Crab Claw", "12_Crab Claw Cracked","You have successfully fabricated the crab by breaking its claw", "11_Crab Claw Hint", "Hint : Tap on the middle of the claw"));
        ShellfishParts.Add(new ShellfishCuts("Cut off left crab pincers", 1.64f, 1f, 1.45f, -0.95f, "4_Crab Legs and Claw", "8_Crab Claw", "You have successfully fabricated the crab by cutting off the left pincers", "4_Pincer Hint", "Hint : Cut off the huge pincers\n[Cut from top of pincer's base to bottom]",true));
        ShellfishParts.Add(new ShellfishCuts("Cut off left crab legs", -0.66f, 1.87f, 0.84f, -2.074f,"9_Crab Legs", "10_Crab Legs Chopped", "You have successfully fabricated the crab by cutting off the legs", "9_Crab Legs Hint", "Hint : Cut off the rest of the legs\n[Cut from top to bottom]", true));
        ShellfishParts.Add(new ShellfishCuts("Break left Crab Claw", -0.4f, 2.4f, -0.4f, 2.4f, "11_Crab Claw", "12_Crab Claw Cracked", "You have successfully fabricated the crab by breaking its claw", "11_Crab Claw Hint", "Hint : Tap on the middle of the claw"));

        //Fish
        FishParts.Add(new FishCuts("Cut open Fish Stomach", -2.7f, -1.1f, 2.2f, -1.0f));
        FishParts.Add(new FishCuts("Debone Fish", 3.6f, -0.2f, -4.6f, -0.8f));
    }

}
