﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalInventory : MonoBehaviour {
    #region PrivateVariables

    public static GlobalInventory staticInventory;

    [SerializeField] PlayerInventory playerInven;
    [SerializeField] ExampleVariableStorage variables;

    [Header("Tools")]
    [SerializeField] bool haveCan;
    [SerializeField] bool haveSeeds, haveSaw, haveClippers;

    [Header("Items")]
    [SerializeField] bool bracelet;
    [SerializeField] bool antlers;

    #endregion
    #region PublicProperties
    public bool HaveCan        { get { return haveCan; }      set { GetItem(0); haveCan = value; } }
    public bool HaveSeeds       { get { return haveSeeds; }     set { GetItem(1); haveSeeds = value; } }
    public bool HaveSaw         { get { return haveSaw; }       set { GetItem(2); haveSaw = value; } }
    public bool HaveClippers    { get { return haveClippers; }  set { GetItem(3); haveClippers = value; } }

    public bool Bracelet        { get { return bracelet; }      set { GetItem(4); bracelet = value; } }
    public bool Antlers         { get { return antlers; }       set { GetItem(5); antlers = value; } }
    #endregion
    #region UnityFunctions
    void Start () {
        staticInventory = this;
    }
    void Update () {
	
    }
    #endregion
    #region CustomFunctions
    void GetItem(int itemIndex)
    {
        switch (itemIndex)
        {
            case 0:                                                 //YOU GOT THE WATERING CAN
                playerInven.GetTool(1);
                string variableName = "have_watering_can";
                foreach (ExampleVariableStorage.DefaultVariable var in variables.defaultVariables)
                {
                    if (var.name == variableName)
                    {
                        object value = true;
                        var v = new Yarn.Value(value);
                        variables.SetValue("$" + var.name, v);
                    }
                }
                break;
            case 1:                                                 //YOU GOT THE SEED BAG
                playerInven.GetTool(2);
                break;
            case 2:                                                 //YOU GOT THE SAW
                break;
            case 3:                                                 //YOU GOT THE CLIPPERS
                break;
            case 4:                                                 //YOU GOT THE BRACELET
                GetBracelet();
                break;
            case 5:                                                 //YOU GOT THE ANTLERS
                break;
        }
    }
    #region Individual Item Functions

    void GetBracelet()
    {
        playerInven.HaveBracelet = true;

    }


    #endregion
    #endregion
}